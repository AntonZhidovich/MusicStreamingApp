using Microsoft.EntityFrameworkCore;
using MusicService.Domain.Constants;
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
                    Id = "70d71f5a-a4ef-488a-b4e9-eb86f82481a8",
                    UserName = "Chester.Bennington",
                    Roles = new List<string> { UserRoles.creator, UserRoles.listener, UserRoles.admin },
                    AuthorId = "e372c6da-4c4d-4cd5-a1ca-c8507cf2d326"
                },
                new User
                {
                    Id = "7b761e59-78f3-4862-b1ad-87065bc8f51b",
                    UserName = "Martijn-Garritsen",
                    Roles = new List<string> { UserRoles.creator, UserRoles.listener },
                    AuthorId = "7bfe40e0-f945-487b-ae93-a07cfbdc87db"
                },
                new User
                {
                    Id = "d480a3c1-99aa-4775-819b-94e9183d0e21",
                    UserName = "MikeShinoda1977",
                    Roles = new List<string> { UserRoles.creator, UserRoles.listener },
                    AuthorId = "e372c6da-4c4d-4cd5-a1ca-c8507cf2d326"
                },
                new User
                {
                    Id = "76ef2410-4e2e-4542-a20b-b0f19dfd5d76",
                    UserName = "dmitry.ivanov",
                    Roles = new List<string> { UserRoles.listener }
                },
                new User
                {
                    Id = "3e4cc735-f041-424d-9ada-f835a7c1978a",
                    UserName = "yegor.kozlov02",
                    Roles = new List<string> { UserRoles.listener, UserRoles.admin }
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
