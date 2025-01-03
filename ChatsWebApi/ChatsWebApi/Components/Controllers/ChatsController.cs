using ChatsWebApi.Components.Repositories.Chats;
using ChatsWebApi.Components.Types.Database;
using Microsoft.AspNetCore.Mvc;

namespace ChatsWebApi.Components.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController : ControllerBase
    {
        private readonly IChatsRepository _chatsRepo;

        public ChatsController(IChatsRepository chatsRepo)
        {
            _chatsRepo = chatsRepo;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<List<Chat>>> GetChatsByUserIdAsync([FromRoute] int userId)
        {
            List<Chat> chats = await _chatsRepo.GetChatsByUserIdAsync(userId);
            if (chats != null)
                return Ok(chats);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateNewChatAsync([FromBody] Chat newChat)
        {
            int? newChatId = await _chatsRepo.CreateAsync(newChat);
            if (newChatId != null)
                return Ok(newChatId);
            return BadRequest("Chat between this users already exists!");
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteChatAsync([FromRoute] int id)
        {
            if (await _chatsRepo.DeleteAsync(id))
                return Ok();
            return BadRequest();
        }
    }
}
