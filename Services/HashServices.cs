using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using ProjetoIntegrador.Interfaces;

namespace ProjetoIntegrador.Services
{
    public class HashServices : IHashServices
    {
        public string GenerateHash(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32));

            return hashed;
        }

        public byte[] GenerateSalt()
        {
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
