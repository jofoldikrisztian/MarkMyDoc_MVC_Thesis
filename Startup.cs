using MarkMyDoctor.Data;
using MarkMyDoctor.Interfaces;
using MarkMyDoctor.Models.Entities;
using MarkMyDoctor.SeedData;
using MarkMyDoctor.Services;
using MarkMyDoctor.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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


            services.AddIdentity<User, IdentityRole<int>>(o => o.User.RequireUniqueEmail = true)
                .AddEntityFrameworkStores<DoctorDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<AppErrorDescriber>();

            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
            });


            services.ConfigureApplicationCookie(optoins =>
            {
                optoins.Cookie.HttpOnly = true;
                optoins.Cookie.IsEssential = true;
                optoins.LoginPath = "/Identity/Account/Login";
                optoins.AccessDeniedPath = "/Identity/Account/AccesDenied";

            });

            services.AddTransient<IAppEmailSender, EmailSender>();

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));


            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IRoleSeedService, RoleSeedService>();

            services.AddScoped<IUserSeedService, UserSeedService>();

            services.AddControllersWithViews();

            services.AddRazorPages();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();



            app.UseAuthentication();
            app.UseAuthorization();

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
