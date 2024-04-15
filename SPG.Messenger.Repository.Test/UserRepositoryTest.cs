using System;
using SPG.Messenger.Domain.Model.UserDomain;
using SPG.Messenger.Infrastructure;
using SPG.Messenger.Repository.Test.Helpers;
using Xunit;

namespace SPG.Messenger.Repository.Test
{
	public class UserRepositoryTest
	{

        // GET ALL

        [Fact]
        public void UserRepository_whenGettingAllUsers_shouldReturnAllUsers()
        {
            using (var db = DatabaseUtilities.CreateDb())
            {
                // Arrange
                var expectedUsers = DatabaseUtilities.GetSeedUsers();
                db.Users.AddRange(expectedUsers);
                db.SaveChanges();

                var repository = new UserRepository(db);

                // Act
                var actualUsers = repository.GetAll().ToList();

                // Assert
                Assert.Equal(expectedUsers.Count, actualUsers.Count);
            }
        }


        // GET SINGLE

        [Fact]
        public void UserRepository_whenGettingSingleUserById_shouldReturnCorrectUser()
        {
            using (var db = DatabaseUtilities.CreateDb())
            {
                // Arrange
                var seedUsers = DatabaseUtilities.GetSeedUsers();
                db.Users.AddRange(seedUsers);
                db.SaveChanges();

                var repository = new UserRepository(db);
                var expectedUser = seedUsers.First();

                // Act
                var actualUser = repository.GetSingle(expectedUser.Id);

                // Assert
                Assert.NotNull(actualUser);
                Assert.Equal(expectedUser.Email, actualUser.Email);
            }
        }



        // CREATE

        [Fact]
        public void UserRepository_whenCreatingUser_shouldAddUser()
        {
            using (var db = DatabaseUtilities.CreateDb())
            {
                // Arrange
                var repository = new UserRepository(db);
                var newUser = new User("new.user@example.com", "spengergasse", new Profile("New", "User"));

                // Act
                repository.Create(newUser);

                // Assert
                var retrievedUser = db.Users.FirstOrDefault(u => u.Email == newUser.Email);
                Assert.NotNull(retrievedUser);
                Assert.Equal("New", retrievedUser.Profile.FirstName);
            }
        }



        // DELETE

        [Fact]
        public void UserRepository_whenDeletingUser_shouldRemoveUser()
        {
            using (var db = DatabaseUtilities.CreateDb())
            {
                // Arrange
                var seedUsers = DatabaseUtilities.GetSeedUsers();
                db.Users.AddRange(seedUsers);
                db.SaveChanges();

                var repository = new UserRepository(db);
                var userToDelete = seedUsers.First();

                // Act
                repository.Delete(userToDelete.Id);

                // Assert
                var retrievedUser = db.Users.Find(userToDelete.Id);
                Assert.Null(retrievedUser);
            }
        }



        // UPDATE

        [Fact]
        public void UserRepository_WhenUpdatingUser_ShouldCorrectlyUpdate()
        {
            using (var db = DatabaseUtilities.CreateDb())
            {
                // Arrange
                var seedUsers = DatabaseUtilities.GetSeedUsers();
                db.Users.AddRange(seedUsers);
                db.SaveChanges();

                var repository = new UserRepository(db);

                var userToUpdate = db.Users.First();
                var expectedFirstName = "UpdatedFirst";
                var expectedLastName = "UpdatedLast";

                // Act
                var updateBuilder = repository
                    .UpdateBuilder(userToUpdate)
                    .WithFirstName(expectedFirstName)
                    .WithLastName(expectedLastName)
                    .Save();

                // Assert
                var updatedUser = db.Users.Find(userToUpdate.Id);
                Assert.NotNull(updatedUser);
                Assert.Equal(expectedFirstName, updatedUser.Profile.FirstName);
                Assert.Equal(expectedLastName, updatedUser.Profile.LastName);
            }
        }



        // FILTER BY EMAIL

        [Fact]
        public void UserRepository_whenFilteringByDomain_shouldReturnCorrectUsers()
        {
            using (var db = DatabaseUtilities.CreateDb())
            {
                // Arrange
                var seedUsers = DatabaseUtilities.GetSeedUsers();
                db.Users.AddRange(seedUsers);
                
                var newUser = new User("new.user@xxx.at", "spengergasse", new Profile("New", "User"));
                db.Users.Add(newUser);

                db.SaveChanges();

                var repository = new UserRepository(db);
                var expectedDomain = "xxx.at";

                // Act
                var filteredUsers = repository.FilterBuilder.ByEmailContains(expectedDomain).Build().ToList();

                // Assert
                Assert.NotNull(filteredUsers);
                Assert.Single(filteredUsers);
                Assert.All(filteredUsers, user => Assert.Contains(expectedDomain, user.Email));
            }
        }
    }
}

