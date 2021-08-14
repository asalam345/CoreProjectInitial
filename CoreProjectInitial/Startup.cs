using CoreProjectInitial.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreProjectInitial
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                //options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });
            services.AddMvc(config => {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddDbContext<AspnetRunContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //add identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AspnetRunContext>()
                .AddDefaultTokenProviders();

            //services.AddIdentity<IdentityUser, IdentityRole>()
            //      //.AddDefaultUI(UIFramework.Bootstrap4)
            //      .AddEntityFrameworkStores<AspnetRunContext>();

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddRazorPages();

            //services.AddControllersWithViews();
            //services.AddMvcCore();
            //services.AddIdentity<User, IdentityRole>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
            });
            //services.AddIdentity<IdentityUser, IdentityRole>(options =>
            //{
            //    options.Password.RequiredLength = 5;
            //    options.Password.RequiredUniqueChars = 3;
            //    options.Password.RequireNonAlphanumeric = false;
            //})
            //.AddEntityFrameworkStores<AspnetRunContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)// IWebHostEnvironment env
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions
                {
                    SourceCodeLineCount = 10
                };
                app.UseDeveloperExceptionPage(developerExceptionPageOptions);
            }

            else
            {
                app.UseExceptionHandler("/Error");
                app.UseExceptionHandler("/Error/500");
                app.UseStatusCodePagesWithReExecute("/Error/NotFound/{0}");

            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        //public void ConfigureDatabases(IServiceCollection services)
        //{
        //    // use in-memory database
        //    services.AddDbContext<AspnetRunContext>(c =>
        //        c.UseInMemoryDatabase("AspnetRunConnection"));

        //    //// use real database
        //    //services.AddDbContext<AspnetRunContext>(c =>
        //    //    c.UseSqlServer(Configuration.GetConnectionString("AspnetRunConnection"), x => x.MigrationsAssembly("AspnetRun.Web")));
        //}

        //public void ConfigureIdentity(IServiceCollection services)
        //{
        //    services.AddDefaultIdentity<IdentityUser>()
        //        .AddDefaultUI()
        //        .AddEntityFrameworkStores<AspnetRunContext>();

        //    services.Configure<IdentityOptions>(options =>
        //    {
        //        // Password settings.
        //        options.Password.RequireDigit = false;
        //        options.Password.RequireLowercase = false;
        //        options.Password.RequireNonAlphanumeric = false;
        //        options.Password.RequireUppercase = false;
        //        options.Password.RequiredLength = 6;
        //        options.Password.RequiredUniqueChars = 1;
        //    });
        //}
    }
}
