using System.Security.Cryptography;

namespace EduTroca.Core.Entities.UsuarioAggregate;
public class RefreshToken
{
    public Guid Id { get; private set; }
    public string Token { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime ExpiresOnUtc { get; private set; }
    public bool IsRevoked { get; private set; }
    protected RefreshToken()
    {
    }
    public RefreshToken(DateTime expiresOnUtc)
    {
        Id = Guid.NewGuid();
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        Token = Convert.ToBase64String(randomBytes);
        CreatedOnUtc = DateTime.UtcNow;
        ExpiresOnUtc = expiresOnUtc;
        IsRevoked = false;
    }

    public void Revoke()
    {
        IsRevoked = true;
    }
}
