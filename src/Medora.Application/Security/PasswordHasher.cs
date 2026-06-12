using System.Security.Cryptography;
using System.Text;

namespace Medora.Application.Security;

public static class PasswordHasher
{
    public static string Hash(string tenantCode, string email, string password)
    {
        var normalized = $"{tenantCode.Trim().ToUpperInvariant()}|{email.Trim().ToLowerInvariant()}|{password}";
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(normalized)));
    }

    public static bool Verify(string tenantCode, string email, string password, string hash)
    {
        var computed = Encoding.UTF8.GetBytes(Hash(tenantCode, email, password));
        var stored = Encoding.UTF8.GetBytes(hash);
        return computed.Length == stored.Length && CryptographicOperations.FixedTimeEquals(computed, stored);
    }
}
