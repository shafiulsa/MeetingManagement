using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingSet.Models
{
    public class Meeting_Minutes_Master_Tbl
    {
         public int Id { get; set; }
        public string CustomerType { get; set; } = string.Empty; // Corporate / Individual
        public int CustomerId { get; set; }
        public DateTime MeetingDateTime { get; set; }
        public string MeetingPlace { get; set; } = string.Empty;
        public string ClientAttendees { get; set; } = string.Empty;
        public string HostAttendees { get; set; } = string.Empty;

    }
}