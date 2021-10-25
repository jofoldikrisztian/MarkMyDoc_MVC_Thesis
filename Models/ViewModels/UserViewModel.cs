using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Models.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Password { get; set; }
    }
}
