using MarkMyDoctor.Data;
using MarkMyDoctor.Extensions;
using MarkMyDoctor.Interfaces;
using MarkMyDoctor.Models.Entities;
using MarkMyDoctor.SeedData;
using MarkMyDoctor.Services;
using MarkMyDoctor.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using static MarkMyDoctor.SeedData.SeedData;

namespace MarkMyDoctor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddDbContext<DoctorDbContext>(options =>
                                        options.UseSqlServer(Configuration.GetConnectionString("MarkMyDoctorDB"),
                                        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                                        .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));


            services.AddIdentity<User, IdentityRole<int>>(o =>
            {
                o.User.RequireUniqueEmail = true;
                o.Lockout.AllowedForNewUsers = true;
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                o.Lockout.MaxFailedAccessAttempts = 3;
            })
                .AddEntityFrameworkStores<DoctorDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<AppErrorDescriber>();

            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
            });


            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccesDenied";
          

            });

            services.AddTransient<IAppEmailSender, EmailSender>();

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));


            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IRoleSeedService, RoleSeedService>();

            services.AddScoped<IUserSeedService, UserSeedService>();

            services.AddControllersWithViews();

            services.AddRazorPages();


        }

      
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseForwardedHeaders();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseForwardedHeaders();
               
                app.UseHsts();
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseUserChecker();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            SeedEntitiesData(app);
            UsersAndRoles(app);
        }
    }
}
