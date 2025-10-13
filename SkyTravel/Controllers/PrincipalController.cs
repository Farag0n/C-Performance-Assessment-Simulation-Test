using Microsoft.AspNetCore.Mvc;

namespace SkyTravel.Controllers;

public class PrincipalController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}