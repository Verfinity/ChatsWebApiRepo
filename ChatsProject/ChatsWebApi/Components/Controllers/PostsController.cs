using ChatsWebApi.Components.Repositories.Posts;
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
    public class PostsController : ControllerBase
    {
        private readonly IPostsRepository _postsRepo;
        private readonly IValidator<Post> _postValidator;

        public PostsController(IPostsRepository postsRepo, IValidator<Post> postValidator)
        {
            _postsRepo = postsRepo;
            _postValidator = postValidator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetPostsByChatIdAsync([FromQuery] int chatId)
        {
            List<Post> posts = await _postsRepo.GetPostsByChatIdAsync(chatId);
            if (posts.IsNullOrEmpty())
                return NoContent();
            return Ok(posts);
        }

        [HttpPost]
        public async Task<ActionResult<Post>> CreateNewPostAsync([FromBody] ChatMessage chatMessage)
        {
            var newPost = new Post
            {
                Content = chatMessage.Content,
                ChatId = chatMessage.ChatId,
                UserId = int.Parse(HttpContext.User.Identity.Name)
            };
            await _postValidator.ValidateAndThrowAsync(newPost);

            Post? post = await _postsRepo.CreateAsync(newPost);
            if (post != null)
                return Ok(newPost);
            return BadRequest("This user isn't in this chat!");
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeletePostAsync([FromRoute] int id)
        {
            var postToDelete = await _postsRepo.GetByIdAsync(id);
            int userId = int.Parse(HttpContext.User.Identity.Name);
            if (postToDelete.UserId != userId)
                return BadRequest("Can't delete someone else's post!");

            if (await _postsRepo.DeleteAsync(id))
                return Ok();
            return BadRequest();
        }
    }
}
