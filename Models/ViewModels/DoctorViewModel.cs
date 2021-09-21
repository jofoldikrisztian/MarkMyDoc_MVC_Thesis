using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MarkMyDoctor.Data;
using MarkMyDoctor.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarkMyDoctor.Models.ViewModels
{
    public class DoctorViewModel
    {
        public ICollection<SelectListItem>? Specialities { get; init; }
        public Doctor Doctor { get; init; }
        [Display(Name ="Specialitások:")]
        public List<string> SelectedSpecialityIds { get; init; }

        public IFormFile? Image { get; set; }

    }
}
