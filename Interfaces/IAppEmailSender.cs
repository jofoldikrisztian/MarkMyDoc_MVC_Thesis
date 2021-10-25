using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Interfaces
{
    public interface IAppEmailSender : IEmailSender
    {
        Task SendEmailToStaffAsync(string name, string from, string message);
    }
}
