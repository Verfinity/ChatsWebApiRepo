using ChatsWebApi.Components.Repositories.Users;
using ChatsWebApi.Components.Types.Database;
using ChatsWebApi.Components.Types.JWT;
using ChatsWebApi.Components.Types.Roles;
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
        private readonly IAuthOptions _authOptions;
        private readonly IUsersRepository _usersRepo;
        private readonly IAdminLogs _adminLogs;

        public AuthController(IAuthOptions authOptions, IUsersRepository usersRepo, IAdminLogs adminLogs)
        {
            _authOptions = authOptions;
            _usersRepo = usersRepo;
            _adminLogs = adminLogs;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<TokenPair>> Register([FromBody] User user)
        {
            SetRole(ref user);

            string refreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;

            User? newUser = await _usersRepo.CreateAsync(user);
            if (newUser != null)
            {
                return Ok(new TokenPair
                {
                    AccessToken = await GetJwtToken(user.NickName, user.Role),
                    RefreshToken = refreshToken
                });
            }
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
            User? user = await _usersRepo.IsExistAsync(loginContext.NickName, loginContext.Password);
            if (user != null)
                return Ok(new TokenPair
                {
                    AccessToken = await GetJwtToken(user.NickName, user.Role),
                    RefreshToken = user.RefreshToken
                });
            return BadRequest("Wrong password or nickname!");
        }

        [HttpGet]
        [Route("refresh/{refreshToken}")]
        public async Task<ActionResult<TokenPair>> Refresh([FromRoute] string refreshToken)
        {
            User? user = await _usersRepo.GetByRefreshTokenAsync(refreshToken);
            if (user != null)
            {
                string newRefreshToken = Guid.NewGuid().ToString();
                await _usersRepo.SetRefreshTokenByIdAsync(newRefreshToken, user.Id);

                return Ok(new TokenPair
                {
                    AccessToken = await GetJwtToken(user.NickName, user.Role),
                    RefreshToken = newRefreshToken
                });
            }
            return BadRequest("Refresh token is invalid!");
        }

        private void SetRole(ref User user)
        {
            AdminLog adminLog = new AdminLog
            {
                NickName = user.NickName,
                Password = user.Password
            };
            Role role = Role.User;
            if (_adminLogs.IsAdmin(adminLog))
                role = Role.Admin;
            user.Role = role.ToString();
        }

        private async Task<string> GetJwtToken(string nickName, string role)
        {
            var claims = new List<Claim>() 
            { 
                new Claim(ClaimTypes.Name, nickName),
                new Claim(ClaimTypes.Role, role)
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
