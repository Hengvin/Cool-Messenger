using System;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using SPG.Messenger.Application.Mock;
using SPG.Messenger.Application.Services;
using SPG.Messenger.Application.Test.Helpers;
using SPG.Messenger.Domain.Dtos;
using SPG.Messenger.Domain.Exceptions;
using SPG.Messenger.Domain.Model.UserDomain;
using SPG.Messenger.Infrastructure;
using SPG.Messenger.Repository;
using Xunit;

namespace SPG.Messenger.Application.Test;

public class UserServiceTest
{
    public static MessengerContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<MessengerContext>()
              .UseSqlite("DataSource=:memory:")
              .Options;

        var context = new MessengerContext(options);
        context.Database.OpenConnection();
        return context;
    }


    private UserService CreateUserService(MessengerContext db)
    {
        return new UserService(
            null,
            new UserRepository(db),
            new UserRepository(db),
            new MessengerRepository(db),
            new GuidServiceMock()
        );
    }



    // GET USER

    [Fact]
    public void GetSingle_ShouldReturnUser_WhenUserExists()
    {
        using (MessengerContext db = DatabaseUtilities.CreateDb())
        {
            // Arrange
            db.Users.AddRange(DatabaseUtilities.GetSeedUsers());
            db.SaveChanges();

            var userService = CreateUserService(db);
            var existingId = db.Users.First().Id;

            // Act
            var userDto = userService.GetSingle(existingId);

            // Assert
            Assert.NotNull(userDto);
            Assert.Equal("alice.wonderland@example.com", userDto.Email);
        }
    }



    [Fact]
    public void GetSingle_ShouldThrowUserServiceReadException_WhenUserDoesNotExist()
    {
        using (var db = DatabaseUtilities.CreateDb())
        {
            // Arrange
            db.Users.AddRange(DatabaseUtilities.GetSeedUsers());
            db.SaveChanges();

            var userService = CreateUserService(db);
            var nonExistingId = 999;

            // Act & Assert
            var exception = Assert.Throws<UserServiceReadException>(() => userService.GetSingle(nonExistingId));
            Assert.Equal("Failed to retrieve user with ID: 999", exception.Message);
        }
    }



    
    // FILTER USERS BY EMAIL

    [Fact]
    public void UserService_whenFilteringByEmail_shouldReturnFilteredUsers()
    {
        using (var db = DatabaseUtilities.CreateDb())
        {
            // Arrange
            var seedUsers = DatabaseUtilities.GetSeedUsers();
            db.Users.AddRange(seedUsers);
            db.SaveChanges();

            var userService = CreateUserService(db);

            var expectedEmailContains = "example.com";
            var expectedCount = seedUsers.Count(user => user.Email.Contains(expectedEmailContains));

            // Act
            var filteredUsers = userService.FilterByEmail(expectedEmailContains);

            // Assert
            Assert.NotNull(filteredUsers);
            Assert.All(filteredUsers, dto => Assert.Contains(expectedEmailContains, dto.Email));
            Assert.Equal(expectedCount, filteredUsers.Count);
        }
    }




    // REGISTER USER


    [Fact]
    public void Register_ShouldCreateUser_WhenEmailIsNotInUse()
    {
        using (var db = DatabaseUtilities.CreateDb())
        {
            // Arrange
            db.Users.AddRange(DatabaseUtilities.GetSeedUsers());
            db.SaveChanges();

            var userService = CreateUserService(db);

            var command = new RegisterUserCommand(
                Email: "new.user@example.com",
                Password: "spengergasse",
                FirstName: "New",
                LastName: "User",
                ParticipantIds: new List<int>(),
                MessengerTitle: null,             
                MessengerDescription: null
            );

            // Act
            var userDto = userService.Register(command);

            // Assert
            Assert.NotNull(userDto);
            Assert.Equal("new.user@example.com", userDto.Email);
            Assert.Equal("New", userDto.FirstName);
            Assert.Equal("User", userDto.LastName);
        }
    }



    [Fact]
    public void Register_ShouldFail_WhenEmailAlreadyExists()
    {
        using (var db = DatabaseUtilities.CreateDb())
        {
            // Arrange
            var existingUserEmail = "existing.user@example.com";
            db.Users.Add(new User(existingUserEmail, "spengergasse", new Profile("Existing", "User")));
            db.SaveChanges();

            var userService = CreateUserService(db);

            var command = new RegisterUserCommand(
                Email: "existing.user@example.com",
                Password: "spengergasse",
                FirstName: "New",
                LastName: "User",
                ParticipantIds: new List<int>(),
                MessengerTitle: null,
                MessengerDescription: null
            );

            // Act & Assert
            var exception = Assert.Throws<UserServiceValidationException>(() => userService.Register(command));
            Assert.Equal($"User registration failed: Email {existingUserEmail} already in use.", exception.Message);
        }
    }




    // REGISTER A USER WITH PARTICIPANTS

    [Fact]
    public void Register_ShouldCreateUserAndMessenger_WhenParticipantsAreSpecified()
    {
        using (var db = DatabaseUtilities.CreateDb())
        {
            // Arrange: Setup DB
            db.Users.AddRange(DatabaseUtilities.GetSeedUsers());
            db.SaveChanges();

            // Arrange: Setup Service
            var userService = CreateUserService(db);

            // Arrange: All seeded users as participants
            var seedUserIds = db.Users.Select(u => u.Id).ToList();
            var command = new RegisterUserCommand(
                Email: "new.user@example.com",
                Password: "spengergasse",
                FirstName: "New",
                LastName: "User",
                ParticipantIds: seedUserIds,
                MessengerTitle: "New Project Discussion",
                MessengerDescription: "Discuss all new project related queries here."
            );

            // Act: Register user with participants
            var userDto = userService.Register(command);

            // Assert: Check user registration
            Assert.NotNull(userDto);
            Assert.Equal("new.user@example.com", userDto.Email);
            Assert.Equal("New", userDto.FirstName);
            Assert.Equal("User", userDto.LastName);

            // Assert: Check messenger creation
            var messengerEntry = db.MessengerEntries.FirstOrDefault(m => m.UserNavigation.Email == "new.user@example.com");
            Assert.NotNull(messengerEntry);
        }
    }




    // UPDATE USER


    [Fact]
    public void Update_ShouldUpdateUserDetails_WhenUserExists()
    {
        using (var db = DatabaseUtilities.CreateDb())
        {
            // Arrange
            db.Users.AddRange(DatabaseUtilities.GetSeedUsers());
            db.SaveChanges();

            var userService = CreateUserService(db);
            var existingUser = db.Users.First();
            var updateUserCommand = new UpdateUserCommand(existingUser.Id, "UpdatedFirst", "UpdatedLast");

            // Act
            var userDto = userService.Update(updateUserCommand);

            // Assert
            Assert.NotNull(userDto);
            Assert.Equal("UpdatedFirst", userDto.FirstName);
            Assert.Equal("UpdatedLast", userDto.LastName);
        }
    }


    [Fact]
    public void Update_ShouldThrowUserServiceUpdateException_WhenUserDoesNotExist()
    {
        using (var db = DatabaseUtilities.CreateDb())
        {
            // Arrange
            var userService = CreateUserService(db);
            var updateUserCommand = new UpdateUserCommand(999, "NonExistentFirst", "NonExistentLast");

            // Act & Assert
            var exception = Assert.Throws<UserServiceUpdateException>(() => userService.Update(updateUserCommand));
            Assert.Equal("Failed to retrieve user for update with ID: 999", exception.Message);
        }
    }



    // DELETE USER

    [Fact]
    public void Delete_ShouldRemoveUser_WhenUserExists()
    {
        using (var db = DatabaseUtilities.CreateDb())
        {
            // Arrange
            db.Users.AddRange(DatabaseUtilities.GetSeedUsers());
            db.SaveChanges();

            var userService = CreateUserService(db);
            var existingUser = db.Users.First();
            var existingUserId = existingUser.Id;

            // Act
            userService.Delete(existingUserId);

            // Assert
            var deletedUser = db.Users.Find(existingUserId);
            Assert.Null(deletedUser);
        }
    }


    [Fact]
    public void Delete_ShouldThrowUserServiceDeleteException_WhenUserDoesNotExist()
    {
        using (var db = DatabaseUtilities.CreateDb())
        {
            // Arrange
            var userService = CreateUserService(db);
            var nonExistingUserId = 999;

            // Act & Assert
            var exception = Assert.Throws<UserServiceDeleteException>(() => userService.Delete(nonExistingUserId));
            Assert.Equal("Failed to delete user with ID: 999.", exception.Message);
        }
    }
}
