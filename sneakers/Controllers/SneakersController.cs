using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sneakers.Data;
using sneakers.Models;

namespace sneakers.Controllers
{
    public class SneakersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SneakersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sneakers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sneaker.Include(s => s.Brand).Include(s => s.Condition).Include(s => s.Size).Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sneakers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sneaker = await _context.Sneaker
                .Include(s => s.Brand)
                .Include(s => s.Condition)
                .Include(s => s.Size)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.SneakerId == id);
            if (sneaker == null)
            {
                return NotFound();
            }

            return View(sneaker);
        }

        // GET: Sneakers/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brand, "BrandId", "BrandType");
            ViewData["ConditionId"] = new SelectList(_context.Condition, "ConditionId", "ConditionType");
            ViewData["SizeId"] = new SelectList(_context.Size, "SizeId", "ShoeSize");
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: Sneakers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SneakerId,SizeId,Title,Description,BrandId,IsSold,ConditionId,Price,ImgPath,UserId")] Sneaker sneaker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sneaker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brand, "BrandId", "BrandType", sneaker.BrandId);
            ViewData["ConditionId"] = new SelectList(_context.Condition, "ConditionId", "ConditionType", sneaker.ConditionId);
            ViewData["SizeId"] = new SelectList(_context.Size, "SizeId", "ShoeSize", sneaker.SizeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", sneaker.UserId);
            return View(sneaker);
        }

        // GET: Sneakers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sneaker = await _context.Sneaker.FindAsync(id);
            if (sneaker == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brand, "BrandId", "BrandType", sneaker.BrandId);
            ViewData["ConditionId"] = new SelectList(_context.Condition, "ConditionId", "ConditionType", sneaker.ConditionId);
            ViewData["SizeId"] = new SelectList(_context.Size, "SizeId", "ShoeSize", sneaker.SizeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", sneaker.UserId);
            return View(sneaker);
        }

        // POST: Sneakers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SneakerId,SizeId,Title,Description,BrandId,IsSold,ConditionId,Price,ImgPath,UserId")] Sneaker sneaker)
        {
            if (id != sneaker.SneakerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sneaker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SneakerExists(sneaker.SneakerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brand, "BrandId", "BrandType", sneaker.BrandId);
            ViewData["ConditionId"] = new SelectList(_context.Condition, "ConditionId", "ConditionType", sneaker.ConditionId);
            ViewData["SizeId"] = new SelectList(_context.Size, "SizeId", "ShoeSize", sneaker.SizeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", sneaker.UserId);
            return View(sneaker);
        }

        // GET: Sneakers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sneaker = await _context.Sneaker
                .Include(s => s.Brand)
                .Include(s => s.Condition)
                .Include(s => s.Size)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.SneakerId == id);
            if (sneaker == null)
            {
                return NotFound();
            }

            return View(sneaker);
        }

        // POST: Sneakers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sneaker = await _context.Sneaker.FindAsync(id);
            _context.Sneaker.Remove(sneaker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SneakerExists(int id)
        {
            return _context.Sneaker.Any(e => e.SneakerId == id);
        }
    }
}
