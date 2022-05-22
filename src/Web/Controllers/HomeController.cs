using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Models.Data.Posts;
using Web.Models.Repositories.Posts;
using Web.Models.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostRepository _postRepository;

        public HomeController(IPostRepository postRepository, ILogger<HomeController> logger)
        {
            _logger = logger;
            _postRepository = postRepository;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _postRepository.GetAsync();
            return View(new IndexViewModel(posts.Select(x => new PostSummary(x.Name, x.Title, x.SummaryContent, x.Tags, x.CreatedAt, x.UpdatedAt)).OrderByDescending(x => x.CreatedAt)));
        }

        public IActionResult StatusCode(int? id)
        {
            return View(new StatusCodeViewModel(id));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}