using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarkMyDoctor.Data;
using MarkMyDoctor.Models.Entities;
using MarkMyDoctor.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace MarkMyDoctor.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly UserManager<User> userManager;

        private IDoctorService DoctorService { get; }

        public ReviewsController(IDoctorService doctorService, UserManager<User> userManager)
        {
            DoctorService = doctorService;
            this.userManager = userManager;
        }


        // GET: Reviews/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (await DoctorService.GetDoctorByIdAsync(id) == null)
            {
                return NotFound();
            }

            var doctorReviewViewModel = new DoctorReviewViewModel() { Doctor = await DoctorService.GetDoctorByIdAsync(id) };

            return View(doctorReviewViewModel);
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, DoctorReviewViewModel doctorReviewViewModel)
        {
            var review = doctorReviewViewModel.Review;

            review.Doctor = await DoctorService.GetDoctorByIdAsync(id);

            if (review.Doctor == null)
            {
                return NotFound();
            }

            review.ReviewedOn = DateTime.Today;


            review.User = await userManager.GetUserAsync(User);

            DoctorService.CreateReview(review);

            var reviewScore = review.CommunicationRating +
                              review.EmpathyRating +
                              review.FlexibilityRating +
                              review.HumanityRating +
                              review.ProfessionalismRating;

            await DoctorService.CalculateDoctorOverall(id, reviewScore);

            await DoctorService.SaveChangesAsync();

            return RedirectToAction("Details", "Doctors", new { id = id });
        }

        //// GET: Reviews/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var review = await _context.Reviews.FindAsync(id);
        //    if (review == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", review.DoctorId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", review.UserId);
        //    return View(review);
        //}

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReviewBody,Recommend,ReviewedOn,IsReported,ProfessionalismRating,HumanityRating,CommunicationRating,EmpathyRating,FelxibilityRating,DoctorId,UserId")] Review review)
        //{
        //    if (id != review.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(review);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ReviewExists(review.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", review.DoctorId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", review.UserId);
        //    return View(review);
        //}

        // GET: Reviews/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var review = await _context.Reviews
        //        .Include(r => r.Doctor)
        //        .Include(r => r.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (review == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(review);
        //}

        // POST: Reviews/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var review = await _context.Reviews.FindAsync(id);
        //    _context.Reviews.Remove(review);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ReviewExists(int id)
        //{
        //    return _context.Reviews.Any(e => e.Id == id);
        //}
    }
}
