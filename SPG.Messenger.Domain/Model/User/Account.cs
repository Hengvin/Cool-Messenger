using System;
namespace SPG.Messenger.Domain.Model.UserDomain
{
    // Value Object
    public class Account
    {
        public bool Verified { get; set; } = false;

        public bool Locked { get; set; } = false;
    }
}

