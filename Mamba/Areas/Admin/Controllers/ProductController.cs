using Mamba.Areas.Admin.ViewModels;
using Mamba.DAL;
using Mamba.Models;
using Mamba.Utilities.Extentions;
using Mamba.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Mamba.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index(int page=1)
        {
            if (page <= 0) return BadRequest();
            double count = await _context.Products.CountAsync();
            ICollection<Product> items = await _context.Products.Skip((page-1)*4).Take(4)
                .Include(x => x.Category).ToListAsync();
            PaginationVM<Product> vM = new PaginationVM<Product>
            {
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / 4),
                Items = items
            };
            return View(vM);
        }
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create()
        {
            CreateProductVM create = new CreateProductVM
            {
                Categories = await _context.Categories.ToListAsync()
            };
            return View(create);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM create)
        {
            if (!ModelState.IsValid)
            {
                create.Categories = await _context.Categories.ToListAsync();
                return View(create);
            }
            bool result = await _context.Products.AnyAsync(x => x.Name.Trim().ToLower() == create.Name.Trim().ToLower());
            if (result)
            {
                create.Categories = await _context.Categories.ToListAsync();
                ModelState.AddModelError("Name", "Is exists");
                return View(create);
            }
            bool categoryResult = await _context.Categories.AnyAsync(x => x.Id == create.CategoryId);
            if (!categoryResult)
            {
                create.Categories = await _context.Categories.ToListAsync();
                ModelState.AddModelError("CategoryId", "Is not exists");
                return View(create);
            }
            if (!create.Photo.IsValid())
            {
                create.Categories = await _context.Categories.ToListAsync();
                ModelState.AddModelError("Photo", "Not Valid");
                return View(create);
            }
            if (!create.Photo.LimitSize())
            {
                create.Categories = await _context.Categories.ToListAsync();
                ModelState.AddModelError("Photo", "Limit size is 10MB");
                return View(create);
            }
            Product item = new Product
            {
                Name = create.Name,
                CategoryId = create.CategoryId,
                Img = await create.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img"),
                Description = create.Description,
                Price = create.Price
            };

            await _context.Products.AddAsync(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Product item = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            UpdateProductVM update = new UpdateProductVM
            {
                Name = item.Name,
                Img = item.Img,
                CategoryId = item.CategoryId,
                Description = item.Description,
                Price = item.Price,
                Categories = await _context.Categories.ToListAsync()
            };
            return View(update);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateProductVM update)
        {
            if (!ModelState.IsValid) return View(update);

            Product existed = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();

            bool result = await _context.Products.AnyAsync(x => x.Name.Trim().ToLower() == update.Name.Trim().ToLower() && x.Id != id);
            if (result)
            {
                update.Categories = await _context.Categories.ToListAsync();
                ModelState.AddModelError("Name", "Is exists");
                return View(update);
            }
            bool categoryResult = await _context.Categories.AnyAsync(x => x.Id == update.CategoryId);
            if (!categoryResult)
            {
                update.Categories = await _context.Categories.ToListAsync();
                ModelState.AddModelError("CategoryId", "Is not exists");
                return View(update);
            }
            if (update.Photo is not null)
            {
                if (!update.Photo.IsValid())
                {
                    update.Categories = await _context.Categories.ToListAsync();
                    ModelState.AddModelError("Photo", "Not Valid");
                    return View(update);
                }
                if (!update.Photo.LimitSize())
                {
                    update.Categories = await _context.Categories.ToListAsync();
                    ModelState.AddModelError("Photo", "Limit size is 10MB");
                    return View(update);
                }
                existed.Img.DeleteFileAsync(_env.WebRootPath, "assets", "img");
                existed.Img = await update.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
            }
            existed.Name = update.Name;
            existed.Price = update.Price;
            existed.Description = update.Description;
            existed.CategoryId = update.CategoryId;


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Product item = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();

            _context.Products.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
