using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatsWebApi.Components.Types.JWT
{
    public class AuthOptions
    {
        public required string Issuer { get; init; }
        public required string Audience { get; init; }
        public required string Key { get; init; }

        public SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}
