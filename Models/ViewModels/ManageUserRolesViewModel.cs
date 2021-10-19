using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Models.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
}
