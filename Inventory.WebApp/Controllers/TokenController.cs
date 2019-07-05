using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Inventory.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Inventory.WebApp.Controllers
{
    public class TokenController : Controller
    {
        private readonly IConfiguration _configuration;
        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("token")]
        public IActionResult Post([FromBody]LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserIdFromCredentials(loginViewModel);
                if (userId == -1)
                {
                    return Unauthorized();
                }

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, loginViewModel.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                string issuer = "inventory_webapp";     // Configuration.GetSection("TokenProviderOptions:Issuer").Value
                string audience = "zhixian";            //Configuration.GetSection("TokenProviderOptions:Audience").Value
                string signingKey = "signingkey";       // _configuration["SigningKey"]

                // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
                // var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                // The algorithm: 'HS256' requires the SecurityKey.KeySize to be greater than '128' bits. 
                //KeySize reported: '80'. 
                //Parameter name: KeySize

                var token = new JwtSecurityToken
                (
                    issuer: issuer, // _configuration["Issuer"]
                    audience: audience, // _configuration["Audience"]
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(60),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                         SecurityAlgorithms.HmacSha256)
                );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return BadRequest();
        }

        private int GetUserIdFromCredentials(LoginViewModel loginViewModel)
        {
            var userId = -1;
            if (loginViewModel.Username == "zhixian" && loginViewModel.Password == "password")
            {
                userId = 5;
            }

            return userId;
        }
    }
}