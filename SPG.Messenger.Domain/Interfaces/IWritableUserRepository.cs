using System;
using SPG.Messenger.Domain.Model.UserDomain;

namespace SPG.Messenger.Domain.Interfaces
{
	public interface IWritableUserRepository
	{
		int Create(User entity);

        IUserUpdateBuilder UpdateBuilder(User entity);

        void Delete(int id);
    }
}

