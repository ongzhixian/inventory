using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApp.Api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class DiceController : ControllerBase
    {
        private RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        // GET: api/Dice/5
        //[HttpGet("{id}", Name = "Get")]
        [HttpGet("{spec:regex((?<num>\\d*)[[d|D]](?<sides>\\d+))}")]
        public int Get(string spec)
        {
            Regex diceRegex = new Regex(@"(?<num>\d*)[d|D](?<sides>\d+)");
            Match m = diceRegex.Match(spec);
            int numberOfDice = 0;
            byte numberOfSides = 0;

            if (!int.TryParse(m.Groups["num"].Value, out numberOfDice))
            {
                numberOfDice = 1;
            }
            if (!byte.TryParse(m.Groups["sides"].Value, out numberOfSides))
            {
                numberOfSides = 1;
            }

            int total = 0;
            for (int i = 0; i < numberOfDice; i++) 
            {
                total += RollDice(numberOfSides);
            }
            return total;
            //https://localhost:5001/api/dice/d4
            //return 18;
        }


        private byte RollDice(byte numberSides)
        {
            if (numberSides <= 0)
                throw new ArgumentOutOfRangeException("numberSides");

            // Create a byte array to hold the random value.
            byte[] randomNumber = new byte[1];
            do
            {
                // Fill the array with a random value.
                rngCsp.GetBytes(randomNumber);
            }
            while (!IsFairRoll(randomNumber[0], numberSides));
            // Return the random number mod the number
            // of sides.  The possible values are zero-
            // based, so we add one.
            return (byte)((randomNumber[0] % numberSides) + 1);
        }

        private static bool IsFairRoll(byte roll, byte numSides)
        {
            // There are MaxValue / numSides full sets of numbers that can come up
            // in a single byte.  For instance, if we have a 6 sided die, there are
            // 42 full sets of 1-6 that come up.  The 43rd set is incomplete.
            int fullSetsOfValues = Byte.MaxValue / numSides;

            // If the roll is within this range of fair values, then we let it continue.
            // In the 6 sided die case, a roll between 0 and 251 is allowed.  (We use
            // < rather than <= since the = portion allows through an extra 0 value).
            // 252 through 255 would provide an extra 0, 1, 2, 3 so they are not fair
            // to use.
            return roll < numSides * fullSetsOfValues;
        }

        // // GET: api/Dice
        // [HttpGet]
        // public IEnumerable<string> Get()
        // {
        //     return new string[] { "value1", "value2" };
        // }

        // // POST: api/Dice

        // [HttpPost]
        // public void Post([FromBody] string value)
        // {
        // }

        // // PUT: api/Dice/5
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
