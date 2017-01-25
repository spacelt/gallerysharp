using GallerySharp.Models;
using Identity.PasswordHasher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GallerySharp.Data
{
    public static class ApplicationDbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            var hasher = new PasswordHasher();
            context.Users.Add(
                new ApplicationUser
                {
                    UserName = "administrator@administrator.admin",
                    PasswordHash = hasher.HashPassword("Admin1234?"),
                    Email = "administrator@administrator.admin",
                    Id = Guid.NewGuid().ToString(),
                    NormalizedEmail = "ADMINISTRATOR@ADMINISTRATOR.ADMIN",
                    NormalizedUserName = "ADMINISTRATOR@ADMINISTRATOR.ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = false,
                    LockoutEnabled = true,
                    PhoneNumberConfirmed = false,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    TwoFactorEnabled = false

                });
            context.SaveChanges();
        }
    }
}
