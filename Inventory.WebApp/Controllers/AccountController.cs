using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/LoginAsync
        [Route("Login")]
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginAsync(IFormCollection collection)
        {
            try
            {
                var claims = new List<Claim>
                {
                    // new Claim(ClaimTypes.Name, user.Email),
                    // new Claim("FullName", user.FullName),
                    new Claim(ClaimTypes.Role, "Administrator"),
                    new Claim(ClaimTypes.Name, "Administrator"),
                    new Claim(ClaimTypes.NameIdentifier, "Administrator")
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.
                    AllowRefresh = true

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.
                    ,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.
                    ,
                    IsPersistent = true

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.
                    ,
                    IssuedUtc = DateTimeOffset.Now

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );


                //return RedirectToLocal(returnUrl);
                //return RedirectToAction(nameof(SecurePage));
                return RedirectToAction("SecurePage", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Forbidden
        [AllowAnonymous]
        public ActionResult AccessDenied()
        {

            return View();
        }


        // GET: Account/Logout
        [AllowAnonymous]
        public async Task<ActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return View("Logout");
        }


        // [AllowAnonymous]
        // [HttpPost]
        // public IActionResult RequestToken([FromBody] TokenRequest request)
        // {
        //     if (request.Username == "Jon" && request.Password == "Again, not for production use, DEMO ONLY!")
        //     {
        //         var claims = new[]
        //         {
        //             new Claim(ClaimTypes.Name, request.Username)
        //         };

        //         var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
        //         var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

        //         var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
        //             issuer: "yourdomain.com",
        //             audience: "yourdomain.com",
        //             claims: claims,
        //             expires: DateTime.Now.AddMinutes(30),
        //             signingCredentials: creds);

        //         return Ok(new
        //         {
        //             token = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token)
        //         });
        //     }

        //     return BadRequest("Could not verify username and password");
        // }
    }
}