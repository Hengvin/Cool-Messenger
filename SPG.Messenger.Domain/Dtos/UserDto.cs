using System;
namespace SPG.Messenger.Domain.Dtos
{
	public record UserDto(
		int Id,
		string Email,
		string FirstName,
		string LastName
	);
}

