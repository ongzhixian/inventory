using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Inventory.WebApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApp.Api.Public
{
    [AllowAnonymous]
    [Route("api/Public/[controller]")]
    [ApiController]
    public class SaltController : ControllerBase
    {
        
        public class PostResult 
        {
            public int SaltLength { get;set;}
            public string Base64Salt { get;set;}
        }

        // POST: api/Salt
        
        [HttpPost]
        //public void  Post([FromBody] string value)
        public IActionResult Post([FromBody]int length)
        {
            byte[] saltBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);

                PostResult result = new PostResult {
                    SaltLength = length,
                    Base64Salt = Convert.ToBase64String(saltBytes)
                };

                return Ok(result);
            }

            //return this.NoContent();

            //System.Diagnostics.Debug.Write(value);
            
            //return result;
            //return Ok();

            // var responseMessage = new HttpResponseMessage<List<string>>(errors, HttpStatusCode.BadRequest);
            // throw new HttpResponseException(responseMessage);
        }

        // https://docs.microsoft.com/en-us/aspnet/web-api/overview/getting-started-with-aspnet-web-api/action-results
        // public HttpResponseMessage Get()
        // {
        // HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
        // response.Content = new StringContent("hello", Encoding.Unicode);
        // response.Headers.CacheControl = new CacheControlHeaderValue()
        // {
        //     MaxAge = TimeSpan.FromMinutes(20)
        // };
        // return response;
        // } 

        // GET: api/Salt
        // [HttpGet]
        // public IEnumerable<string> Get()
        // {
        //     return new string[] { "value1", "value2" };
        // }

        // // GET: api/Salt/5
        // [HttpGet("{length:range(1,255)}")]
        // public string Get(byte length)
        // {
        //     byte[] saltBytes = new byte[length];
        //     using (var rng = RandomNumberGenerator.Create())
        //     {
        //         rng.GetBytes(saltBytes);
        //         return Convert.ToBase64String(saltBytes);
        //     }

        //     // Alternative one-liner
        //     // return Convert.ToBase64String(SecurityHelper.GetRandomBytes(length));
        // }





    }
}
