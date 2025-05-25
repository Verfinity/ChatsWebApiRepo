using ChatsWebApi.Components.Repositories.ChatsToUsers;
using ClassLibrary;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace ChatsWebApi.Components.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [Route("add-current-user-to-chat/{chatId}")]
        public async Task<ActionResult<ChatsUsers>> AddCurrentUserToChatAsync([FromRoute] int chatId)
        {
            var chatsUsers = new ChatsUsers
            {
                UserId = int.Parse(HttpContext.User.Identity.Name),
                ChatId = chatId
            };
            await _chatsUsersValidator.ValidateAndThrowAsync(chatsUsers);

            var createdChatsUsers = await _chatsUsersRepository.AddUserToChatAsync(chatsUsers);
            if (createdChatsUsers != null)
                return Ok(chatsUsers);
            return BadRequest("This relation already exists!");
        }

        [HttpPost]
        [Route("remove-current-user-from-chat/{chatId}")]
        public async Task<ActionResult> RemoveCurrentUserFromChatAsync([FromRoute] int chatId)
        {
            var chatsUsers = new ChatsUsers
            {
                UserId = int.Parse(HttpContext.User.Identity.Name),
                ChatId = chatId
            };
            await _chatsUsersValidator.ValidateAndThrowAsync(chatsUsers);

            if (await _chatsUsersRepository.RemoveUserFromChatAsync(chatsUsers))
                return Ok();
            return BadRequest("Can't remove non-existent relation!");
        }
    }
}
