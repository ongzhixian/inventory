using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashController : ControllerBase
    {
        // GET: api/Hash
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Hash/5
        //[HttpGet("{id}", Name = "Get")]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Hash
        
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

    }
}
