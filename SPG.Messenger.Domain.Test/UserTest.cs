using SPG.Messenger.Domain.Exceptions;
using SPG.Messenger.Domain.Model.UserDomain;
using Xunit;

namespace SPG.Messenger.Domain.Test
{
    public class UserTest
    {

        private readonly ModelFakers Faker = new();



        // ADD FRIEND ----------------------------------------------------------


        [Fact]
        public void AddFriend_shouldWork_WhenNoRelationExists()
        {
            // Given: Two unrelated fake users
            var self = Faker.User();
            var friend = Faker.User();

            // When: Adding one user as a friend to the other
            self.AddFriend(friend);

            // Then: Their social relations should reflect the friend request
            Assert.True(self.GetRelation(friend, self).Type == RelationType.OUTGOING);
            Assert.True(friend.GetRelation(self, friend).Type == RelationType.INCOMING);
        }


        [Fact]
        public void AddFriend_shouldFail_WhenRelationExists()
        {
            // Given: Two users, where one is already added as a friend to the other
            var self = Faker.User();
            var friend = Faker.User();
            self.AddFriend(friend);

            // When / Then: Adding the same friend again should throw an exception
            Assert.Throws<UserException>(() => self.AddFriend(friend));
        }




        // ACCEPT FRIEND -------------------------------------------------------


        [Fact]
        public void AcceptFriend_shouldWork_WhenIncomingRelationExists()
        {
            // Given: A friendship request from one user to another
            var self = Faker.User();
            var friend = Faker.User();
            self.AddFriend(friend);

            // When: The recipient accepts the friend request
            friend.AcceptFriend(self);

            // Then: The social relations should now be established
            Assert.True(self.GetRelation(friend, self).Type == RelationType.ESTABLISHED);
            Assert.True(friend.GetRelation(self, friend).Type == RelationType.ESTABLISHED);
        }


        [Fact]
        public void AcceptFriend_shouldFail_WhenNoIncomingRelationExists()
        {
            // Given: Two unrelated users
            var self = Faker.User();
            var friend = Faker.User();

            // When / Then: Attempting to accept a non-existent friend request should throw an exception
            Assert.Throws<UserException>(() => self.AcceptFriend(friend));
        }




        // CANCEL FRIEND -------------------------------------------------------


        [Fact]
        public void CancelFriend_shouldWork_WhenAnyRelationExists()
        {
            // Given: A user who has received a friend request
            var self = Faker.User();
            var friend = Faker.User();
            self.AddFriend(friend);

            // When: The recipient cancels the friend request
            friend.RemoveFriend(self);

            // Then: The social relations should be null (friendship cancelled)
            Assert.Null(self.GetRelationOrNull(friend, self));
            Assert.Null(friend.GetRelationOrNull(friend, self));
        }


        [Fact]
        public void CancelFriend_shouldFail_WhenNoRelationExists()
        {
            // Given: Two unrelated users
            var self = Faker.User();
            var friend = Faker.User();

            // When / Then: Attempting to cancel a non-existent friendship should throw an exception
            Assert.Throws<UserException>(() => self.RemoveFriend(friend));
        }



        // CREATE MESSENGER ----------------------------------------------------

        [Fact]
        public void CreateMessenger_ShouldWork_WhenUniqueParticipantsGiven()
        {
            // Given: A user and unique set of participants
            var user = Faker.User();
            var participants = new HashSet<User> { Faker.User(), Faker.User() };

            // When: Creating a new messenger entry
            var messengerEntry = user.CreateMessenger(participants, "Test Title", "Test Description");

            // Then: The messenger entry should be successfully added to the user's messengers
            Assert.Contains(messengerEntry, user.Messengers);
        }


        [Fact]
        public void CreateMessenger_ShouldFail_WhenDuplicateParticipantsGiven()
        {
            // Given: A user and a set of participants already in a messenger
            var user = Faker.User();
            var participants = new HashSet<User> { Faker.User() };
            user.CreateMessenger(participants, "Test Title", "Test Description");

            // When / Then: Creating another messenger with the same participants should throw an exception
            Assert.Throws<MessengerException>(() => user.CreateMessenger(participants, "Another Title", "Another Description"));
        }



        // DELETE MESSENGER ----------------------------------------------------

        [Fact]
        public void DeleteMessenger_ShouldWork_WhenMessengerExists()
        {
            // Given: A user with an existing messenger entry
            var user = Faker.User();
            var participants = new HashSet<User> { Faker.User() };
            var messengerEntry = user.CreateMessenger(participants, "Test Title", "Test Description");

            // When: Deleting the messenger entry
            var deletedEntry = user.DeleteMessenger(messengerEntry.Guid);

            // Then: The messenger entry should be removed from the user's messengers
            Assert.DoesNotContain(deletedEntry, user.Messengers);
        }


        [Fact]
        public void DeleteMessenger_ShouldFail_WhenMessengerDoesNotExist()
        {
            // Given: A user without a specific messenger entry
            var user = Faker.User();
            var randomGuid = Guid.NewGuid();

            // When / Then: Deleting a non-existent messenger should throw an exception
            Assert.Throws<MessengerException>(() => user.DeleteMessenger(randomGuid));
        }


    }
}

