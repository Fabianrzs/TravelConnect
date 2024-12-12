using System.Security.Cryptography;
using TravelConnect.Domain.Ports.Security;

namespace TravelConnect.Infrastructure.Adapters.Security;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16; 
    private const int HashSize = 32; 
    private const int Iterations = 10000; 

    /// <summary>
    /// Genera un hash de contraseña utilizando PBKDF2 con un salt único.
    /// </summary>
    public string HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize
        );

        var hashBytes = new byte[SaltSize + HashSize];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
        Buffer.BlockCopy(hash, 0, hashBytes, SaltSize, HashSize);

        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    /// Verifica si la contraseña proporcionada coincide con el hash almacenado.
    /// </summary>
    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var hashBytes = Convert.FromBase64String(hashedPassword);

        var salt = new byte[SaltSize];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

        var storedHash = new byte[HashSize];
        Buffer.BlockCopy(hashBytes, SaltSize, storedHash, 0, HashSize);

        var providedHash = Rfc2898DeriveBytes.Pbkdf2(
            providedPassword,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize
        );

        return CryptographicOperations.FixedTimeEquals(storedHash, providedHash);
    }
}
