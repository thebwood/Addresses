using Addresses.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Addresses.DatabaseLayer.Data
{
    public class AddressDbContext : DbContext
    {
        public AddressDbContext(DbContextOptions<AddressDbContext> options)
        : base(options)
        {
        }
        public DbSet<AddressModel> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressModel>().ToTable("Addresses");

        }



    }
}
