using ChatsWebApi.Components.Repositories.ChatsToUsers;
using ClassLibrary;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatsWebApi.Components.Controllers
{
    [ApiController]
    [Route("api/chats-users")]
    [Authorize]
    public class ChatsUsersController : ControllerBase
    {
        private readonly IChatsUsersRepository _chatsUsersRepository;
        private readonly IValidator<ChatsUsers> _chatsUsersValidator;

        public ChatsUsersController(IChatsUsersRepository chatsUsersRepository, IValidator<ChatsUsers> chatsUsersValidator)
        {
            _chatsUsersRepository = chatsUsersRepository;
            _chatsUsersValidator = chatsUsersValidator;
        }

        [HttpPost]
        public async Task<ActionResult<ChatsUsers>> AddCurrentUserToChatAsync([FromBody] ChatsUsers chatsUsers)
        {
            await _chatsUsersValidator.ValidateAndThrowAsync(chatsUsers);

            var createdChatsUsers = await _chatsUsersRepository.AddAsync(chatsUsers);
            if (createdChatsUsers != null)
                return Ok(chatsUsers);
            return BadRequest("This relation already exists!");
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveCurrentUserFromChatAsync([FromBody] ChatsUsers chatsUsers)
        {
            await _chatsUsersValidator.ValidateAndThrowAsync(chatsUsers);

            if (await _chatsUsersRepository.RemoveAsync(chatsUsers))
                return Ok();
            return BadRequest("Can't remove non-existent relation!");
        }
    }
}
