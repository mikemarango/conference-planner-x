using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(
            DbContextOptions<ApplicationContext> options) 
            : base(options)
        { }

        public DbSet<Speaker> Speakers { get; set; }
    }
}
