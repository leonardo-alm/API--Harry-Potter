namespace HarryPotterAPI.AuthorizationAndAuthentication
{
    public class TokenConfiguration
    {
        public string Secret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ExpirationTime { get; set; }
        public string UserName { get; set; }
      
    }
}
