using ChatsFrontend.Logic.SavingData.TokensSaver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChatsFrontend.Logic.SavingData.UserData
{
    public class UserDataReader : IUserDataReader
    {
        private readonly ITokenPairSaver _tokenPairSaver;

        public UserDataReader(ITokenPairSaver tokenPairSaver)
        {
            _tokenPairSaver = tokenPairSaver;
        }

        public async Task<IEnumerable<Claim>> ReadUserDataAsync()
        {
            var tokenPair = await _tokenPairSaver.GetDataAsync();
            if (tokenPair == null)
                throw new UnauthorizedAccessException("Can't get tokens from storage!");

            var jwt = tokenPair.AccessToken;
            return new JwtSecurityTokenHandler().ReadJwtToken(jwt).Claims;
        }
    }
}
