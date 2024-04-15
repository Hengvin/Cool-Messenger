using System;
using SPG.Messenger.Domain.Model.MessengerDomain;
using SPG.Messenger.Domain.Model.UserDomain;
using SPG.Messenger.Repository.Test.Helpers;
using Xunit;

namespace SPG.Messenger.Repository.Test
{
	public class MessengerRepositoryTest
	{


        // GET ALL

        [Fact]
        public void MessengerRepository_whenGettingAllEntries_shouldReturnAllEntries()
        {
            using (var db = DatabaseUtilities.CreateDb())
            {
                // Arrange
                var seedUsers = DatabaseUtilities.GetSeedUsers();
                db.Users.AddRange(seedUsers);
                db.SaveChanges();

                var seedMessengers = DatabaseUtilities.GetSeedMessengers(db);
                db.MessengerEntries.AddRange(seedMessengers);
                db.SaveChanges();

                var repository = new MessengerRepository(db);

                // Act
                var actualMessengers = repository.GetAll().ToList();

                // Assert
                Assert.Equal(seedMessengers.Count, actualMessengers.Count);
            }
        }



        // GET SINGLE

        [Fact]
        public void MessengerRepository_whenGettingSingleEntryById_shouldReturnCorrectEntry()
        {
            using (var db = DatabaseUtilities.CreateDb())
            {
                // Arrange
                var seedUsers = DatabaseUtilities.GetSeedUsers();
                db.Users.AddRange(seedUsers);
                db.SaveChanges();

                var seedMessengers = DatabaseUtilities.GetSeedMessengers(db);
                db.MessengerEntries.AddRange(seedMessengers);
                db.SaveChanges();

                var repository = new MessengerRepository(db);
                var expectedEntry = seedMessengers.First();

                // Act
                var actualEntry = repository.GetSingle(expectedEntry.Id);

                // Assert
                Assert.NotNull(actualEntry);
                Assert.Equal(expectedEntry.Title, actualEntry.Title);
            }
        }



        // CREATE

        [Fact]
        public void MessengerRepository_whenCreatingEntry_shouldAddEntry()
        {
            using (var db = DatabaseUtilities.CreateDb())
            {
                // Arrange
                var seedUsers = DatabaseUtilities.GetSeedUsers();
                db.Users.AddRange(seedUsers);
                db.SaveChanges();

                var repository = new MessengerRepository(db);
                var newEntry = new MessengerEntry(seedUsers[0], new HashSet<User>(), "New Discussion", "A place to discuss new ideas.");

                // Act
                repository.Create(newEntry);

                // Assert
                var retrievedEntry = db.MessengerEntries.FirstOrDefault(m => m.Title == newEntry.Title);
                Assert.NotNull(retrievedEntry);
                Assert.Equal("New Discussion", retrievedEntry.Title);
            }
        }



        // FILTER

        [Fact]
        public void MessengerRepository_whenFilteringByTitleContains_shouldReturnFilteredEntries()
        {
            using (var db = DatabaseUtilities.CreateDb())
            {
                // Arrange
                var seedUsers = DatabaseUtilities.GetSeedUsers();
                db.Users.AddRange(seedUsers);
                db.SaveChanges();

                var seedMessengers = DatabaseUtilities.GetSeedMessengers(db);
                db.MessengerEntries.AddRange(seedMessengers);
                db.SaveChanges();

                var repository = new MessengerRepository(db);
                var expectedTitleContains = "Discussion";

                // Act
                var filteredEntries = repository.FilterBuilder.ByTitleContains(expectedTitleContains).Build().ToList();

                // Assert
                Assert.NotNull(filteredEntries);
                Assert.All(filteredEntries, entry => Assert.Contains(expectedTitleContains.ToLower(), entry.Title.ToLower()));
            }
        }


    }
}

