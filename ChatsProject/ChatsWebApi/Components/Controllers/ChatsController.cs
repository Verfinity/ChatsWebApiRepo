using ChatsWebApi.Components.Repositories.Chats;
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
    public class ChatsController : ControllerBase
    {
        private readonly IChatsRepository _chatsRepo;
        private readonly IValidator<Chat> _chatValidator;

        public ChatsController(IChatsRepository chatsRepo, IValidator<Chat> chatValidator)
        {
            _chatsRepo = chatsRepo;
            _chatValidator = chatValidator;
        }

        [HttpGet]
        [Route("current-user")]
        public async Task<ActionResult<List<Chat>>> GetCurrentUserChatsAsync()
        {
            int userId = int.Parse(HttpContext.User.Identity.Name);
            List<Chat> chats = await _chatsRepo.GetChatsByUserIdAsync(userId);
            if (chats.IsNullOrEmpty())
                return NoContent();
            return Ok(chats);
        }

        [HttpGet]
        [Route("{chatName}")]
        public async Task<ActionResult<Chat>> GetChatByNameAsync([FromRoute] string chatName)
        {
            var chat = await _chatsRepo.GetByExpressionAsync(c => c.Name == chatName);
            if (chat == null)
                return NoContent();
            return Ok(chat);
        }

        [HttpPost]
        public async Task<ActionResult<Chat>> CreateNewChatAsync([FromBody] Chat newChat)
        {
            await _chatValidator.ValidateAndThrowAsync(newChat);

            Chat? chat = await _chatsRepo.CreateAsync(newChat);
            if (chat != null)
                return Ok(chat);
            return BadRequest("Can't create chat");
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteChatAsync([FromRoute] int id)
        {
            if (await _chatsRepo.DeleteAsync(id))
                return Ok();
            return BadRequest();
        }
    }
}
