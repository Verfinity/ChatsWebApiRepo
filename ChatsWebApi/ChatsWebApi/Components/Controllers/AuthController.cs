using ChatsWebApi.Components.Repositories;
using ChatsWebApi.Components.Repositories.Users;
using ChatsWebApi.Components.Types.Database;
using ChatsWebApi.Components.Types.JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace ChatsWebApi.Components.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthOptions _authOptions;
        private readonly IUsersRepository _usersRepo;

        public AuthController(AuthOptions authOptions, IUsersRepository usersRepo)
        {
            _authOptions = authOptions;
            _usersRepo = usersRepo;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<TokenPair>> Register([FromBody] User user)
        {
            string refreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;

            int? newUserId = await _usersRepo.CreateAsync(user);
            if (newUserId != null)
                return Ok(new TokenPair
                {
                    AccessToken = await GetJwtToken(user.NickName),
                    RefreshToken = refreshToken
                });
            return BadRequest("This nickname already used!");
        }

        public record LoginContext
        {
            public required string NickName { get; set; }
            public required string Password { get; set; }
        };

        public record TokenPair
        {
            public required string AccessToken { get; set; }
            public required string RefreshToken { get; set; }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<TokenPair>> Login([FromBody] LoginContext loginContext)
        {
            User? user = await _usersRepo.IsUserExistAsync(loginContext.NickName, loginContext.Password);
            if (user != null)
                return Ok(new TokenPair
                {
                    AccessToken = await GetJwtToken(user.NickName),
                    RefreshToken = user.RefreshToken
                });
            return BadRequest("Wrong password or nickname!");
        }

        [HttpGet]
        [Route("refresh/{refreshToken}")]
        public async Task<ActionResult<TokenPair>> Refresh([FromRoute] string refreshToken)
        {
            List<User> users = await _usersRepo.GetAllAsync();
            User? user = users.FirstOrDefault(i => i.RefreshToken == refreshToken);
            if (user != null)
            {
                string newRefreshToken = Guid.NewGuid().ToString();
                await _usersRepo.SetRefreshTokenByNickNameAsync(newRefreshToken, user.NickName);

                return Ok(new TokenPair
                {
                    AccessToken = await GetJwtToken(user.NickName),
                    RefreshToken = newRefreshToken
                });
            }
            return BadRequest("Refresh token is invalid!");
        }

        private async Task<string> GetJwtToken(string nickName)
        {
            var claims = new List<Claim>() 
            { 
                new Claim(ClaimTypes.Name, nickName),
                new Claim(ClaimTypes.Role, "User")
            };
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
