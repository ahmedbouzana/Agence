using Agence.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agence
{
    class Context : DbContext
    {
        public DbSet<Registre> Registres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlite("Data Source=Data.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Registre>()
                .HasIndex(b => b.NAffaire)
                .IsUnique();
        }
    }
}