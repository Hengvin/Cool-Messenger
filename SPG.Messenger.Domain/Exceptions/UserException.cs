namespace SPG.Messenger.Domain.Exceptions
{
    // Custom exception for user relation-related issues
    public class UserException : Exception
    {
        // Basic constructor with just a message
        public UserException(string message)
            : base(message)
        {
        }

        // Constructor with a message and an inner exception
        public UserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

