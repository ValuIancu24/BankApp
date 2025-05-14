using System.Security.Cryptography;
using System.Text;

namespace BankingApp.Api.Helpers;

public static class PasswordHelper
{
    public static string HashPassword(string password)
    {
        using (var hmac = new HMACSHA512())
        {
            var salt = hmac.Key;
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            // Combine salt and hash
            var result = new byte[salt.Length + hash.Length];
            Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
            Buffer.BlockCopy(hash, 0, result, salt.Length, hash.Length);
            
            return Convert.ToBase64String(result);
        }
    }
    
    public static bool VerifyPassword(string hashedPassword, string password)
    {
        var bytes = Convert.FromBase64String(hashedPassword);
        
        // Extract salt (first 64 bytes)
        var salt = new byte[64];
        Buffer.BlockCopy(bytes, 0, salt, 0, salt.Length);
        
        // Extract stored hash (remaining bytes)
        var hash = new byte[bytes.Length - salt.Length];
        Buffer.BlockCopy(bytes, salt.Length, hash, 0, hash.Length);
        
        // Compute hash with the same salt
        using (var hmac = new HMACSHA512(salt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            // Compare computed hash with stored hash
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != hash[i])
                    return false;
            }
        }
        
        return true;
    }
}