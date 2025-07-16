using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingSet.Models
{
    public class MeetingMinutesViewModel
    {
        public int Id { get; set; }
        public string? CustomerType { get; set; }
        public string? CustomerName { get; set; }
        public DateTime MeetingDateTime { get; set; }
        public string? MeetingPlace { get; set; }
        public string? ClientAttendees { get; set; }
        public string? HostAttendees { get; set; }
        public string? Agenda { get; set; }
        public string? Discussion { get; set; }
        public string? Decision { get; set; }
    }
}