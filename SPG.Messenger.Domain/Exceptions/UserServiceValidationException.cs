using System;
namespace SPG.Messenger.Domain.Exceptions
{
    public class UserServiceValidationException : Exception
    {
        public UserServiceValidationException()
            : base()
        { }

        public UserServiceValidationException(string message)
            : base(message)
        { }

        public UserServiceValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public static UserServiceValidationException FromExistingEmail(string email)
        {
            return new UserServiceValidationException($"User with {email} already exists");
        }
    }
}

