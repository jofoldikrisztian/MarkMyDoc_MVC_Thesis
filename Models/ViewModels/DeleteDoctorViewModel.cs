using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Models.ViewModels
{
    public class DeleteDoctorViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
