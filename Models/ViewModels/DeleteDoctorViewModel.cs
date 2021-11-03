using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Models.ViewModels
{
    public class DeleteDoctorViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Név:")]
        public string Name { get; set; }
        [Display(Name = "Email cím:")]
        public string Email { get; set; }
        [Display(Name = "Telefonszám:")]
        public string PhoneNumber { get; set; }
    }
}
