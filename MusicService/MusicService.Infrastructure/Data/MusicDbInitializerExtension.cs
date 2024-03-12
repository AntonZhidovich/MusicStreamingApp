using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Entities;

namespace MusicService.Infrastructure.Data
{
    internal static class MusicDbInitializerExtension
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<User>().HasData(
                new User
                {
                    Id = "c13d3c43-7724-4cbd-9226-56fb2fb7e482",
                    UserName = "Chester.Bennington",
                    AuthorId = "e372c6da-4c4d-4cd5-a1ca-c8507cf2d326"
                },
                new User
                {
                    Id = "6930fd61-ac4f-4fc1-ab3c-8cc9db42aa90",
                    UserName = "Martijn_Garritsen",
                    AuthorId = "7bfe40e0-f945-487b-ae93-a07cfbdc87db"
                },
                new User
                {
                    Id = "98975656-f5e8-42d9-9f6b-58598773e16c",
                    UserName = "MikeShinoda1977",
                    AuthorId = "e372c6da-4c4d-4cd5-a1ca-c8507cf2d326"
                },
                new User
                {
                    Id = "af3c8548-ac9e-43bf-8474-ae514d9780d6",
                    UserName = "dmitry.ivanov",
                });

            builder.Entity<Author>().HasData(
                new Author
                {
                    Id = "e372c6da-4c4d-4cd5-a1ca-c8507cf2d326",
                    Name = "Linkin park",
                    CreatedAt = DateTime.Now,
                    IsBroken = false,
                    Description = "Linkin Park is an American rock band from Agoura Hills, California. " +
                    "The band's current lineup comprises vocalist/rhythm guitarist/keyboardist Mike Shinoda, " +
                    "lead guitarist Brad Delson, bassist Dave Farrell, DJ/turntables Joe Hahn and drummer Rob" +
                    " Bourdon, all of whom are founding members.",
                },
                new Author
                {
                    Id = "7bfe40e0-f945-487b-ae93-a07cfbdc87db",
                    Name = "Martin Garrix",
                    CreatedAt = DateTime.Now,
                    IsBroken = false,
                    Description = "Dutch electronic music producer whose multi-platinum dance anthems topped " +
                    "charts around the globe.",
                });
        }
    }
}
