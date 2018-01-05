using CoreWeb.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CoreWeb.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        // Stored Procedures or tables
        public DbSet<spGetOneExample> spGetOneExample { get; set; }
        public DbSet<spGetManyExamples> spGetManyExamples { get; set; }
    }
}
