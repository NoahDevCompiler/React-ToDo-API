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
    }
}
