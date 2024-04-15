using System;
namespace SPG.Messenger.Domain.Exceptions
{
    public class UserServiceUpdateException : Exception
    {
        public UserServiceUpdateException()
            : base()
        { }

        public UserServiceUpdateException(string message)
            : base(message)
        { }

        public UserServiceUpdateException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

