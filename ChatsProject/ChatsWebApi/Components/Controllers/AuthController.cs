using ChatsWebApi.Components.Repositories.Users;
using ChatsWebApi.Components.Types.Database;
using ClassLibrary;
using ChatsWebApi.Components.Types.JWT.Logic;
using ChatsWebApi.Components.Types.JWT.Options;
using ChatsWebApi.Components.Types.Roles;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] LoginFields loginFields)
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
                SetTokenPairInCookie(_jwtCreator.CreateJWT(user.NickName, user.Role, _authOptions), user.RefreshToken, HttpContext.Response);
                return Ok();
            }
            return BadRequest("This nickname already used!");
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginFields loginFields)
        {
            await _lfValidator.ValidateAndThrowAsync(loginFields);

            User? user = await _usersRepo.IsExistAsync(loginFields.NickName, loginFields.Password);
            if (user != null)
            {
                SetTokenPairInCookie(_jwtCreator.CreateJWT(user.NickName, user.Role, _authOptions), user.RefreshToken, HttpContext.Response);
                return Ok();
            }
            return BadRequest("Wrong password or nickname!");
        }

        [HttpGet]
        [Route("refresh/{refreshToken}")]
        public async Task<ActionResult> Refresh([FromRoute] string refreshToken)
        {
            User? user = await _usersRepo.GetByRefreshTokenAsync(refreshToken);
            if (user != null)
            {
                string newRefreshToken = Guid.NewGuid().ToString();
                await _usersRepo.SetRefreshTokenByIdAsync(newRefreshToken, user.Id);

                SetTokenPairInCookie(_jwtCreator.CreateJWT(user.NickName, user.Role, _authOptions), newRefreshToken, HttpContext.Response);
                return Ok();
            }
            return BadRequest("Refresh token is invalid!");
        }

        private void SetTokenPairInCookie(string jwtToken, string refreshToken, HttpResponse response)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            };
            response.Cookies.Append("X-Access-Token", jwtToken, cookieOptions);
            response.Cookies.Append("X-Refresh-Token", refreshToken, cookieOptions);
        }
    }
}
