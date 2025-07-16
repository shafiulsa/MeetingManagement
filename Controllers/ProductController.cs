using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetingSet.Data;
using MeetingSet.Models;

namespace MeetingSet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products_Service_Tbls
                .Select(p => new { p.Id, p.Name, p.Unit })
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("DisplayProducts")]
        public async Task<IActionResult> GetDisplayProducts()
        {
            var displayProducts = await _context.DisplayProducts
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Quantity,
                    p.Unit
                })
                .ToListAsync();

            return Ok(displayProducts);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] DisplayProduct dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || 
                string.IsNullOrWhiteSpace(dto.Quantity) || string.IsNullOrWhiteSpace(dto.Unit))
                return BadRequest("Invalid data.");

            var entity = new DisplayProduct
            {
                Name = dto.Name,
                Quantity = dto.Quantity,
                Unit = dto.Unit
            };

            await _context.DisplayProducts.AddAsync(entity);
            await _context.SaveChangesAsync();

            return Ok(new { entity.Id });
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] DisplayProduct updated)
        {
            if (updated == null || string.IsNullOrWhiteSpace(updated.Name) || 
                string.IsNullOrWhiteSpace(updated.Quantity) || string.IsNullOrWhiteSpace(updated.Unit))
                return BadRequest("Invalid data.");

            var product = await _context.DisplayProducts.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            product.Name = updated.Name;
            product.Quantity = updated.Quantity;
            product.Unit = updated.Unit;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.DisplayProducts.FindAsync(id);
            if (entity == null) return NotFound();

            _context.DisplayProducts.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}