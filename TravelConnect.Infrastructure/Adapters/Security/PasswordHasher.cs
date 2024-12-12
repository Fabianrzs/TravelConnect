using System.Security.Cryptography;
using TravelConnect.Domain.Ports.Security;

namespace TravelConnect.Infrastructure.Adapters.Security;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16; // Tamaño del salt en bytes
    private const int HashSize = 32; // Tamaño del hash en bytes
    private const int Iterations = 10000; // Número de iteraciones de PBKDF2

    /// <summary>
    /// Genera un hash de contraseña utilizando PBKDF2 con un salt único.
    /// </summary>
    public string HashPassword(string password)
    {
        // Generar salt aleatorio
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);

        // Generar hash de la contraseña
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize
        );

        // Combinar salt y hash en un solo arreglo
        var hashBytes = new byte[SaltSize + HashSize];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
        Buffer.BlockCopy(hash, 0, hashBytes, SaltSize, HashSize);

        // Convertir a Base64 para almacenamiento
        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    /// Verifica si la contraseña proporcionada coincide con el hash almacenado.
    /// </summary>
    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        // Convertir el hash almacenado de Base64 a bytes
        var hashBytes = Convert.FromBase64String(hashedPassword);

        // Extraer salt y hash del arreglo
        var salt = new byte[SaltSize];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

        var storedHash = new byte[HashSize];
        Buffer.BlockCopy(hashBytes, SaltSize, storedHash, 0, HashSize);

        // Generar hash de la contraseña proporcionada usando el mismo salt
        var providedHash = Rfc2898DeriveBytes.Pbkdf2(
            providedPassword,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize
        );

        // Comparar el hash proporcionado con el almacenado
        return CryptographicOperations.FixedTimeEquals(storedHash, providedHash);
    }
}
