using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class AboutMeController : Controller
{
    public IActionResult Index()
        => View();
}