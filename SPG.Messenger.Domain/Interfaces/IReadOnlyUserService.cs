using System;
using SPG.Messenger.Domain.Dtos;

namespace SPG.Messenger.Domain.Interfaces
{
	public interface IReadOnlyUserService
	{
        List<UserDto> GetAll();
        UserDto GetSingle(int id);
        List<UserDto> FilterByEmail(string email);
    }
}

