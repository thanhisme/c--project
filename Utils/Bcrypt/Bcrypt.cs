namespace WebApplication1.Utils.Encryption
{
    public static class Bcrypt
    {
        public static string HashPassword(string passsword)
        {
            return BCrypt.Net.BCrypt.HashPassword(passsword);
        }

        public static bool Verify(string password, string targetPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, targetPassword);
        }
    }
}
