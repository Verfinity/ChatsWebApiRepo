using ChatsWebApi.Components.Repositories.Users;
using ClassLibrary;
using ChatsWebApi.Components.Types.JWT.Options;
using ChatsWebApi.Components.Types.Roles;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ChatsWebApi.Components.Types.JWT.Logic.Creation;
using Microsoft.AspNetCore.Authorization;

namespace ChatsWebApi.Components.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthOptions _authOptions;
        private readonly IJWTCreator _jwtCreator;
        private readonly IUsersRepository _usersRepo;
        private readonly IValidator<LoginFields> _lfValidator;

        public AuthController(IAuthOptions authOptions, IJWTCreator jwtCreator, IUsersRepository usersRepo, IValidator<LoginFields> lfValidator)
        {
            _authOptions = authOptions;
            _jwtCreator = jwtCreator;
            _usersRepo = usersRepo;
            _lfValidator = lfValidator;
        }

        [HttpGet]
        [Route("ping")]
        [Authorize]
        public async Task<ActionResult<string>> CheckAuthAsync()
        {
            return Ok("pong");
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
                Role = Role.User,
                RefreshToken = Guid.NewGuid().ToString()
            };

            user = await _usersRepo.CreateAsync(user);
            if (user != null)
            {
                return Ok(new TokenPair
                {
                    AccessToken = _jwtCreator.CreateJWT(user, _authOptions),
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
            {
                return Ok(new TokenPair
                {
                    AccessToken = _jwtCreator.CreateJWT(user, _authOptions),
                    RefreshToken = user.RefreshToken
                });
            }
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
                    AccessToken = _jwtCreator.CreateJWT(user, _authOptions),
                    RefreshToken = newRefreshToken
                });
            }
            return BadRequest("Refresh token is invalid!");
        }
    }
}
