using System;
namespace SPG.Messenger.Domain.Exceptions
{
    public class RepositoryDeleteException : Exception
    {
        public RepositoryDeleteException()
            : base()
        { }

        public RepositoryDeleteException(string message)
            : base(message)
        { }

        public RepositoryDeleteException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public static RepositoryDeleteException FromDbError(Exception ex, string entity)
        {
            return new RepositoryDeleteException($"Delete {entity} failed!", ex);
        }
    } 
}

