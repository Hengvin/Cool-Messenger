using System;
using SPG.Messenger.Domain.Model.Validation;

namespace SPG.Messenger.Domain.Model.UserDomain
{
	public abstract class BaseUser : BaseEntity
	{
        // Properties ---------------------------------------------------------

        private string _email = string.Empty;
        public string Email {
            get => _email;
            set => _email = Guard.IsValidEmail(value, nameof(Email)); }

        private string _password = string.Empty;
        public string Password {
            get => _password;
            set => _password = Guard.IsStrongPassword(value, nameof(Password));
        }



        // Ctor ----------------------------------------------------------------

        protected BaseUser() { }

        public BaseUser(string email, string password)
		{
            Email = email;
            Password = password;
		}
    }
}

