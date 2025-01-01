﻿using ChatsWebApi.Components.Repositories.Posts;
using ChatsWebApi.Components.Types;
using Microsoft.AspNetCore.Mvc;

namespace ChatsWebApi.Components.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostsRepository _postsRepo;

        public PostsController(IPostsRepository postsRepo)
        {
            _postsRepo = postsRepo;
        }

        [HttpGet]
        [Route("{chatId}")]
        public async Task<ActionResult<List<Post>>> GetPostsByChatIdAsync([FromRoute] int chatId)
        {
            List<Post> posts = await _postsRepo.GetPostsByChatIdAsync(chatId);
            if (posts != null)
                return Ok(posts);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewPostAsync([FromBody] Post newPost)
        {
            int? newPostId = await _postsRepo.CreateAsync(newPost);
            if (newPostId != null)
                return Ok(newPostId);
            return BadRequest("This user isn't in this chat!");
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeletePostAsync([FromRoute] int id)
        {
            if (await _postsRepo.DeleteAsync(id))
                return Ok();
            return BadRequest();
        }
    }
}
