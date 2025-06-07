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
        [Route("current-user")]
        public async Task<ActionResult<ChatsUsers>> AddCurrentUserToChatAsync([FromQuery] int chatId)
        {
            var chatsUsers = new ChatsUsers
            {
                UserId = int.Parse(HttpContext.User.Identity.Name),
                ChatId = chatId
            };
            await _chatsUsersValidator.ValidateAndThrowAsync(chatsUsers);

            var createdChatsUsers = await _chatsUsersRepository.AddAsync(chatsUsers);
            if (createdChatsUsers != null)
                return Ok(chatsUsers);
            return BadRequest("This relation already exists!");
        }

        [HttpDelete]
        [Route("current-user")]
        public async Task<ActionResult> RemoveCurrentUserFromChatAsync([FromQuery] int chatId)
        {
            var chatsUsers = new ChatsUsers
            {
                UserId = int.Parse(HttpContext.User.Identity.Name),
                ChatId = chatId
            };
            await _chatsUsersValidator.ValidateAndThrowAsync(chatsUsers);

            if (await _chatsUsersRepository.RemoveAsync(chatsUsers))
                return Ok();
            return BadRequest("Can't remove non-existent relation!");
        }
    }
}
