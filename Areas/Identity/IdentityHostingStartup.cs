using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(MarkMyDoctor.Areas.Identity.IdentityHostingStartup))]
namespace MarkMyDoctor.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}