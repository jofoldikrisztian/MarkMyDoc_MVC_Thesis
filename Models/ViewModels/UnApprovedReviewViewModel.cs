using System.ComponentModel.DataAnnotations;

namespace MarkMyDoctor.Models.ViewModels
{
    public class UnApprovedReviewViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Felhasználónév:")]
        public string UserName { get; set; }
        [Display(Name = "Értékelés címe:")]
        public string Title { get; set; }
        [Display(Name = "Értékelés szövege:")]
        public string? Body { get; set; }
        [Display(Name = "Értékelt orvos neve:")]
        public string Doctor { get; set; }
    }
}
