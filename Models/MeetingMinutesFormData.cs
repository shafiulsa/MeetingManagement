using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingSet.Models
{
    public class MeetingMinutesFormData
    {
     // Master table fields (first part)
    public string? CustomerType { get; set; }
    public int CustomerId { get; set; }
    public string? MeetingDate { get; set; }  // As string? from form
    public string? MeetingTime { get; set; }  // As string? from form
    public string? MeetingPlace { get; set; }
    public string? ClientAttendees { get; set; }
    public string? HostAttendees { get; set; }
    
    // Detail table fields (second part)
    public string? Agenda { get; set; }
    public string? Discussion { get; set; }
    public string? Decision { get; set; }
    }
}