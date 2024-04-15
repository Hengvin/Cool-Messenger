using System;
namespace SPG.Messenger.Domain.Exceptions
{
    public class RepositoryReadException : Exception
    {
        public RepositoryReadException()
            : base()
        { }

        public RepositoryReadException(string message)
            : base(message)
        { }

        public RepositoryReadException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public static RepositoryReadException FromNotFound(int id, string entity)
        {
            return new RepositoryReadException($"{entity} with Id {id} not found!");
        }
    }
}

