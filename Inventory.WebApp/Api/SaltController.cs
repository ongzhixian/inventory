using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Inventory.WebApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApp.Api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class SaltController : ControllerBase
    {
        // GET: api/Salt/5
        [HttpGet("{length:range(1,255)}")]
        public string Get(byte length)
        {
            byte[] saltBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
                return Convert.ToBase64String(saltBytes);
            }

            // Alternative one-liner
            // return Convert.ToBase64String(SecurityHelper.GetRandomBytes(length));
        }

        // GET: api/Salt
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // // POST: api/Salt
        
        // [HttpPost]
        // public void Post([FromBody] string value)
        // {
        // }

        // // PUT: api/Salt/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody] string value)
        // {
        // }

        // // DELETE: api/ApiWithActions/5
        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
        // }
    }
}
