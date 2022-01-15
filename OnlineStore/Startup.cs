using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineStore.Data;
using OnlineStore.Entities;
using OnlineStore.Interfaces;
using OnlineStore.Repositories;
using OnlineStore.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore
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
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var en = new CultureInfo("en-US");
                en.NumberFormat.NumberDecimalSeparator = ".";
                en.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                en.DateTimeFormat.LongTimePattern = "dd/MM/yyyy";
                en.DateTimeFormat.ShortTimePattern = "HH:mm";
                en.DateTimeFormat.LongTimePattern = "HH:mm";
                var al = new CultureInfo("sq-AL");
                al.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
                al.DateTimeFormat.LongTimePattern = "dd.MM.yyyy";
                al.DateTimeFormat.ShortTimePattern = "HH:mm";
                al.DateTimeFormat.LongTimePattern = "HH:mm";
                al.NumberFormat.NumberDecimalSeparator = ".";

                var supportedCultures = new[]
                {
                   en,
                   al
                };

                options.DefaultRequestCulture = new RequestCulture(en, en);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false; // was true
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });

            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.AddAuthorization(option =>
            {
                option.AddPolicy("SuperAdmin", policy => policy.RequireRole("Super Admin"));
                option.AddPolicy("Admin", policy => policy.RequireRole("Admin", "Super Admin"));
                option.AddPolicy("Manager", policy => policy.RequireRole("Admin", "Super Admin", "Manager"));
                option.AddPolicy("Receptionist", policy => policy.RequireRole("Admin", "Super Admin", "Receptionist"));
                option.AddPolicy("Client", policy => policy.RequireRole("Client"));
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                //options.LoginPath = "/Identity/Account/Login";
                //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            // Add application services.
            services.AddTransient<ISelectListService, SelectListService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRolesRepository, RolesRepository>();
            services.AddTransient<IUserService, UserService>();
            //services.AddScoped<INewsletterService, NewsletterService>();
            //services.AddTransient<IRepository, Repository>();
            services.AddControllersWithViews();

            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            services.AddMvc()
                .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
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
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession(); // was added

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "MyArea", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(name: "default", pattern: "{area=Store}/{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
