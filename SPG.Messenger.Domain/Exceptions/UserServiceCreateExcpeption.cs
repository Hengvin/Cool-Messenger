using System;
namespace SPG.Messenger.Domain.Exceptions
{
    public class UserServiceCreateException : Exception
    {
        public UserServiceCreateException()
            : base()
        { }

        public UserServiceCreateException(string message)
            : base(message)
        { }

        public UserServiceCreateException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

