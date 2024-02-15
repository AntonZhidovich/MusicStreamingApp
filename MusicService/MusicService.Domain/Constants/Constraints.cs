namespace MusicService.Domain.Constants
{
    public static class Constraints
    {
        public const int idMaxLength = 50;
        public const string timeSpanFormat = "hh\\:mm\\:ss";
        public const int authorNameMaxLength = 50;
        public const int genreNameMaxLength = 30;
        public const int releaseNameMaxLength = 50;
        public const int releaseTypeMaxLength = 20;
        public const int descriptionMaxLength = 300;
        public const int songTitleMaxLength = 50;
        public const int songSourceMaxLength = 200;
        public const int playlistNameMaxLength = 50;
        public static readonly DateTime minimumDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}
