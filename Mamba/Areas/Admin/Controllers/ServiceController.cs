﻿using Mamba.Areas.Admin.ViewModels;
using Mamba.DAL;
using Mamba.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            ICollection<Service> items = await _context.Services.ToListAsync();
            return View(items);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceVM create)
        {
            if (!ModelState.IsValid) return View(create);
            bool result = await _context.Services.AnyAsync(x => x.Title.Trim().ToLower() == create.Title.Trim().ToLower());
            if (result)
            {
                ModelState.AddModelError("Name", "Is exists");
                return View(create);
            }
            Service item = new Service
            {
                Title = create.Title,
                SubTitle = create.SubTitle,
                Icon = create.Icon
            };

            await _context.Services.AddAsync(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Service item = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            UpdateServiceVM update = new UpdateServiceVM
            {
                Title = item.Title,
                SubTitle = item.SubTitle,
                Icon = item.Icon
            };
            return View(update);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateServiceVM update)
        {
            if (!ModelState.IsValid) return View(update);

            Service existed = await _context.Services.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();

            bool result = await _context.Services.AnyAsync(x => x.Title.Trim().ToLower() == update.Title.Trim().ToLower() && x.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Is exists");
                return View(update);
            }

            existed.Title = update.Title;
            existed.SubTitle = update.SubTitle;
            existed.Icon = update.Icon;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Service item = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();

            _context.Services.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
