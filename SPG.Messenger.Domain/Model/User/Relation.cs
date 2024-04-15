namespace SPG.Messenger.Domain.Model.UserDomain
{
    // Entity
    public class Relation : BaseEntity
    {

        // Properties ---------------------------------------------------------

        public RelationType Type { get; private set; }

        public virtual User Self { get; private set; } = default!;
        public virtual User Friend { get; private set; } = default!;



        // Ctor ----------------------------------------------------------------

        protected Relation() { }

        public Relation(User self, User friend, RelationType type)
        {
            Self = self;
            Friend = friend;
            Type = type;
        }



        // Methods -------------------------------------------------------------

        public override bool Equals(object? obj)
        {
            // Equality is based on Self and Friend
            return obj is Relation relation &&
                   Self == relation.Self &&
                   Friend == relation.Friend;
        }


        public override int GetHashCode()
        {
            // HashCode based on Self and Friend
            return HashCode.Combine(Self, Friend);
        }
    }
}

