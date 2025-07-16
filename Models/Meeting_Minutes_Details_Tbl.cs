using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingSet.Models
{
    public class Meeting_Minutes_Details_Tbl
    {
        public int Id { get; set; }
        //Foreign key to Master
        public int MasterId { get; set; }
        public string Agenda { get; set; } = string.Empty;
        public string Discussion { get; set; } = string.Empty;
        public string Decision { get; set; } = string.Empty;
    }
}