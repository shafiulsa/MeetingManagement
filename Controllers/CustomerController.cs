using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MeetingSet.Data;
using MeetingSet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MeetingSet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(AppDbContext context, ILogger<CustomerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetCustomers")]
        public async Task<IActionResult> GetCustomers(string type)
        {
            try
            {
                if (type == "Corporate")
                {
                    var corporateCustomers = await _context.Corporate_Customer_Tbls
                        .Select(c => new { c.Id, c.Name })
                        .ToListAsync();
                    return Ok(corporateCustomers);
                }
                else if (type == "Individual")
                {
                    var individualCustomers = await _context.Individual_Customer_Tbls
                        .Select(c => new { c.Id, c.Name })
                        .ToListAsync();
                    return Ok(individualCustomers);
                }

                return BadRequest("Invalid customer type");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customers");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> SaveMeetingMinutes([FromBody] MeetingMinutesFormData formData)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Validate and parse input
                if (!DateTime.TryParse(formData.MeetingDate, out var meetingDate))
                    return BadRequest("Invalid meeting date format");

                if (!TimeSpan.TryParse(formData.MeetingTime, out var meetingTime))
                    return BadRequest("Invalid meeting time format");

                DateTime meetingDateTime = meetingDate.Add(meetingTime);

                // 1. Save to Master table and get the new ID
                var masterIdParam = new SqlParameter("@NewId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync(
                    @"EXEC Meeting_Minutes_Master_Save_SP 
                    @CustomerType={0}, 
                    @CustomerId={1}, 
                    @MeetingDateTime={2}, 
                    @MeetingPlace={3}, 
                    @ClientAttendees={4}, 
                    @HostAttendees={5},
                    @NewId={6} OUTPUT",
                    formData.CustomerType,
                    formData.CustomerId,
                    meetingDateTime,
                    formData.MeetingPlace,
                    formData.ClientAttendees,
                    formData.HostAttendees,
                    masterIdParam);

                int masterId = (int)masterIdParam.Value;

                // 2. Save to Details table
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $@"EXEC Meeting_Minutes_Details_Save_SP 
                    @MasterId={masterId}, 
                    @Agenda={formData.Agenda}, 
                    @Discussion={formData.Discussion}, 
                    @Decision={formData.Decision}");

                await transaction.CommitAsync();
                return Ok(new { success = true, message = "Meeting minutes saved successfully" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error saving meeting minutes");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpGet("GetMeetingMinutes")]
        public async Task<IActionResult> GetMeetingMinutes()
        {
            try
            {
                var meetingMinutes = await _context.Meeting_Minutes_Master_Tbls
                    .Join(_context.Meeting_Minutes_Details_Tbls,
                        master => master.Id,
                        detail => detail.MasterId,
                        (master, detail) => new MeetingMinutesViewModel
                        {
                            Id = master.Id,
                            CustomerType = master.CustomerType,
                            CustomerName = master.CustomerType == "Corporate"
                                ? (_context.Corporate_Customer_Tbls
                                    .Where(c => c.Id == master.CustomerId)
                                    .Select(c => c.Name)
                                    .FirstOrDefault() ?? string.Empty)
                                : (_context.Individual_Customer_Tbls
                                    .Where(c => c.Id == master.CustomerId)
                                    .Select(c => c.Name)
                                    .FirstOrDefault() ?? string.Empty),
                            MeetingDateTime = master.MeetingDateTime,
                            MeetingPlace = master.MeetingPlace,
                            ClientAttendees = master.ClientAttendees,
                            HostAttendees = master.HostAttendees,
                            Agenda = detail.Agenda,
                            Discussion = detail.Discussion,
                            Decision = detail.Decision
                        })
                    .OrderByDescending(m => m.MeetingDateTime)
                    .ToListAsync();

                return Ok(meetingMinutes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }


        }
    }
}