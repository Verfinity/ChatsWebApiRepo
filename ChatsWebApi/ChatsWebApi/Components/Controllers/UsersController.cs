using ChatsWebApi.Components.Repositories.Users;
using ChatsWebApi.Components.Types.Database;
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
