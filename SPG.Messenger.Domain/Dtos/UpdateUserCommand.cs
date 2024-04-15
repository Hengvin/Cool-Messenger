using System;
using System.ComponentModel.DataAnnotations;

namespace SPG.Messenger.Domain.Dtos
{
    public record UpdateUserCommand(
        int Id,

        [StringLength(maximumLength: 255, ErrorMessage = "FirstName zu lange")]
        string FirstName,

        [StringLength(maximumLength: 255, ErrorMessage = "LastName zu lange")]
        string LastName
    );
}


