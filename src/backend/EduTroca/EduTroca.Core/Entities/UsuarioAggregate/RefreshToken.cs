namespace EduTroca.Core.Entities.UsuarioAggregate;
public class RefreshToken : Entity
{
    public string Token { get; private set; }
    public DateTime ExpiresOnUtc { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? RevokedOnUtc { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresOnUtc;
    public bool IsRevoked => RevokedOnUtc != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    protected RefreshToken() { }

    public RefreshToken(string token, DateTime expiresOnUtc)
    {
        Token = token;
        ExpiresOnUtc = expiresOnUtc;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void Revoke()
    {
        RevokedOnUtc = DateTime.UtcNow;
    }
}
