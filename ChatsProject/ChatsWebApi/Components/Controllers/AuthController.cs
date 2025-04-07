using ChatsWebApi.Components.Repositories.Users;
using ChatsWebApi.Components.Types.Database;
using ChatsWebApi.Components.Types.JWT;
using ChatsWebApi.Components.Types.Roles;
using FluentValidation;
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
        private readonly IValidator<LoginFields> _lfValidator;

        public AuthController(IAuthOptions authOptions, IUsersRepository usersRepo, IAdminLogs adminLogs, IValidator<LoginFields> lfValidator)
        {
            _authOptions = authOptions;
            _usersRepo = usersRepo;
            _adminLogs = adminLogs;
            _lfValidator = lfValidator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<TokenPair>> Register([FromBody] LoginFields loginFields)
        {
            await _lfValidator.ValidateAndThrowAsync(loginFields);

            var user = new User
            {
                NickName = loginFields.NickName,
                Password = loginFields.Password,
                Role = GetRole(loginFields).ToString(),
                RefreshToken = Guid.NewGuid().ToString()
            };

            user = await _usersRepo.CreateAsync(user);
            if (user != null)
            {
                return Ok(new TokenPair
                {
                    AccessToken = await GetJwtToken(user.NickName, user.Role),
                    RefreshToken = user.RefreshToken
                });
            }
            return BadRequest("This nickname already used!");
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<TokenPair>> Login([FromBody] LoginFields loginFields)
        {
            await _lfValidator.ValidateAndThrowAsync(loginFields);

            User? user = await _usersRepo.IsExistAsync(loginFields.NickName, loginFields.Password);
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

        private Role GetRole(LoginFields loginFields)
        {
            var role = Role.User;
            if (_adminLogs.IsAdmin(loginFields))
                role = Role.Admin;
            return role;
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
