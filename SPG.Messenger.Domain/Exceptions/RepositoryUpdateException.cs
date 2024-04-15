using System;
namespace SPG.Messenger.Domain.Exceptions
{
    public class RepositoryUpdateException : Exception
    {
        public RepositoryUpdateException()
            : base()
        { }

        public RepositoryUpdateException(string message)
            : base(message)
        { }

        public RepositoryUpdateException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public static RepositoryCreateException FromDbError(Exception ex, string entity)
        {
            return new RepositoryCreateException($"Update {entity} failed!", ex);
        }
    }
}

