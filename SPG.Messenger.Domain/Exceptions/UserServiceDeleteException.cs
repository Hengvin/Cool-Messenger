using System;
namespace SPG.Messenger.Domain.Exceptions
{
    public class UserServiceDeleteException : Exception
    {
        public UserServiceDeleteException()
            : base()
        { }

        public UserServiceDeleteException(string message)
            : base(message)
        { }

        public UserServiceDeleteException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

