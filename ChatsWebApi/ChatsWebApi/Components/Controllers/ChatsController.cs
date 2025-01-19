using ChatsWebApi.Components.Repositories.Chats;
using ChatsWebApi.Components.Types.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<ActionResult<int>> CreateNewChatAsync([FromBody] Chat newChat)
        {
            Chat? chat = await _chatsRepo.CreateAsync(newChat);
            if (chat != null)
                return Ok(chat);
            return BadRequest("Chat between this users already exists!");
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
