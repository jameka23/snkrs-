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
        public async Task<IActionResult> Index(string searchString, string SearchBar)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["SearchBar"] = SearchBar;

            var currentUser = GetCurrentUserAsync().Result;
            IQueryable<Sneaker> sneakers = _context.Sneaker.Include(s => s.Brand).Include(s => s.Condition).Include(s => s.Size).Include(s => s.User);

            if (!String.IsNullOrEmpty(searchString))
            {
                switch (SearchBar)
                {
                    case "1":
                        
                        sneakers = sneakers.Where(s => s.Brand.BrandType.ToUpper().Contains(searchString.ToUpper()));
                        break;
                    case "2":
                        sneakers = sneakers.Where(s => s.Size.ShoeSize.ToUpper().Contains(searchString.ToUpper()));
                        break;
                    case "3":
                        sneakers = sneakers.Where(s => s.Condition.ConditionType.ToUpper().Contains(searchString.ToUpper()));
                        break;
                    default:
                        //products.Where(p => p.Title.ToUpper().Contains(searchString.ToUpper())
                        //               || p.City.ToUpper().Contains(searchString.ToUpper()))
                        sneakers = sneakers.Where(s => s.Condition.ConditionType.ToUpper().Contains(searchString.ToUpper())
                                            || s.Condition.ConditionType.ToUpper().Contains(searchString.ToUpper())
                                            || s.Size.ShoeSize.ToUpper().Contains(searchString.ToUpper()));
                        break;
                }

            }
            
            return View(await sneakers.ToListAsync());
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            /*
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            var viewModel = new BookEditViewModel
            {
                Book = book,
                AvailableAuthors = await _context.Author.ToListAsync()
            };
            return View(viewModel);
             */
            // if the id that was passed in if null, then an error is thrown
            if (id == null)
            {
                return NotFound();
            }

            var sneaker = await _context.Sneaker.FindAsync(id);
            if (sneaker == null)
            {// if the sneaker from the db that was passed in if null, then an error is thrown
                return NotFound();
            }
            var viewModel = new SneakersEditViewModel
            {
                Sneaker = sneaker,
                AvailableBrands = await _context.Brand.ToListAsync(),
                AvailableConditions = await _context.Condition.ToListAsync(),
                AvailableSizes = await _context.Size.ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Sneakers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SneakersEditViewModel viewModel)
        {
            var sneaker = viewModel.Sneaker;

            if (id != sneaker.SneakerId)
            {
                return NotFound();
            }

            ModelState.Remove("Sneaker.Brand");
            ModelState.Remove("Sneaker.Condition");
            ModelState.Remove("Sneaker.Size");
            ModelState.Remove("Sneaker.User");
            ModelState.Remove("Sneaker.UserId");

            string uniqueFileName = null;

            if (viewModel.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                viewModel.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await GetCurrentUserAsync();
                    sneaker.UserId = currentUser.Id;
                    sneaker.ImgPath = uniqueFileName;
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
                return RedirectToAction(nameof(MySneakers));
            }
            viewModel.AvailableBrands = await _context.Brand.ToListAsync();
            viewModel.AvailableConditions = await _context.Condition.ToListAsync();
            viewModel.AvailableSizes = await _context.Size.ToListAsync();
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
        [Authorize]
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
            
            // grab a list of sneakers
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

        public async Task<IActionResult> UserProfile(string userId4Profile)
        {
            var theUserProfile = await _context.Users.FindAsync(userId4Profile);


            UserProfileViewModel viewModel = new UserProfileViewModel
            {
                User = theUserProfile,
                AllSneakers = await _context.Sneaker.Include(u => u.User)
                    .Where(s => s.UserId == userId4Profile)
                    .ToListAsync(),
                AllReviews = await _context.Review.Include(r => r.User)
                    .Where(r => r.UserId == userId4Profile)
                    .ToListAsync()
            };

            viewModel.User.Rating = await CalculateRating(viewModel.User.Id);

            return View(viewModel);
        }

        private async Task<double> CalculateRating(string userId)
        {
            double total = 0.0;
            int count = 0;
            double rating = 0.0;

            // grab the user
            var user = await _context.Users
                .Include(u => u.Reviews)
                .Where(u => u.Id == userId)
                .FirstAsync();

            // this foreach will go through each review that is generated for 
            // a user and add all the ratings so then we can get an average
            foreach (var item in user.Reviews)
            {
                count++;
                total += item.Rating;
            }

            // get the average and return 
            rating = total / count;

            return rating;
        }
    }
}
