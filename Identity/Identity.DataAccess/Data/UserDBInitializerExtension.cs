using Identity.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataAccess.Data
{
    internal static class UserDBInitializerExtension
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<User>().HasData(
                new User
                {
                    Id = "af3c8548-ac9e-43bf-8474-ae514d9780d6",
                    FirstName = "Dmitry",
                    LastName = "Ivanov",
                    UserName = "dmitry.ivanov",
                    Email = "dmitry.ivanov@gmail.com",
                    NormalizedEmail = "DMITRY.IVANOV@GMAIL.COM",
                    PasswordHash = "AQAAAAIAAYagAAAAECu5BUlDI0sDkxmupwojpUNvz7hUGDPoO0pUl2XMg/aI6SBzW79FNA/sT8uaZtukug==",
                    CreationDate = DateTime.UtcNow,
                    Region = "Italy"
                });

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "f128150c-afbe-472f-af50-0df6d2cb86a1", Name = "admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "f95d795a-0d1a-4c38-8323-6f6140f4b54d", Name = "creator", NormalizedName = "CREATOR" }, 
                new IdentityRole { Id = "f95d795a-0d1s-4728-8323-ada65d4sa4da", Name = "listener", NormalizedName = "LISTENER"});

            builder.Entity<IdentityUserRole<string>>().HasData( 
                new IdentityUserRole<string> 
                { 
                    RoleId = "f128150c-afbe-472f-af50-0df6d2cb86a1",
                    UserId = "af3c8548-ac9e-43bf-8474-ae514d9780d6"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "f95d795a-0d1s-4728-8323-ada65d4sa4da",
                    UserId = "af3c8548-ac9e-43bf-8474-ae514d9780d6"
                });
        }
    }
}
