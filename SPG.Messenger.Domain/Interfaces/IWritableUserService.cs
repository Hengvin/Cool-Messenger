using System;
using SPG.Messenger.Domain.Dtos;

namespace SPG.Messenger.Domain.Interfaces
{
	public interface IWritableUserService
	{
		UserDto Register(RegisterUserCommand command);

		UserDto Update(UpdateUserCommand command);

		void Delete(int id);
	}
}

