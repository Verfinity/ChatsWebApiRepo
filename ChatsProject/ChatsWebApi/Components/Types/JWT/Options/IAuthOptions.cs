using Microsoft.IdentityModel.Tokens;

namespace ChatsWebApi.Components.Types.JWT.Options
{
    public interface IAuthOptions
    {
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public string Key { get; init; }

        public SymmetricSecurityKey GetSymmetricSecurityKey();
    }
}
