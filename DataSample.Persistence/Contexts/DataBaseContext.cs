using DataSample.Application.Interfaces.Contexts;
using DataSample.Common.Roles;
using DataSample.Domain.Entities.Fainances;
using DataSample.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSample.Persistence.Contexts
{
    public class DataBaseContext : DbContext, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInRole> UserInRoles { get; set; }
        public DbSet<Cheque> Cheques { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cheque>()
                .HasOne(p => p.User)
                .WithMany(p => p.Cheques)
                .OnDelete(DeleteBehavior.NoAction);

            //Seed Data
            SeedData(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = nameof(UserRoles.Admin) });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 2, Name = nameof(UserRoles.Operator) });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 3, Name = nameof(UserRoles.Customer) });
        }
    }
}
