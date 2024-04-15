using System;
using System.ComponentModel.DataAnnotations;

namespace SPG.Messenger.Domain.Dtos
{
    public record RegisterUserCommand(
        string Email,

        string Password,

        [StringLength(maximumLength: 255, ErrorMessage = "FirstName too long")]
        string FirstName,

        [StringLength(maximumLength: 255, ErrorMessage = "LastName too long")]
        string LastName,


        // Optional, when registering a user we also create messenger with those participants
        List<int> ParticipantIds,
        string MessengerTitle,
        string MessengerDescription
    );
}

