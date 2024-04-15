using SPG.Messenger.Domain.Dtos;
using SPG.Messenger.Domain.Exceptions;
using SPG.Messenger.Domain.Model.MessengerDomain;

namespace SPG.Messenger.Domain.Model.UserDomain
{
    // Root Aggregate
    public class User : BaseUser
    {

        public Profile Profile { get; private set; } = default!;

        public Account Account { get; private set; } = new Account();

        public readonly UserRole Role = UserRole.USER;


        // Messengers the user has created, for Back Reference see `MessengerEntry` class
        private readonly List<MessengerEntry> _messengers = new();
        public IReadOnlyList<MessengerEntry> Messengers => _messengers;


        // Relations to other Users
        private readonly HashSet<Relation> _relations = new();
        public IReadOnlyCollection<Relation> Relations => _relations;



        // Ctor ----------------------------------------------------------------

        protected User() { }

        public User(string email, string password, Profile profile):
            base(email, password)
        {
            Profile = profile;
        }



        // Methods -------------------------------------------------------------


        // CREATE ESSENGER

        public MessengerEntry CreateMessenger(HashSet<User> participants, string title, string description)
        {
            // Can only create unuqiue Messengers
            if (_messengers.Any(messenger => messenger.Participants.SetEquals(participants)))
            {
                throw new MessengerException("Messenger with such partipants already exists");   
            }

            
            var entry = new MessengerEntry(this, participants, title, description);
            _messengers.Add(entry);

            return entry;

        }


        // DELETE MESSENGER

        public MessengerEntry DeleteMessenger(Guid entryGuid)
        {
            var entryToRemove = _messengers.FirstOrDefault(entry => entry.Guid.Equals(entryGuid))
                ?? throw new MessengerException("Messenger not found");

            _messengers.Remove(entryToRemove);
            return entryToRemove;
        }



        // GET RELATION

        public Relation GetRelation(User friend, User self)
        {
            return GetRelationOrNull(friend, self) ??
                throw new UserException("Non existing relation");
        }

        public Relation? GetRelationOrNull(User friend, User self)
        {
            return _relations.FirstOrDefault(r => r.Self == self && r.Friend == friend);
        }



        // ADD FRIEND

        public void AddFriend(User friend)
        {
            if (this == friend)
                throw new UserException("Cannot add yourself");

            var existingRelation = GetRelationOrNull(friend, this);
            if (existingRelation is null)
            {
                // own side
                _relations.Add(new Relation(this, friend, RelationType.OUTGOING));

                // friend side
                friend._relations.Add(new Relation(friend, this, RelationType.INCOMING));

            }
            else
            {
                throw new UserException("A relation already exists.");
            }
        }


        // ACCEPT FRIEND

        public void AcceptFriend(User friend)
        {
            var selfRelation = GetRelation(friend, this);
            var friendRelation = friend.GetRelation(this, friend);

            if (selfRelation is not null && selfRelation.Type == RelationType.INCOMING)
            {
                // own side
                _relations.Remove(selfRelation);
                _relations.Add(new Relation(this, friend, RelationType.ESTABLISHED));

                // friend side
                friend._relations.Remove(friendRelation);
                friend._relations.Add(new Relation(friend, this, RelationType.ESTABLISHED));
            }
            else
            {
                throw new UserException("Cannot accept, no incoming relation exists.");
            }
        }


        // CANCEL FRIEND or FRIEND-REQUEST

        public void RemoveFriend(User friend)
        {
            var selfRelation = GetRelation(friend, this);
            var friendRelation = friend.GetRelation(this, friend);

            if (selfRelation is not null)
            {
                // own side
                _relations.Remove(selfRelation);

                // friend side
                friend._relations.Remove(friendRelation);
            }
            else
            {
                throw new UserException("Cannot cancel relationship.");
            }
        }



        // DTOs

        public UserDto ToDto()
        {
            return new UserDto(Id, Email, Profile.FirstName, Profile.LastName);
        }


        public static User FromDto(RegisterUserCommand command)
        {
            Profile profile = new(command.FirstName, command.LastName);
            return new(command.Email, command.Password, profile);
        }
    }
}

