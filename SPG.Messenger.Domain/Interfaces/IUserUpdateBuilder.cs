using System;
namespace SPG.Messenger.Domain.Interfaces
{
	public interface IUserUpdateBuilder
	{
        IUserUpdateBuilder WithFirstName(string firstName);

        IUserUpdateBuilder WithLastName(string firstName);

        int Save();
    }
}

