namespace SPG.Messenger.Domain.Model
{
	
    public abstract class BaseEntity
    {
        // Primary Key
        public int Id { get; set; }

        public Guid Guid { get; set; } = Guid.NewGuid(); // Given in App Layer

        public DateTime CreatedAt { get; set; } = DateTime.Now;


        // Override Equals
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var other = (BaseEntity)obj;
            return Guid.Equals(other.Guid);
        }


        // Override GetHashCode
        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }


        // Overrides ==
        public static bool operator ==(BaseEntity left, BaseEntity right)
        {
            return Equals(left, right);
        }


        // Overrides !=
        public static bool operator !=(BaseEntity left, BaseEntity right)
        {
            return !Equals(left, right);
        }
    }
}

