using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatsWebApi.Components.Types.JWT.Options
{
    public class AuthOptions : IAuthOptions
    {
        public required string Issuer { get; init; }
        public required string Audience { get; init; }
        public required string Key { get; init; }

        public SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}
