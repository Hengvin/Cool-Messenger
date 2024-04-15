using System.Net.Mail;

namespace SPG.Messenger.Domain.Model.Validation
{
    // TODO Unit Tests
    public static class Guard
    {
        public static string IsNotNullOrEmpty(string value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{propertyName} cannot be null or empty.");

            return value;
        }

        public static string IsValidLength(string value, int maxLength, string propertyName)
        {
            if (value.Length > maxLength)
                throw new ArgumentException($"{propertyName} cannot be more than {maxLength} characters.");

            return value;
        }

        public static string IsStrongPassword(string value, string propertyName)
        {
            if (value.Length < 8)
                throw new ArgumentException($"{propertyName} must be 8 characters or more.");

            return value;
        }

        public static long IsPositive(long value, string propertyName)
        {
            if (value < 0)
                throw new ArgumentException($"{propertyName} cannot be negative.");

            return value;
        }

        public static int? IsPositiveOrNullable(int? value, string propertyName)
        {
            if (value.HasValue && value <= 0)
                throw new ArgumentException($"{propertyName} must be a positive number.");

            return value;
        }

        public static string IsValidEmail(string value, string propertyName)
        {
            try
            {
                _ = new MailAddress(value);
                return value;
            }
            catch
            {
                throw new ArgumentException($"{propertyName} has an invalid email format.");
            }
        }
    }
}
