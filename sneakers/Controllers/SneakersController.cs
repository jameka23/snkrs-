using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sneakers.Data;
using sneakers.Models;
using sneakers.Models.SneakersViewModels;

namespace sneakers.Controllers
{
    public class SneakersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment hostingEnvironment;

        public SneakersController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Sneakers
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var viewModel = new SneakersCreateViewModel
            {
                AvailableBrands = await _context.Brand.ToListAsync(),
                AvailableConditions = await _context.Condition.ToListAsync(),
                AvailableSizes = await _context.Size.ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Sneakers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SneakersCreateViewModel viewModel)
        {
            ModelState.Remove("Sneaker.Brand");
            ModelState.Remove("Sneaker.Condition");
            ModelState.Remove("Sneaker.Size");
            ModelState.Remove("Sneaker.User");
            ModelState.Remove("Sneaker.UserId");


            // If the Photo property on the incoming model object is not null, then the user
            // has selected an image to upload.
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // If the Photo property on the incoming model object is not null, then the user
                // has selected an image to upload.
                if (viewModel.Photo != null)
                {
                    // The image must be uploaded to the images folder in wwwroot
                    // To get the path of the wwwroot folder we are using the inject
                    // HostingEnvironment service provided by ASP.NET Core
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    // To make sure the file name is unique we are appending a new
                    // GUID value and and an underscore to the file name
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    // Use CopyTo() method provided by IFormFile interface to
                    // copy the file to wwwroot/images folder
                    viewModel.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                var sneaker = viewModel.Sneaker;
                var currUser = await GetCurrentUserAsync();
                sneaker.ImgPath = uniqueFileName;
                sneaker.UserId = currUser.Id;
                _context.Add(sneaker);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            viewModel.AvailableBrands = await _context.Brand.ToListAsync();
            viewModel.AvailableConditions = await _context.Condition.ToListAsync();
            viewModel.AvailableSizes = await _context.Size.ToListAsync();

            return View(viewModel);
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

        // MY Sneakers, gets all the current user's sneakers 
        public async Task<IActionResult> MySneakers()
        {
            // grab the current user
            var currentUser = await GetCurrentUserAsync();
            //var products = await _context.Product.Include(p => p.ProductType)
            //    .Include(p => p.User)
            //    .Where(p => p.User == currentUser)
            //    .OrderByDescending(p => p.DateCreated).ToListAsync();\
            var sneakers = await _context.Sneaker.Include(b => b.Brand)
                .Include(c => c.Condition)
                .Include(s => s.Size)
                .Include(u => u.User)
                .Where(u => u.User == currentUser).ToListAsync();


            return View(sneakers);
        }

        private bool SneakerExists(int id)
        {
            return _context.Sneaker.Any(e => e.SneakerId == id);
        }
    }
}
