using Microsoft.AspNetCore.Mvc;
using Web.Models.Repositories.Posts;

namespace Web.Controllers;

public class BlogController : Controller
{
    private readonly IPostRepository _postRepository;

    public BlogController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpGet("blog/{name}")]
    public async Task<IActionResult> IndexAsync(string name)
    {
        var post = await _postRepository.GetByIdAsync(name);

        if(post is null)
            return NotFound();
        
        return View(post);
    }
}