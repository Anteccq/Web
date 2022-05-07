﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Models.Data.Blogs;
using Web.Models.Services.Blogs;
using Web.Models.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogRepository _blogRepository;

        public HomeController(IBlogRepository blogRepository, ILogger<HomeController> logger)
        {
            _logger = logger;
            _blogRepository = blogRepository;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _blogRepository.GetAsync();
            return View(new IndexViewModel(blogs.Select(x => new BlogSummary(x.Id, x.Title, x.SummaryContent, x.Tags, x.CreatedAt, x.UpdatedAt))));
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