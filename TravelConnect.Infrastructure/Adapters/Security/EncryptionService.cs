using System.Security.Cryptography;
using System.Text;
using TravelConnect.Domain.Ports;

namespace TravelConnect.Infrastructure.Adapters.Security;

public class EncryptionService(string encryptionKey) : IEncryptionService
{
    private readonly string _encryptionKey = encryptionKey ?? throw new ArgumentNullException(nameof(encryptionKey));

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        var key = Encoding.UTF8.GetBytes(_encryptionKey.PadRight(32));
        aes.Key = key;
        aes.IV = new byte[16];

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt(string cipherText)
    {
        using var aes = Aes.Create();
        var key = Encoding.UTF8.GetBytes(_encryptionKey.PadRight(32));
        aes.Key = key;
        aes.IV = new byte[16];

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
}
