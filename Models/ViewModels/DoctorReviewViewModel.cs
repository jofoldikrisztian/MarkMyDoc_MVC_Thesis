
using MarkMyDoctor.Models.Entities;

namespace MarkMyDoctor.Models.ViewModels
{
    public class DoctorReviewViewModel
    {
        public Doctor Doctor { get; set; }
        public Review Review { get; set; }
        public DoctorReviewViewModel()
        {
            Review = new Review();
            Doctor = new Doctor();
        }
    }
}