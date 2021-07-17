using Microsoft.EntityFrameworkCore;
using RateMyAir.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateMyAir.Repository
{
    public class DatabaseContext : DbContext
    {
        public DbSet<AirData> AirData { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=D:\\ProgettiVS\\Database\\domotica.db");
        }
        */

    }
}
