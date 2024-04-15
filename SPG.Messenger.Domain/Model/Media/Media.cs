namespace SPG.Messenger.Domain.MediaDomain
{
    using SPG.Messenger.Domain.Model;
    using SPG.Messenger.Domain.Model.Validation;

    public class Media : BaseEntity
    {
        public string Filename { get; }
        public string MimeType { get; }
        public long Size { get; }
        public int? Width { get; }
        public int? Height { get; }


        protected Media() {}

        public Media(string filename, string mimeType, long size, int? width, int? height)
        {
            Filename = Guard.IsNotNullOrEmpty(filename, nameof(Filename));
            MimeType = Guard.IsNotNullOrEmpty(mimeType, nameof(MimeType));
            Size = Guard.IsPositive(size, nameof(Size));
            Width = Guard.IsPositiveOrNullable(width, nameof(Width));
            Height = Guard.IsPositiveOrNullable(height, nameof(Height));
        }
    }
}
