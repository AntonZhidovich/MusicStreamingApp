namespace Identity.DataAccess.Entities
{
    public class RefreshToken
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAd { get; set; }
        public DateTime ExpiresAt { get; set;}
    }
}
