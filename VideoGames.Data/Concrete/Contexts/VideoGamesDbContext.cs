using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Entity.Concrete;
using VideoGames.Shared.ComplexTypes;

namespace VideoGames.Data.Concrete.Contexts
{
    public class VideoGamesDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public VideoGamesDbContext(DbContextOptions<VideoGamesDbContext> options) : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<VideoGame> VideoGames { get; set; }
        public DbSet<VideoGameCDkey> VideoGameCDkeys { get; set; }
        public DbSet<OrderItemCDKey> OrderItemCDKeys { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.ID);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Description).HasMaxLength(500);
            });

            builder.Entity<VideoGame>(entity =>
            {
                entity.HasKey(v => v.ID);
                entity.Property(v => v.Name).IsRequired().HasMaxLength(400);
                entity.Property(v => v.Description).HasMaxLength(500);
                entity.Property(v => v.Price).IsRequired().HasColumnType("decimal(18,2)");
            });

            builder.Entity<VideoGameCategory>(entity =>
            {
                entity.HasKey(vc => new { vc.VideoGameId, vc.CategoryId });
                entity.HasOne(vc => vc.VideoGame)
                      .WithMany(v => v.VideoGameCategories)
                      .HasForeignKey(vc => vc.VideoGameId);
                entity.HasOne(vc => vc.Category)
                      .WithMany(c => c.VideoGameCategories)
                      .HasForeignKey(vc => vc.CategoryId);

            });


            builder.Entity<VideoGameCDkey>(entity =>
            {
                entity.HasKey(k => k.ID); // Primary key


                entity.HasOne(k => k.VideoGame)
                      .WithMany(v => v.CDkeys)
                      .HasForeignKey(k => k.VideoGameId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(k => k.CDkey)
                                     .IsRequired()
                                     .HasMaxLength(100); // Key karakter sınırı (örnek)

            });

            builder.Entity<OrderItem>(entity =>
            {

                entity.HasOne(oi => oi.VideoGameCDkey)
                      .WithOne()
                      .HasForeignKey<OrderItem>(oi => oi.VideoGameCDkeyId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(oi => oi.VideoGame)
                      .WithMany()
                      .HasForeignKey(oi => oi.VideoGameId)
                      .OnDelete(DeleteBehavior.NoAction);

 
                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(oi => oi.UnitPrice)
                      .HasColumnType("decimal(10,2)");
            });

            builder.Entity<OrderItemCDKey>(entity =>
            {
                entity.HasKey(x => x.ID);

                
                entity.HasOne(x => x.OrderItem)
                      .WithMany((x => x.OrderItemCDKeys)) 
                      .HasForeignKey(x => x.OrderItemId) 
                      .OnDelete(DeleteBehavior.Cascade); 

              
                entity.HasOne(x => x.VideoGameCDkey)
                      .WithMany()
                      .HasForeignKey(x => x.VideoGameCDkeyId)
                      .OnDelete(DeleteBehavior.Restrict); 
            });

            // Default roles
            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = "115c7796-cfac-44de-91b5-916eaae125b5", Name = "AdminUser", NormalizedName = "ADMINUSER", Description = "Administrator role" },
                new ApplicationRole { Id = "811f466c-9d06-43f8-a054-24aedbb4161b", Name = "NormalUser", NormalizedName = "NORMALUSER", Description = "Regular user role" }
            );

            // Default users
            var hasher = new PasswordHasher<ApplicationUser>();
            var adminUser = new ApplicationUser
            {
                Id = "c0b7fef7-df2b-4857-9b3d-bc8967ad19ac",
                UserName = "adminuser@gmail.com",
                NormalizedUserName = "ADMINUSER@GMAIL.COM",
                Email = "adminuser@gmail.com",
                NormalizedEmail = "ADMINUSER@GMAIL.COM",
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "User",
                Address = "",
                PhoneNumber = "",
                City = "",
                DateOfBirth = DateTime.Now,
                Gender = GenderType.None,
                PasswordHash = hasher.HashPassword(null, "Qwe123.,")
            };
            var normalUser = new ApplicationUser
            {
                Id = "14a0183f-1e96-4930-a83d-6ef5f22d8c09",
                UserName = "normaluser@gmail.com",
                NormalizedUserName = "NORMALUSER@GMAIL.COM",
                Email = "normaluser@gmail.com",
                NormalizedEmail = "NORMALUSER@GMAIL.COM",
                EmailConfirmed = true,
                FirstName = "Normal",
                LastName = "User",
                Address = "",
                PhoneNumber = "",
                City = "",
                DateOfBirth = DateTime.Now,
                Gender = GenderType.None,
                PasswordHash = hasher.HashPassword(null, "Qwe123.,")
            };
            builder.Entity<ApplicationUser>().HasData(adminUser, normalUser);

            // Assign roles to users
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = adminUser.Id, RoleId = "115c7796-cfac-44de-91b5-916eaae125b5" },
                new IdentityUserRole<string> { UserId = normalUser.Id, RoleId = "811f466c-9d06-43f8-a054-24aedbb4161b" }
            );

            builder.Entity<Cart>().HasData(
                new Cart { ID = 1, ApplicationUserId = adminUser.Id },
                new Cart { ID = 2, ApplicationUserId = normalUser.Id }
            );
        }

    }
}
