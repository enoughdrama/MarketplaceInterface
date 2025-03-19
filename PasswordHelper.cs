using System;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Xml.Linq;

public static class PasswordHelper
{
    public static void CreatePasswordHash(string password, out string passwordHash, out string salt)
    {
        byte[] saltBytes = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }
        salt = Convert.ToBase64String(saltBytes);

        using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000))
        {
            byte[] hashBytes = deriveBytes.GetBytes(20);
            passwordHash = Convert.ToBase64String(hashBytes);
        }
    }

    public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
    {
        byte[] saltBytes = Convert.FromBase64String(storedSalt);
        using (var deriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000))
        {
            byte[] enteredHashBytes = deriveBytes.GetBytes(20);
            string enteredHash = Convert.ToBase64String(enteredHashBytes);
            return enteredHash == storedHash;
        }
    }
}