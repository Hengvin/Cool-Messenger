using System;
namespace SPG.Messenger.Domain.Exceptions
{
    public class UserServiceReadException : Exception
    {
        public UserServiceReadException()
            : base()
        { }

        public UserServiceReadException(string message)
            : base(message)
        { }

        public UserServiceReadException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

