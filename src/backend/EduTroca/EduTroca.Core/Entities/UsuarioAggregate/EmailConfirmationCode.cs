namespace EduTroca.Core.Entities.UsuarioAggregate;
public class EmailConfirmationCode : Entity
{
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime ExpiresOnUtc { get; private set; }
    public DateTime? UsedOnUtc { get; private set; }
    protected EmailConfirmationCode()
    {
    }

    public EmailConfirmationCode(DateTime expiresOnUtc)
    {
        CreatedOnUtc = DateTime.UtcNow;
        ExpiresOnUtc = expiresOnUtc;
        UsedOnUtc = null;
    }
    public void Use()
    {
        UsedOnUtc = DateTime.UtcNow;
    }
}
