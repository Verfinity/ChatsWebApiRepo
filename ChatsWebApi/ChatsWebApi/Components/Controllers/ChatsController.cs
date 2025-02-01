using ChatsWebApi.Components.Repositories.Chats;
using ChatsWebApi.Components.Types.Database;
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

        public ChatsController(IChatsRepository chatsRepo)
        {
            _chatsRepo = chatsRepo;
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
