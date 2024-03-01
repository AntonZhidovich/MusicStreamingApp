namespace MusicService.Domain.Constants
{
    public static class ExceptionMessages
    {
        public const string UserNotFound = "No user was found.";
        public const string UserAlreadyExists = "User already exists";
        public const string AuthorNotFound = "No author was found.";
        public const string GenreNotFound = "No genre was found.";
        public const string ReleaseNotFound = "No releases was found.";
        public const string SongNotFound = "No song was found.";
        public const string PlaylistNotFound = "No playlist was found";
        public const string PlanNotFound = "No user tariff was specified.";
        public const string UserAlreadyInAuthor = "User is already in author group.";
        public const string SongAlreadyInPlaylist = "Song is already in the playlist.";
        public const string AuthorAlreadyExists = "Author with such name already exists.";
        public const string NoAccessToPlaylist = "User doesn't have acces to this playlist.";
        public const string PlanDoesntAllow = "Current plan does not allow to do this action";
        public const string UserIsNotCreator = "User does not have a creator role.";
        public const string NotAuthorMember = "Only author members can do this action.";
    }
}
