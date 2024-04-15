using System;
namespace SPG.Messenger.Domain.Exceptions
{
	public class RepositoryCreateException : Exception
	{
        public RepositoryCreateException() : base()
        { }

        public RepositoryCreateException(string message) : base(message)
        { }

        public RepositoryCreateException(string message, Exception innerException) : base(message, innerException)
        { }

        public static RepositoryCreateException FromDbError(Exception ex, string entity)
        {
            return new RepositoryCreateException($"Create {entity} failed!", ex);
        }
    }
}

