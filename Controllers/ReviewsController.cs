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
using Microsoft.AspNetCore.Authorization;

namespace MarkMyDoctor.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IUnitOfWork unitOfWork;

        public ReviewsController(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }


        // GET: Reviews/Create
        [Authorize]
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorReviewViewModel = new DoctorReviewViewModel() 
                                                { 
                                                    Doctor = await unitOfWork.DoctorRepository.GetByIdAsync(id.Value) 
                                                };

            if (doctorReviewViewModel.Doctor == null)
            {
               return RedirectToAction("NoResult", "Home"); //!!!!!!!!!!!!Kicserélni!!!!!!!!!!!!!!
            }

            return View(doctorReviewViewModel);
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorReviewViewModel doctorReviewViewModel)
        {
            var user = await userManager.GetUserAsync(User);

            var isCreated = await unitOfWork.ReviewRepository.Create(doctorReviewViewModel, user);


            if (isCreated)
            {
                return RedirectToAction("Details", "Doctors", new { id = doctorReviewViewModel.Doctor.Id });
            }
            else
            {
                return RedirectToAction("NoResult", "Home"); //!!!!!!!!!!Kicsierélni!!!!!!!!!!!!
            }
            
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
