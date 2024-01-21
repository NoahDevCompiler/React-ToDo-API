using Org.BouncyCastle.Asn1.BC;
using System.Security.Cryptography;

namespace React__User_Control__API.Auth
{
    public static class PasswordHash
    {
        public static void CreatePassword(string password, out byte[] PasswordHashed, out byte[] PasswordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                PasswordSalt = hmac.Key;

                PasswordHashed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }
        public static bool VerifyPassword(string password,  byte[] storedHash,  byte[] storedSalt) {
            using(var hmac = new HMACSHA512(storedSalt)) {

                byte[] enterdpassHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return enterdpassHash.SequenceEqual(storedHash);
            }
        }
    }
}
