using SPG.Messenger.Domain.Model.Validation;

namespace SPG.Messenger.Domain.Model.UserDomain
{
    // Value Object
    public class Profile
    {
        // FirstName
        private string _firstName = string.Empty;
        public string FirstName
        {
            get => _firstName;
            set => _firstName = Guard.IsValidLength(value, 255, nameof(FirstName));
        }


        // LastName
        private string _lastName = string.Empty;
        public string LastName
        {
            get => _lastName;
            set => _lastName = Guard.IsValidLength(value, 255, nameof(LastName));
        }


        protected Profile() { }


        // Ctor
        public Profile(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
