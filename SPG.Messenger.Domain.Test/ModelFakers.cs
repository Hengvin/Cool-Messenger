using System;
using Bogus;
using SPG.Messenger.Domain.Model.MessengerDomain;
using SPG.Messenger.Domain.Model.UserDomain;

namespace SPG.Messenger.Domain.Test
{
	public class ModelFakers
	{
        private readonly Faker<User> _userFaker;
        private readonly Faker<MessengerEntry> _messengerFaker;

        public MessengerEntry Messenger() => _messengerFaker.Generate();
        public User User() => _userFaker.Generate();

        public ModelFakers()
		{
            var profileFaker = new Faker<Profile>()
               .CustomInstantiator(f => new Profile(
                   firstName: f.Name.FirstName(),
                   lastName: f.Name.LastName()));

            _userFaker = new Faker<User>()
                .CustomInstantiator(f => new User(
                    email: f.Internet.Email(),
                    password: f.Internet.Password(),
                    profile: profileFaker.Generate())
                );

            _messengerFaker = new Faker<MessengerEntry>()
                .CustomInstantiator(f =>
                {
                    var creator = _userFaker.Generate();
                    var participants = new HashSet<User> { creator };
                    return new MessengerEntry(
                        creator,
                        participants,
                        Truncate(f.Lorem.Sentence(), 100),
                        Truncate(f.Lorem.Paragraph(), 200));
                });
        }


        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value[..maxLength];
        }
    }
}

