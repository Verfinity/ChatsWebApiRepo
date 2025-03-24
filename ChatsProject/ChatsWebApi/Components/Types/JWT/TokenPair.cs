namespace ChatsWebApi.Components.Types.JWT
{
    public class TokenPair
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
