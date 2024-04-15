using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Xunit;

namespace SPG.Messenger.Infrastructure.Test
{
    public class MessengerContextTest
    {

        public MessengerContextTest()
        {
            Batteries.Init(); // Initialize SQLitePCL
        }


        private MessengerContext CreateDb()
        {
            // TODO in memory just for testing
            var options = new DbContextOptionsBuilder<MessengerContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var db = new MessengerContext(options);
            db.Database.OpenConnection();
            return db;
        }


        [Fact]
        public void EnsureDatabase_shouldBeCreated()
        {
            using var db = CreateDb();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}
