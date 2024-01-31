namespace Identity.BusinessLogic.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public IEnumerable<string> Audiences { get; set; }
        public string Key { get; set; }
        public int ExpirationMins { get; set; }
    }
}
