using System.Security.Cryptography;
using System.Text;

namespace Game.Helpers;

public static class HashHelper
{
    public static string GetSha256Hash(string input)
    {
        string hash;
            
        using (var sha256 = SHA256.Create())
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = sha256.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }

            hash = sb.ToString();
        }

        return hash;
    }
}