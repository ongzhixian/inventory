using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.WebApp
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

                // Overide consent cookie
                options.ConsentCookie = new CookieBuilder()
                {
                    Name = ".Inventory.Consent",            // Change from default of ".AspNet.Consent"
                    Expiration = TimeSpan.FromDays(365),    // 
                    IsEssential = true,                     // 
                    //HttpOnly = true                       // Cookie won't be set if this true
                };

                options.Secure = CookieSecurePolicy.Always; // Default: CookieSecurePolicy.None

                // ZX:  For some reason, the HttpOnly does not work for the CookiePolicy 
                //      Probably has got to do with the way it is implemented.
                //      Comment out for now.
                //options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
                
            });

            // services.AddAuthorization(options => {
            // });
            // services.AddAuthorization(options =>
            // {
            //     options.AddPolicy("Over18", policy =>
            //     {
            //         policy.AuthenticationSchemes.Add(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);
            //         policy.RequireAuthenticatedUser();
            //         policy.Requirements.Add(new MinimumAgeRequirement());
            //     });
            // });

            // Override antiforgery cookie
            services.AddAntiforgery(options => {
                options.Cookie.Name = "Inventory.AntiForgery";
            });

            // services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
            // .AddCookie(options => {
            // });
            services.AddAuthentication()
            .AddCookie(options => {
                options.LoginPath = "/Account/Unauthorized/";
                options.AccessDeniedPath = "/Account/Forbidden/";
            })
            .AddJwtBearer(options => {
                options.Audience = "http://localhost:5001/";
                options.Authority = "http://localhost:5000/";
            });

            services.AddMvc(options => {
                // Add filters here
                //options.Filters.Add()
                
                // Add default policy which only allow authenticated users to view
                // 
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // app.UseStatusCodePages(); // Status Code: 404; Not Found
            // app.UseStatusCodePages("text/plain", "Status code page, status code: {0}");   
            // app.UseStatusCodePages(async context =>
            // {
            //     context.HttpContext.Response.ContentType = "text/plain";

            //     await context.HttpContext.Response.WriteAsync(
            //         "Status code page, status code: " + 
            //         context.HttpContext.Response.StatusCode);
            // });
            app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Use authentication
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
