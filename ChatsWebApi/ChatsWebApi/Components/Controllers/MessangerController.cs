using ChatsWebApi.Components.Repositories;
using ChatsWebApi.Components.Types;
using Microsoft.AspNetCore.Mvc;

namespace ChatsWebApi.Components.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessangerController : ControllerBase
    {
        private readonly IChatRepository _chatsRepo;

        public MessangerController(IChatRepository chatsRepo)
        {
            _chatsRepo = chatsRepo;
        }

        [HttpGet]
        [Route("all-chats/{userId}")]
        public async Task<ActionResult<List<Chat>>> GetChatsByUserIdAsync([FromRoute] int userId)
        {
            List<Chat> chats = await _chatsRepo.GetChatsByUserIdAsync(userId);
            if (chats != null)
                return Ok(chats);
            return NoContent();
        }
    }
}
