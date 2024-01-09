using Mamba.DAL;
using Mamba.Models;
using Mamba.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<Product> products = await _context.Products.Include(x => x.Category).ToListAsync();
            ICollection<Service> services = await _context.Services.ToListAsync();
            ICollection<Team> teams = await _context.Teams.Include(x => x.Position).ToListAsync();

            HomeVM vm = new HomeVM
            {
                Products = products,
                Services = services,
                Teams = teams
            };
            return View(vm);
        }
    }
}
