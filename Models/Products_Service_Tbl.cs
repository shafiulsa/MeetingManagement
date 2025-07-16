using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingSet.Models
{
    public class Products_Service_Tbl
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Unit { get; set; }
    }
}