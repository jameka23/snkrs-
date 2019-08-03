using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sneakers.Data;
using sneakers.Models;
using sneakers.Models.ReviewsViewModels;

namespace sneakers.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Review.Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public async Task<IActionResult> Create(string userId)
        {
            var user2BeReviewed = await _context.Users.FindAsync(userId);
            var currUser = await GetCurrentUserAsync();
            ViewBag.currUser = currUser.FirstName;
            // reframe from letting user create a review for themselves!
            if (currUser == user2BeReviewed)
            {
                return RedirectToAction("Index", "Sneakers");
            }
            

            ReviewsCreateViewModel viewModel = new ReviewsCreateViewModel
            {
                User = user2BeReviewed
            };

            return View(viewModel);
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewsCreateViewModel viewModel)
        {
            ModelState.Remove("Review.User");
            ModelState.Remove("Review.UserId");
            ModelState.Remove("User.FirstName");
            ModelState.Remove("User.LastName");
            ModelState.Remove("Review.ReviewId");

            var reviewedUser = _context.Users.Find(viewModel.User.Id);
            viewModel.User = reviewedUser;

            if (ModelState.IsValid)
            {
                var review = viewModel.Review;
                review.User = viewModel.User;
                review.User.Rating = await CalculateRating(viewModel.User.Id);
                _context.Add(review);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("UserProfile", "Sneakers", new { userId4Profile = viewModel.User.Id });

        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", review.UserId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewId,Rating,UserId")] Review review)
        {
            if (id != review.ReviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewId))
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
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", review.UserId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Review.FindAsync(id);
            _context.Review.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
            foreach(var item in user.Reviews)
            {
                count++;
                total += item.Rating;
            }

            // get the average and return 
            rating = total / count;

            return rating;
        }
        private bool ReviewExists(int id)
        {
            return _context.Review.Any(e => e.ReviewId == id);
        }
    }
}
