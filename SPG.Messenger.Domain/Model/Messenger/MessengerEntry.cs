using SPG.Messenger.Domain.Exceptions;
using SPG.Messenger.Domain.MediaDomain;
using SPG.Messenger.Domain.Model.Messenger;
using SPG.Messenger.Domain.Model.UserDomain;
using SPG.Messenger.Domain.Model.Validation;

namespace SPG.Messenger.Domain.Model.MessengerDomain
{
	// Root Aggregate
    public class MessengerEntry : BaseEntity
    {

        // Properties ---------------------------------------------------------

        // Title
        private string _title = string.Empty;
        public string Title {
            get => _title;
            set => _title = Guard.IsValidLength(value, 100, nameof(Title)); }


        // Description
        private string _description = string.Empty;
        public string Description {
            get => _description;
            set => _description = Guard.IsValidLength(value, 200, nameof(Description)); }


        // Who is part of the MessengerEntry
        public IReadOnlySet<User> Participants => _participants;
        private readonly HashSet<User> _participants = new();


        // Messages been sent in the MessengerEntry
        public IReadOnlyList<Message> Messages => _messages;
        private readonly List<Message> _messages = new();


        // Back Reference to the creator of the MessengerEntry
        // public int? UserId { get; set; }
        public virtual User UserNavigation { get; set; } = default!;



        // Ctor ----------------------------------------------------------------

        // Constructor for the Framework
        protected MessengerEntry() { }

        // Constructor for us Developers
        public MessengerEntry(User creator, HashSet<User> participantIds, string title, string description)
        {
            UserNavigation = creator;
            _participants = participantIds;
            Title = title;
            Description = description;
        }



        // Methods -------------------------------------------------------------

        public void AddParticipant(User newParticipant)
        {
            if (!_participants.Add(newParticipant))
            {
                throw new MessengerException("Participant already part of messenger");
            }

        }


        public void RemoveParticipant(User removeParticipant)
        {
            if (!_participants.Remove(removeParticipant))
            {
                throw new MessengerException("Participant not part of messenger");
            }
        }


        public Message SendTextMessage(User sender, string text)
        {
            if (!_participants.Contains(sender))
                throw new MessengerException("Sender not part of participants");


            Message message = new(sender, this, text, null);
            _messages.Add(message);

            return message;
        }


        public Message SendMediaMessage(User sender, string text, Media media)
        {
            if (!_participants.Contains(sender))
                throw new MessengerException("Sender not part of participants");

            Message message = new(sender, this, text, media);
            _messages.Add(message);

            return message;
        }


        public Message DeleteMessage(User sender, Guid messageId)
        {
            Message messageToRemove = _messages.First(message => message.Guid.Equals(messageId))
                ?? throw new MessengerException("Message not found.");

            if (!sender.Equals(messageToRemove.SenderNavigation))
                throw new MessengerException("Only sender can delete it's message");

            _messages.Remove(messageToRemove);
            return messageToRemove;
        }
    }
    
}

