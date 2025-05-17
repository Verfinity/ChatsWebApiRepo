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
        public async Task<ActionResult<List<Chat>>> GetChatsByUserIdAsnync([FromQuery] int userId)
        {
            List<Chat> chats = await _chatsRepo.GetChatsByUserIdAsync(userId);
            if (chats.IsNullOrEmpty())
                return NoContent();
            return Ok(chats);
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
