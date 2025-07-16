using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetingSet.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingSet.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Corporate_Customer_Tbl> Corporate_Customer_Tbls { get; set; }
        public DbSet<Individual_Customer_Tbl> Individual_Customer_Tbls { get; set; }
        public DbSet<Products_Service_Tbl> Products_Service_Tbls { get; set; }
        public DbSet<DisplayProduct> DisplayProducts { get; set; }
        public DbSet<Meeting_Minutes_Master_Tbl> Meeting_Minutes_Master_Tbls { get; set; }
        public DbSet<Meeting_Minutes_Details_Tbl> Meeting_Minutes_Details_Tbls { get; set; }

    }
}

