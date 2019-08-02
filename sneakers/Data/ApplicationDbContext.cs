using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sneakers.Models;

namespace sneakers.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Sneaker> Sneaker { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Condition> Condition { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Review> Review { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>()
                .Property(b => b.Date)
                .HasDefaultValueSql("GETDATE()");

            // Restrict deletion of related sneaker when Message entry is removed
            modelBuilder.Entity<Sneaker>()
                .HasMany(o => o.Messages)
                .WithOne(l => l.Sneaker)
                .OnDelete(DeleteBehavior.Restrict);


            ApplicationUser user1 = new ApplicationUser
            {
                FirstName = "Jameka",
                LastName = "Echols",
                UserName = "jameka.echols@gmail.com",
                NormalizedUserName = "JAMEKA.ECHOLS@GMAIL.COM",
                Email = "jameka.echols@gmail.com",
                NormalizedEmail = "JAMEKA.ECHOLS@GMAIL.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = "7f434309-a4d9-48e9-9ebb-8803db794579",
                Id = "00000001-ffff-ffff-ffff-ffffffffffff"
            };

            var passwordHash = new PasswordHasher<ApplicationUser>();
            user1.PasswordHash = passwordHash.HashPassword(user1, "Admin8*");
            modelBuilder.Entity<ApplicationUser>().HasData(user1);

            // brand
            modelBuilder.Entity<Brand>().HasData(
                new Brand
                {
                    BrandId = 1,
                    BrandType = "Nike"
                },
                new Brand
                {
                    BrandId = 2,
                    BrandType = "Air Jordan"
                },
                new Brand
                {
                    BrandId = 3,
                    BrandType = "Vans"
                },
                new Brand
                {
                    BrandId = 4,
                    BrandType = "Adidas"
                },
                new Brand
                {
                    BrandId = 5,
                    BrandType = "New Balance"
                },
                new Brand
                {
                    BrandId = 6,
                    BrandType = "Asics"
                },
                new Brand
                {
                    BrandId = 7,
                    BrandType = "Reebok"
                },
                new Brand
                {
                    BrandId = 8,
                    BrandType = "Puma"
                },
                new Brand
                {
                    BrandId = 9,
                    BrandType = "Fila"
                },
                new Brand
                {
                    BrandId = 10,
                    BrandType = "Converse"
                },
                new Brand
                {
                    BrandId = 11,
                    BrandType = "Saucony"
                }
            );

            // size
            modelBuilder.Entity<Size>().HasData(
                new Size
                {
                    SizeId = 1,
                    ShoeSize = "15M"
                },
                new Size
                {
                    SizeId = 2,
                    ShoeSize = "14.5M"
                },
                new Size
                {
                    SizeId = 3,
                    ShoeSize = "14M"
                },
                new Size
                {
                    SizeId = 4,
                    ShoeSize = "13.5M"
                },
                new Size
                {
                    SizeId = 5,
                    ShoeSize = "13M"
                },
                new Size
                {
                    SizeId = 6,
                    ShoeSize = "12.5M"
                },
                new Size
                {
                    SizeId = 7,
                    ShoeSize = "12M"
                },
                new Size
                {
                    SizeId = 8,
                    ShoeSize = "11.5M"
                },
                new Size
                {
                    SizeId = 9,
                    ShoeSize = "11M"
                },
                new Size
                {
                    SizeId = 10,
                    ShoeSize = "10.5M"
                },
                new Size
                {
                    SizeId = 11,
                    ShoeSize = "10M"
                },
                new Size
                {
                    SizeId = 12,
                    ShoeSize = "9.5M"
                },
                new Size
                {
                    SizeId = 13,
                    ShoeSize = "9M"
                },
                new Size
                {
                    SizeId = 14,
                    ShoeSize = "8.5M"
                },
                new Size
                {
                    SizeId = 15,
                    ShoeSize = "8M"
                },
                new Size
                {
                    SizeId = 16,
                    ShoeSize = "7.5M"
                },
                new Size
                {
                    SizeId = 17,
                    ShoeSize = "7M"
                },
                new Size
                {
                    SizeId = 18,
                    ShoeSize = "6.5M"
                },
                new Size
                {
                    SizeId = 19,
                    ShoeSize = "6M"
                },
                new Size
                {
                    SizeId = 20,
                    ShoeSize = "6W"
                },
                new Size
                {
                    SizeId = 21,
                    ShoeSize = "6.5W"
                },
                new Size
                {
                    SizeId = 22,
                    ShoeSize = "7W"
                },
                new Size
                {
                    SizeId = 23,
                    ShoeSize = "7.5W"
                },
                new Size
                {
                    SizeId = 24,
                    ShoeSize = "8W"
                },
                new Size
                {
                    SizeId = 25,
                    ShoeSize = "8.5W"
                },
                new Size
                {
                    SizeId = 26,
                    ShoeSize = "9W"
                },
                new Size
                {
                    SizeId = 27,
                    ShoeSize = "9.5W"
                },
                new Size
                {
                    SizeId = 28,
                    ShoeSize = "10W"
                },
                new Size
                {
                    SizeId = 29,
                    ShoeSize = "10.5W"
                },
                new Size
                {
                    SizeId = 30,
                    ShoeSize = "11W"
                }
            );

            modelBuilder.Entity<Condition>().HasData(
                new Condition
                {
                    ConditionId = 1,
                    ConditionType = "Never Worn"
                },
                new Condition
                {
                    ConditionId = 2,
                    ConditionType = "Barely Worn"
                },
                new Condition
                {
                    ConditionId = 3,
                    ConditionType = "Fair"
                },
                new Condition
                {
                    ConditionId = 4,
                    ConditionType = "Slightly Damaged"
                }
            );

            modelBuilder.Entity<Sneaker>().HasData(
                new Sneaker
                {
                    SneakerId = 1,
                    UserId = user1.Id,
                    SizeId = 19,
                    Title = "Air Jordan Retro 1 Shadow",
                    Description = "barely used retro 1s, shadow colorway",
                    BrandId = 2,
                    IsSold = false,
                    ConditionId = 2,
                    Price = 215.00
                }
            );
        }
    }
}
