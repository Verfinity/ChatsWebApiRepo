using ChatsWebApi.Components.Types.JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChatsWebApi.Components.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthOptions _authOptions;

        public AuthController(AuthOptions authOptions)
        {
            _authOptions = authOptions;
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<string> GetJwtToken([FromRoute] string name)
        {
            var claims = new List<Claim>() { new Claim(ClaimTypes.Name, name)};

            var jwt = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
