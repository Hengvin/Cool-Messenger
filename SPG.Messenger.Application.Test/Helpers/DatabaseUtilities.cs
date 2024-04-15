using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SPG.Messenger.Domain.Model.MessengerDomain;
using SPG.Messenger.Domain.Model.UserDomain;
using SPG.Messenger.Infrastructure;

namespace SPG.Messenger.Application.Test.Helpers
{
    public static class DatabaseUtilities
    {
        public static MessengerContext CreateDb()
        {
            SqliteConnection connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            DbContextOptions options = new DbContextOptionsBuilder()
                .UseSqlite(connection)
                .Options;

            MessengerContext db = new MessengerContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }


        public static List<User> GetSeedUsers()
        {
            return new List<User>
            {
                new User("alice.wonderland@example.com", "spengergasse", new Profile("Alice", "Wonderland")),
                new User("bob.builder@example.com", "spengergasse", new Profile("Bob", "Builder")),
                new User("charlie.chaplin@example.com", "spengergasse", new Profile("Charlie", "Chaplin")),
                new User("diana.prince@example.com", "spengergasse", new Profile("Diana", "Prince"))
            };
        }



        // A messenger needs a user

        public static List<MessengerEntry> GetSeedMessengers(MessengerContext db)
        {
            var users = db.Users.ToList();

            return new List<MessengerEntry>
            {
                new MessengerEntry(
                    creator: users[0],
                    participantIds: new HashSet<User> { users[1], users[2] },
                    title: "Project Discussion",
                    description: "Discuss all the project related queries here."
                ),
                new MessengerEntry(
                    creator: users[1],
                    participantIds: new HashSet<User> { users[0], users[3] },
                    title: "Fun Zone",
                    description: "Place to share all the fun stuff."
                )
            };
        }
    }
}

