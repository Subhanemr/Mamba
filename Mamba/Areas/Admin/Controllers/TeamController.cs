using Mamba.Areas.Admin.ViewModels;
using Mamba.DAL;
using Mamba.Models;
using Mamba.Utilities.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            ICollection<Team> items = await _context.Teams.Include(x => x.Position).ToListAsync();
            return View(items);
        }
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create()
        {
            CreateTeamVM item = new CreateTeamVM
            {
                Positions = await _context.Positions.ToListAsync()
            };
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamVM create)
        {
            if (!ModelState.IsValid)
            {
                create.Positions = await _context.Positions.ToListAsync();
                return View(create);
            }
            bool result = await _context.Categories.AnyAsync(x => x.Name.Trim().ToLower() == create.Name.Trim().ToLower());
            if (result)
            {
                ModelState.AddModelError("Name", "Is exists");
                return View(create);
            }
            bool positonResult = await _context.Positions.AnyAsync(x => x.Id == create.PositionId);
            if (!positonResult)
            {
                create.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("PositionId", "Is not exists");
                return View(create);
            }
            if (!create.Photo.IsValid())
            {
                create.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("PositionId", "Not Valid");
                return View(create);
            }
            if (!create.Photo.LimitSize())
            {
                create.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("PositionId", "Limit size is 10MB");
                return View(create);
            }
            Team item = new Team
            {
                Name = create.Name,
                PositionId = create.PositionId,
                InstaLink = create.InstaLink,
                FaceLink = create.FaceLink,
                TwitLink = create.TwitLink,
                LinkedLink = create.LinkedLink,
                Img = await create.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img")

            };

            await _context.Teams.AddAsync(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Team item = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            UpdateTeamVM update = new UpdateTeamVM
            { 
                Name = item.Name,
                Img = item.Img,
                Positions = await _context.Positions.ToListAsync(),
                InstaLink = item.InstaLink,
                FaceLink = item.FaceLink,
                TwitLink = item.TwitLink,
                LinkedLink = item.LinkedLink,
                PositionId = item.PositionId
            };
            return View(update);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateTeamVM update)
        {
            if (!ModelState.IsValid) return View(update);

            Team existed = await _context.Teams.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();

            bool result = await _context.Teams.AnyAsync(x => x.Name.Trim().ToLower() == update.Name.Trim().ToLower() && x.Id != id);
            if (result)
            {
                update.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Name", "Is exists");
                return View(update);
            }
            bool positonResult = await _context.Positions.AnyAsync(x => x.Id == update.PositionId);
            if (!positonResult)
            {
                update.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("PositionId", "Is not exists");
                return View(update);
            }
            if(update.Photo is not null)
            {
                if (!update.Photo.IsValid())
                {
                    update.Positions = await _context.Positions.ToListAsync();
                    ModelState.AddModelError("Photo", "Not Valid");
                    return View(update);
                }
                if (!update.Photo.LimitSize())
                {
                    update.Positions = await _context.Positions.ToListAsync();
                    ModelState.AddModelError("Photo", "Limit size is 10MB");
                    return View(update);
                }
                existed.Img.DeleteFileAsync(_env.WebRootPath, "assets", "img");
                existed.Img = await update.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
            }
            existed.Name = update.Name;
            existed.InstaLink = update.InstaLink;
            existed.FaceLink = update.FaceLink;
            existed.LinkedLink = update.LinkedLink;
            existed.TwitLink = update.TwitLink;
            existed.PositionId = update.PositionId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Team item = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();

            _context.Teams.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
