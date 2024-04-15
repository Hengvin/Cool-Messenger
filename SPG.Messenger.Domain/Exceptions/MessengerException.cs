namespace SPG.Messenger.Domain.Exceptions
{
    // Custom exception for user relation-related issues
    public class MessengerException : Exception
    {
        // Basic constructor with just a message
        public MessengerException(string message)
            : base(message)
        {
        }

        // Constructor with a message and an inner exception
        public MessengerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

