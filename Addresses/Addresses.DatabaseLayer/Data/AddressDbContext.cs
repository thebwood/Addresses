using Addresses.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Addresses.DatabaseLayer.Data
{
    public class AddressDbContext : IdentityDbContext<UserModel, RoleModel, Guid>
    {
        public AddressDbContext(DbContextOptions<AddressDbContext> options) : base(options)
        {
        }

        public DbSet<AddressModel> Addresses { get; set; }
        public DbSet<TokenBlacklistModel> TokenBlacklist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AddressModel>().ToTable("Addresses");

            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<RoleModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            modelBuilder.Entity<UserRoleModel>(entity =>
            {
                entity.HasOne(e => e.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(e => e.UserId);
                entity.HasOne(e => e.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(e => e.RoleId);
            });

            modelBuilder.Entity<TokenBlacklistModel>(entity =>
            {
                entity.HasKey(e => e.Token);
                entity.Property(e => e.Token).HasMaxLength(450);
            });
        }
    }
}
