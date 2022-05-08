using Microsoft.AspNetCore.Mvc;
using Web.Models.Services.Blogs;

namespace Web.Controllers;

public class BlogController : Controller
{
    private readonly IBlogRepository _blogRepository;

    public BlogController(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    [HttpGet("blog/{id}")]
    public async Task<IActionResult> IndexAsync(int id)
    {
        var blog = await _blogRepository.GetByIdAsync(id);

        if(blog is null)
            return NotFound();
        
        return View(await _blogRepository.GetByIdAsync(id));
    }
}