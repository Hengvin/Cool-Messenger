using System;
using SPG.Messenger.Domain.MediaDomain;
using SPG.Messenger.Domain.Model.MessengerDomain;
using SPG.Messenger.Domain.Model.UserDomain;
using SPG.Messenger.Domain.Model.Validation;

namespace SPG.Messenger.Domain.Model.Messenger
{
    // Entity
    public class Message : BaseEntity
    {
        // Properties ---------------------------------------------------------


        // Navigation

        // Who is the Sender
        public virtual User SenderNavigation { get; set; } = default!;
        public virtual MessengerEntry MessengerNavigation { get; set; } = default!;


        // Media
        public readonly Media? Media = default!;


        // Text
        private string _text = string.Empty;
        public string Text {
            get => _text;
            set => _text = Guard.IsValidLength(value, 1000, nameof(Text));
        }


        // Discriminator
        public readonly MessageType Type;



        // Ctor ----------------------------------------------------------------


        // Constructor for the Framework
        protected Message() { }

        // Constructor for us Developers
        public Message(User sender, MessengerEntry messenger, string text, Media? media)
        {
            SenderNavigation = sender;
            MessengerNavigation = messenger;
            Text = text;
            Media = media;
            Type = media == null? MessageType.MEDIA : MessageType.TEXT;
        }
    }
}

