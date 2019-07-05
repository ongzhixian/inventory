using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Inventory.WebApp.Helpers
{
    public class DiceHelper
    {
        public byte[] GetRandomBytes(int size)
        {
            byte[] randomBytes = new byte[size];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                return randomBytes;
            }
        }

        public string GetRandomBytesAsBase64(int size)
        {
            return Convert.ToBase64String(GetRandomBytes(size));
        }

//Password-Based Key Derivation Function 2
        public byte[] DeriveKeyBytes(string password, byte[] salt, int iteration = 10000, int length = 256)
        {
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: iteration,
                numBytesRequested: length);
        }
    
    } // end class
} // end namespace