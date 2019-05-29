using Microsoft.AspNetCore.Mvc;

namespace FeedMe.Controllers
{
    public class CartController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}