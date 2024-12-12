using System.Security.Cryptography;

namespace TravelConnect.Domain.Utils;

public class CrypterDefault
{
    private static readonly byte[] Key = Convert.FromBase64String("QuSs0h/2tVnNz1EklGvGXNYWKAdnDwR4z6uZrxsqWfc=");
    private static readonly byte[] FixedIV = [0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F];

    public static string Encrypt(string plainText)
    {
        using Aes aesAlg = Aes.Create();
        aesAlg.Key = Key;
        aesAlg.IV = FixedIV; // Usar IV fijo de 16 bytes

        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        byte[] encryptedData;
        using (var msEncrypt = new System.IO.MemoryStream())
        {
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            encryptedData = msEncrypt.ToArray();
        }

        return Convert.ToBase64String(encryptedData);
    }

    public static string Decrypt(string encryptedText)
    {
        byte[] cipherText = Convert.FromBase64String(encryptedText);

        using Aes aesAlg = Aes.Create();
        aesAlg.Key = Key;
        aesAlg.IV = FixedIV; // Usar IV fijo de 16 bytes

        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        string plainText;

        using (var msDecrypt = new System.IO.MemoryStream(cipherText))
        {
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new System.IO.StreamReader(csDecrypt);
            plainText = srDecrypt.ReadToEnd();
        }

        return plainText;
    }
}
