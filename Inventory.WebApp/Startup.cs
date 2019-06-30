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
using Microsoft.AspNetCore.Rewrite;
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
            //services.AddAuthorization(options =>
            //{
            //    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
            //        Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
            //        Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
            //        "AzureAD"
            //        );

            //    defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
            //    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();

            //    //    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
            //    //JwtBearerDefaults.AuthenticationScheme,
            //    //"AzureAD");
            //    //    defaultAuthorizationPolicyBuilder =
            //    //        defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
            //    //    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();

            //    //options.AddPolicy("Over18", policy =>
            //    //{
            //    //    policy.AuthenticationSchemes.Add(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);
            //    //    policy.RequireAuthenticatedUser();
            //    //    policy.Requirements.Add(new MinimumAgeRequirement());
            //    //});
            //});

            // Override antiforgery cookie
            services.AddAntiforgery(options => {
                options.Cookie.Name = "Inventory.AntiForgery";
            });

            // services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
            // .AddCookie(options => {
            // });
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                
            })
            .AddCookie(options => {
                //Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.LoginPath
                // https://github.com/aspnet/AspNetCore/blob/master/src/Security/Authentication/Cookies/src/CookieAuthenticationDefaults.cs
                //options.AccessDeniedPath = "/Account/AccessDenied/";  // Defaults to: "/Account/AccessDenied"; See CookieAuthenticationDefaults.LoginPath
                //options.LoginPath = "/Account/Login/";                // Defaults to: "/Account/Login"; See CookieAuthenticationDefaults.LoginPath
                //options.LogoutPath = "/Account/Logout/";              // Defaults to: "/Account/Logout"; See CookieAuthenticationDefaults.LoginPath
                // options.SlidingExpiration = true;                    // Defaults is: true
                //options.ReturnUrlParameter = "go";                    // Default is: "ReturnUrl"
                //options.ExpireTimeSpan = TimeSpan.FromDays(14);       // Default is: TimeSpan.FromDays(14);
                options.Cookie.Name = "csi.authentication";
            })
            .AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    ValidAudience = Configuration["Tokens:Issuer"],
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                };


                options.Audience = "http://localhost:5001/";
                options.Authority = "http://localhost:5000/";
            })
            .AddJwtBearer("AzureAD", options =>
            {
                options.Audience = "https://localhost:5000/";
                options.Authority = "https://login.microsoftonline.com/eb971100-6f99-4bdc-8611-1bc8edd7f436/";
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

            // Demo Redirect vs Rewrite 
            // https://docs.microsoft.com/en-gb/aspnet/core/fundamentals/url-rewriting?view=aspnetcore-2.2
            // https://tahirnaushad.com/2017/08/18/url-rewriting-in-asp-net-core/
            // Redirect: Server responds with status code 301 (Moved Permanently) or 302 (Found) with new Location header, instructing client to request the new location e.g. /movies
            // Rewrite: Server will internally map to new location e.g. /stars and return 200 (OK).
            var rewrite = new RewriteOptions()
                .AddRedirect("films", "movies")
                .AddRewrite("actors", "stars", true);
            app.UseRewriter(rewrite);

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
