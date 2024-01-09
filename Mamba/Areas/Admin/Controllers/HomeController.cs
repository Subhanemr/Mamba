using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mamba.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public IActionResult Index()
        {
            return View();
        }
    }
}
