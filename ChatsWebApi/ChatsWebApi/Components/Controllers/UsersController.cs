using ChatsWebApi.Components.Repositories;
using ChatsWebApi.Components.Types;
using Microsoft.AspNetCore.Mvc;

namespace ChatsWebApi.Components.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IRepository<User> _usersRepo;

        public UsersController(IRepository<User> usersRepo)
        {
            _usersRepo = usersRepo;
        }

        [HttpGet]
        public async Task<List<User>> GetUsersAsync()
        {
            return await _usersRepo.GetAllAsync();
        }

        [HttpPost]
        public async Task CreateUser([FromBody] User user)
        {
            await _usersRepo.CreateAsync(user);
        }
    }
}
