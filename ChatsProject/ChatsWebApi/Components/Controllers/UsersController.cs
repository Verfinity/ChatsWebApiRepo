using ChatsWebApi.Components.Repositories.Users;
using ClassLibrary;
using FluentValidation;
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
        [Route("{userId}")]
        public async Task<ActionResult<User?>> GetUserById([FromRoute] int userId)
        {
            User? user = await _usersRepo.GetByIdAsync(userId);
            if (user == null)
                return NoContent();
            return Ok(user);
        }

        [HttpGet]
        [Route("current-user")]
        public async Task<ActionResult<User?>> GetCurrentUserAsync()
        {
            int id = int.Parse(HttpContext.User.Identity.Name);
            User? user = await _usersRepo.GetByIdAsync(id);
            if (user == null)
                return NoContent();
            await _usersRepo.SetCollectionAsync(user, u => u.Chats);
            return Ok(user);
        }

        [HttpDelete]
        [Route("current-user")]
        public async Task<ActionResult<User?>> RemoveCurrentUserAsync()
        {
            int id = int.Parse(HttpContext.User.Identity.Name);
            if (await _usersRepo.DeleteAsync(id))
                return Ok();
            return BadRequest("User with this id doesn't exists!");
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUserAsync([FromRoute] int id)
        {
            if (await _usersRepo.DeleteAsync(id))
                return Ok();
            return BadRequest("User with this id doesn't exists!");
        }
    }
}
