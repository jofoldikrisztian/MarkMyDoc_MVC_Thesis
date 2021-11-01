using MarkMyDoctor.Interfaces;
using MarkMyDoctor.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Controllers
{
    public class ReviewsController : Controller
    {
       // private readonly UserManager<User> userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(IUnitOfWork unitOfWork, ILogger<ReviewsController> logger)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }


        // GET: Reviews/Create
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorReviewViewModel = new DoctorReviewViewModel()
            {
                Doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(id.Value)
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

            try
            {
                await _unitOfWork.ReviewRepository.CreateAsync(doctorReviewViewModel);

                _unitOfWork.Commit();

                _logger.LogInformation("Értékelés hozzáadva: {0}", doctorReviewViewModel.Doctor.Id);

                return RedirectToAction("Details", "Doctors", new { id = doctorReviewViewModel.Doctor.Id });
            }
            catch (Exception ex)
            {

                _logger.LogError("Hiba történt az értékelés során. Hiba: {0}", ex.Message);
                _unitOfWork.Dispose();
                return RedirectToAction("SomethingWentWrong", "Home");
            }
            
        }


        // GET: UnApproved
        [Authorize(Roles="Administrator,Moderator")]
        public IActionResult UnApprovedReviews()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadUnApprovedReviews()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

                // Indulási érték (hányadiktól) 
                var start = Request.Form["start"].FirstOrDefault();

                // Keresési érték (search box)
                var searchValue = Request.Form["search[value]"].FirstOrDefault();


                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                var reviewData = await _unitOfWork.ReviewRepository.GetUnApprovedReviewsAsync();


                if (!string.IsNullOrEmpty(searchValue))
                {
                    reviewData = reviewData.Where(m => m.UserName.Contains(searchValue));
                }

                //rekordok száma  
                recordsTotal = reviewData.Count();
                //oldalazása   
                var data = reviewData.Skip(skip).Take(20).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                _logger.LogError("Hiba történt az értékelések betöltése során. Hiba: {}", ex.Message);
                return RedirectToAction("SomethingWentWrong", "Home");
            }
        }
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id.Value, r => r.Include(m => m.User));


            if (review == null)
            {
                return RedirectToAction("NoResult", "Home");
            }

            var unApprovedReview = new UnApprovedReviewViewModel()
            {
                Id = review.Id,
                UserName = review.User.UserName,
                Title = review.Title
            };

            return View(unApprovedReview);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);

                _unitOfWork.ReviewRepository.Remove(review);

                _unitOfWork.Commit();

                return RedirectToAction("UnApprovedReviews", "Reviews");
            }
            catch (Exception ex)
            {

                _logger.LogError("Hiba történt az értékelés törlése során. Hiba: {}", ex.Message);
                _unitOfWork.Dispose();
                return RedirectToAction("SomethingWentWrong", "Home");
            }
        }
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id.Value, r => r.Include(m => m.User).Include(m => m.Doctor));


            if (review == null)
            {
                return RedirectToAction("NoResult", "Home");
            }

            var unApprovedReview = new UnApprovedReviewViewModel()
            {
                Id = review.Id,
                UserName = review.User.UserName,
                Title = review.Title,
                Body = review.ReviewBody,
                Doctor = review.Doctor.IsStartWithDr == true ? $"dr. {review.Doctor.Name}" : review.Doctor.Name,
            };

            return View(unApprovedReview);
        }

        [HttpPost, ActionName("Approve")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveConfirmed(int id)
        {
            try
            {
              await _unitOfWork.ReviewRepository.ApproveReviewAsync(id);
              _unitOfWork.Commit();
              return RedirectToAction("UnApprovedReviews", "Reviews");
            }
            catch (Exception ex)
            {
                _logger.LogError("Hiba történt az értékelés jóváhagyása során. Hiba: {}", ex.Message);
                _unitOfWork.Dispose();
                return RedirectToAction("SomethingWentWrong", "Home");
            }     
        }
    }
}
