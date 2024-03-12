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
                    NormalizedUserName = "DMITRY.IVANOV",
                    Email = "dmitry.ivanov@gmail.com",
                    NormalizedEmail = "DMITRY.IVANOV@GMAIL.COM",
                    PasswordHash = "AQAAAAIAAYagAAAAEGnb9gCu0ZszBQAsOO8EfkSggUCxR0fk9mQJdz5Imzxtn6HipugF+ELehdTdFZ3wHg==",
                    SecurityStamp = "LHOBZXJKHQASYZODSS7FPA2UQOKTAHDR",
                    ConcurrencyStamp = "7af8d93c-9b9b-4e09-bc1d-9ef180915552",
                    CreationDate = DateTime.UtcNow,
                    Region = "Italy"
                },
                new User
                {
                    Id = "c13d3c43-7724-4cbd-9226-56fb2fb7e482",
                    FirstName = "Chester",
                    LastName = "Bennington",
                    UserName = "Chester.Bennington",
                    NormalizedUserName = "CHESTER.BENNINGTON",
                    Email = "bennington@outlook.com",
                    NormalizedEmail = "BENNINGTON@OUTLOOK.COM",
                    PasswordHash = "AQAAAAIAAYagAAAAECsYLJkr31tTmZasfE+51TiiS2V1NwwmRmFgCtr/p7K0aK6jTznzliXwQVzet9q4PA==",
                    SecurityStamp = "ZNUHMAR7OTK2XF4DUDTZUOJVRLP5CKSZ",
                    ConcurrencyStamp = "48fa758e-e07d-4abf-83ca-797db1fe3311",
                    CreationDate = DateTime.UtcNow,
                    Region = "Los Angeles"
                },
                new User
                {
                    Id = "98975656-f5e8-42d9-9f6b-58598773e16c",
                    FirstName = "Mike",
                    LastName = "Shinoda",
                    UserName = "MikeShinoda1977",
                    NormalizedUserName = "MIKESHINODA1977",
                    Email = "MikeShinoda@gmail.com",
                    NormalizedEmail = "MIKESHINODA@GMAIL.COM",
                    PasswordHash = "AQAAAAIAAYagAAAAEMleAqjG11pRU4FUv3ReSBj+3QvI5irY8WpzypbgyOD9qhLCgI8IRFiWbB7Gq7O6dw==",
                    SecurityStamp = "BGA2JFI5KXTO6565STR4P34F3QPQR2GG",
                    ConcurrencyStamp = "f1c51ace-2e20-4ac7-8b56-8799f4c794bb",
                    CreationDate = DateTime.UtcNow,
                    Region = "Los Angeles"
                },
                new User
                {
                    Id = "6930fd61-ac4f-4fc1-ab3c-8cc9db42aa90",
                    FirstName = "Martin",
                    LastName = "Garritsen",
                    UserName = "Martijn_Garritsen",
                    NormalizedUserName = "MARTIJN_GARRITSEN",
                    Email = "garritsen@gmail.com",
                    NormalizedEmail = "GARRITSEN@GMAIL.COM",
                    PasswordHash = "AQAAAAIAAYagAAAAEIdo5ONoHW/GVFfPqonfYe1pGZPGPNA5LZJkbJCs2pw59yHOx8PVxj62thP8RrEH0A==",
                    SecurityStamp = "CFSP6QTASV5VVOH2ROTWN7SGSEK3RHFW",
                    ConcurrencyStamp = "6b975ebc-7954-4be3-91ec-5854ba11b035",
                    CreationDate = DateTime.UtcNow,
                    Region = "Amsterdam"
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
                    RoleId = "f95d795a-0d1a-4c38-8323-6f6140f4b54d",
                    UserId = "af3c8548-ac9e-43bf-8474-ae514d9780d6"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "f95d795a-0d1s-4728-8323-ada65d4sa4da",
                    UserId = "af3c8548-ac9e-43bf-8474-ae514d9780d6"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "f95d795a-0d1a-4c38-8323-6f6140f4b54d",
                    UserId = "c13d3c43-7724-4cbd-9226-56fb2fb7e482"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "f95d795a-0d1a-4c38-8323-6f6140f4b54d",
                    UserId = "98975656-f5e8-42d9-9f6b-58598773e16c"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "f95d795a-0d1a-4c38-8323-6f6140f4b54d",
                    UserId = "6930fd61-ac4f-4fc1-ab3c-8cc9db42aa90"
                });
        }
    }
}
