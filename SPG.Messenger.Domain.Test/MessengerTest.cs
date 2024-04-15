using System;
using SPG.Messenger.Domain.Exceptions;
using SPG.Messenger.Domain.MediaDomain;
using Xunit;

namespace SPG.Messenger.Domain.Test
{
	public class MessengerTest
	{
        private readonly ModelFakers Faker = new();



        // ADD PARTICIPANT -----------------------------------------------------


        [Fact]
        public void AddParticipant_ShouldWork_WhenNewParticipantAdded()
        {
            // Given: A MessengerEntry with a set of participants
            var messengerEntry = Faker.Messenger();
            var newParticipant = Faker.User();

            // When: Adding a new participant to the messenger entry
            messengerEntry.AddParticipant(newParticipant);

            // Then: The new participant should be added successfully
            Assert.Contains(newParticipant, messengerEntry.Participants);
        }



        [Fact]
        public void AddParticipant_ShouldFail_WhenParticipantAlreadyExists()
        {
            // Given: A MessengerEntry with a set of participants, including an existing participant
            var messengerEntry = Faker.Messenger();
            var existingParticipant = messengerEntry.Participants.First();

            // When / Then: Adding an existing participant should throw an exception
            Assert.Throws<MessengerException>(() =>
                messengerEntry.AddParticipant(existingParticipant));
        }



        // REMOVE PARTICIPANT --------------------------------------------------


        [Fact]
        public void RemoveParticipant_ShouldWork_WhenExistingParticipantRemoved()
        {
            // Given: A MessengerEntry with a set of participants
            var messengerEntry = Faker.Messenger();
            var participantToRemove = messengerEntry.Participants.First();

            // When: Removing an existing participant from the messenger entry
            messengerEntry.RemoveParticipant(participantToRemove);

            // Then: The participant should be removed successfully
            Assert.DoesNotContain(participantToRemove, messengerEntry.Participants);
        }


        [Fact]
        public void RemoveParticipant_ShouldFail_WhenParticipantDoesNotExist()
        {
            // Given: A MessengerEntry and a user not in the participant list
            var messengerEntry = Faker.Messenger();
            var nonParticipant = Faker.User();

            // When / Then: Removing a non-participant should throw an exception
            Assert.Throws<MessengerException>(() =>
                messengerEntry.RemoveParticipant(nonParticipant));
        }



        // SEND TEXT MESSAGE ---------------------------------------------------


        [Fact]
        public void SendTextMessage_ShouldWork_WhenParticipantSendsMessage()
        {
            // Given: A MessengerEntry and a participant
            var messengerEntry = Faker.Messenger();
            var participant = messengerEntry.Participants.First();
            var messageText = "Hello World";

            // When: The participant sends a text message
            var message = messengerEntry.SendTextMessage(participant, messageText);

            // Then: The message should be added to the messenger entry
            Assert.Contains(message, messengerEntry.Messages);
        }


        [Fact]
        public void SendTextMessage_ShouldFail_WhenNonParticipantSendsMessage()
        {
            // Given: A MessengerEntry and a non-participant user
            var messengerEntry = Faker.Messenger();
            var nonParticipant = Faker.User();
            var messageText = "Hello World";

            // When / Then: Sending a message by a non-participant should throw an exception
            Assert.Throws<MessengerException>(() =>
                messengerEntry.SendTextMessage(nonParticipant, messageText));
        }



        // SEND MEDIA MESSAGE --------------------------------------------------

        [Fact]
        public void SendMediaMessage_ShouldWork_WhenParticipantSendsMediaMessage()
        {
            // Given: A MessengerEntry with participants and a media object
            var messengerEntry = Faker.Messenger();
            var participant = messengerEntry.Participants.First();
            var media = new Media("filename.jpg", "image/jpeg", 1024, 800, 600);

            // When: The participant sends a media message
            var message = messengerEntry.SendMediaMessage(participant, "Check this out!", media);

            // Then: The message should be added to the messenger entry's messages
            Assert.Contains(message, messengerEntry.Messages);
        }


        [Fact]
        public void SendMediaMessage_ShouldFail_WhenNonParticipantSendsMediaMessage()
        {
            // Given: A MessengerEntry and a media object, and a non-participant user
            var messengerEntry = Faker.Messenger();
            var nonParticipant = Faker.User(); // Ensure this user is not in messengerEntry.Participants
            var media = new Media("filename.jpg", "image/jpeg", 1024, 800, 600);

            // When / Then: Sending a media message by a non-participant should throw an exception
            Assert.Throws<MessengerException>(() =>
                messengerEntry.SendMediaMessage(nonParticipant, "Check this out!", media));
        }




        // DELETE MESSAGE ------------------------------------------------------

        [Fact]
        public void DeleteMessage_ShouldWork_WhenSenderDeletesTheirOwnMessage()
        {
            // Given: A MessengerEntry with a message from a participant
            var messengerEntry = Faker.Messenger();
            var participant = messengerEntry.Participants.First();
            var message = messengerEntry.SendTextMessage(participant, "Hello World");

            // When: The sender deletes their own message
            var deletedMessage = messengerEntry.DeleteMessage(participant, message.Guid);

            // Then: The message should be removed from the messenger entry
            Assert.DoesNotContain(deletedMessage, messengerEntry.Messages);
        }


        [Fact]
        public void DeleteMessage_ShouldFail_WhenNonSenderTriesToDeleteMessage()
        {
            // Given: A MessengerEntry with a message and a different non-sender participant
            var messengerEntry = Faker.Messenger();
            var sender = messengerEntry.Participants.First();
            var message = messengerEntry.SendTextMessage(sender, "Hello World");
            var nonSender = Faker.User(); // Ensure this user is not in messengerEntry.Participants

            // When / Then: Trying to delete a message by someone other than the sender should throw an exception
            Assert.Throws<MessengerException>(() =>
                messengerEntry.DeleteMessage(nonSender, message.Guid));
        }


    }
}

