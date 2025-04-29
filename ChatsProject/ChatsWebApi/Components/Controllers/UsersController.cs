using ChatsWebApi.Components.Repositories.Users;
using ChatsWebApi.Components.Types.Database;
using ChatsWebApi.Components.Types.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ChatsWebApi.Components.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepo;

        public UsersController(IUsersRepository usersRepo)
        {
            _usersRepo = usersRepo;
        }

        [HttpPost]
        [Route("add-user-to-chat")]
        public async Task<ActionResult> AddUserToChat([FromBody] ChatsToUsers chatsToUsers)
        {
            if (await _usersRepo.AddUserToChatAsync(chatsToUsers.ChatId, chatsToUsers.UserId))
                return Ok();
            return BadRequest("Incorrect ChatId or UserId");
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsersAsync()
        {
            List<User> users = await _usersRepo.GetAllAsync();
            if (users.IsNullOrEmpty())
                return NoContent();
            return Ok(users);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<User?>> GetUserByIdAsync([FromRoute] int id)
        {
            User? user = await _usersRepo.GetByIdAsync(id);
            if (user != null)
                return Ok(user);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUserAsync([FromRoute] int id)
        {
            if (await _usersRepo.DeleteAsync(id))
                return Ok();
            return BadRequest();
        }
    }
}
