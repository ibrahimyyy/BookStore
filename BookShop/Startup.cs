using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Data;
using BookShop.Repository;
using Microsoft.AspNetCore.Identity;
using BookShop.Models;
using BookShop.Helpers;
using BookShop.Service;

namespace BookShop
{
    public class Startup
    {        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BookShopContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));//here i declear the method here in (Startup.cs).
           services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BookShopContext>().AddDefaultTokenProviders(); //it will give u all features in this package but this (services.AddIdentityCore) it will give a minimal featuers like login and sign in so we use (services.AddIdentity) because it's more useful , and this services it will work with 2 classes its IdentityUser and IdentityRole , and i should put what DBContext i work on it.
            services.AddControllersWithViews();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));
            services.Configure<IdentityOptions>(option => 
            {
                option.Lockout.MaxFailedAccessAttempts = 3; // when user failed to login his account for 3 times it will be blocked for 10min.
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                option.Password.RequireDigit = true;
                option.Password.RequiredLength = 5;
                option.Password.RequireUppercase = false;
                option.SignIn.RequireConfirmedEmail = true; // this for verify the account.
            });//this service make me put the syntax of password user i want (like i should has Digit and at least 1 char Uppercase and many things) and many other things like SignIn.
            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/login";
            });//this service make the method which i have to be login(Authorize) to use it will redirect me to login firts instead of error.
            services.Configure<DataProtectionTokenProviderOptions>(option =>
            {
                option.TokenLifespan = TimeSpan.FromMinutes(5);
            });// this service provide me to set the life span of the token
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
