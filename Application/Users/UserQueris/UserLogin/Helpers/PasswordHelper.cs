using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace Application.Users.UserQueris.UserLogin.Helpers
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        public static bool VerifyPassword(string storedPasswordHash, string password)
        {
            var parts = storedPasswordHash.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = parts[1];

            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hash == storedHash;
        }
    }
}
