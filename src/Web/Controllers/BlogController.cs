using Microsoft.AspNetCore.Mvc;
using Web.Models.Repositories.Blogs;

namespace Web.Controllers;

public class BlogController : Controller
{
    private readonly IBlogRepository _blogRepository;

    public BlogController(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    [HttpGet("blog/{name}")]
    public async Task<IActionResult> IndexAsync(string name)
    {
        var blog = await _blogRepository.GetByIdAsync(name);

        if(blog is null)
            return NotFound();
        
        return View(await _blogRepository.GetByIdAsync(name));
    }
}