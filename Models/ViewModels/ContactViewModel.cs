using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Models.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage="A név megadása kötelező!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Az email cím megadása kötelező!")]
        public string  Email { get; set; }
        [Required(ErrorMessage = "Az üzenet magadása kötelező!")]
        public string Message { get; set; }
        public string? StatusMessage { get; set; }

    }
}
