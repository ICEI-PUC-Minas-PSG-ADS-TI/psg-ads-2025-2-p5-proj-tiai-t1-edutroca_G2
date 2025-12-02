using EduTroca.Core.Abstractions;
using EduTroca.Core.Enums;

namespace EduTroca.Core.Entities.UsuarioAggregate;
public class Usuario : ISoftDelete
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string SenhaHash { get; private set; }
    public string Bio { get; private set; }
    public string CaminhoImagem { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedOnUtc { get; private set; }
    public EmailConfirmationCode EmailConfirmationCode { get; private set; }
    public bool IsConfirmed => EmailConfirmationCode.UsedOnUtc is not null;
    private readonly List<Role> _roles = new();
    private readonly List<RefreshToken> _refreshTokens = new();
    private readonly List<Categoria> _categoriasDeInteresse = new();
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
    public IReadOnlyCollection<Categoria> CategoriasDeInteresse => _categoriasDeInteresse.AsReadOnly();

    protected Usuario()
    {
    }
    public Usuario(string nome, string email, string senhaHash, DateTime emailConfirmationExpiresOnUtc, Role initialRole)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        SenhaHash = senhaHash;
        Bio = string.Empty;
        EmailConfirmationCode = new EmailConfirmationCode(emailConfirmationExpiresOnUtc);
        CaminhoImagem = string.Empty;
        _roles.Add(initialRole);
        IsDeleted = false;
    }
    public void AddRefreshToken(RefreshToken refreshToken)
    {
        _refreshTokens.Add(refreshToken);
    }
    public void RemoveRefreshToken(string token)
    {
        var refreshToken = _refreshTokens.FirstOrDefault(rt => rt.Token == token);
        if (refreshToken is null)
            throw new InvalidOperationException($"O refresh token {token} não pertence ao usuario {Id}");
        _refreshTokens.Remove(refreshToken);
    }
    public void RevokeRefreshToken(string token)
    {
        var refreshToken = _refreshTokens.FirstOrDefault(rt => rt.Token == token);
        if (refreshToken is null)
            throw new InvalidOperationException($"O refresh token {token} não pertence ao usuario {Id}");
        refreshToken.Revoke();
    }
    public void RevokeAllRefreshTokens()
    {
        foreach (var token in _refreshTokens.Where(rt => !rt.IsRevoked))
            token.Revoke();
    }
    public void AddRole(Role role)
    {
        if (_roles.Any(r => r.Id == role.Id))
            throw new InvalidOperationException($"O usuario {Id} ja possui a role {role.Id}");
        _roles.Add(role);
    }
    public void RemoveRole(Role role)
    {
        if (!_roles.Any(r => r.Id == role.Id))
            throw new InvalidOperationException($"O usuario {Id} não possui a role {role.Id}");
        if(role.Id is (int)ERole.User)
            throw new InvalidOperationException($"Não é possivel remover a role {role.Id}");
        _roles.Remove(role);
    }
    public void AddCategoriaDeInteresse(Categoria categoria)
    {
        if (_categoriasDeInteresse.Any(cat => cat.Id == categoria.Id))
            throw new InvalidOperationException($"O usuario {Id} ja possui a categoria {categoria.Id}");
        _categoriasDeInteresse.Add(categoria);
    }
    public void ConfirmEmail()
    {
        EmailConfirmationCode.Use();
    }
    public void RegenerateEmailConfirmationCode(DateTime expiresOnUtc)
    {
        if (IsConfirmed)
            throw new InvalidOperationException("Não é possível gerar um novo código para um email já confirmado.");

        EmailConfirmationCode = new EmailConfirmationCode(expiresOnUtc);
    }
    public void UpdateProfile(string nome, string bio)
    {
        Nome = nome;
        Bio = bio;
    }
    public void UpdatePicture(string fotoPath)
    {
        CaminhoImagem = fotoPath;
    }
    public void UpdateEmail(string novoEmail, DateTime emailConfirmationExpiresOnUtc)
    {
        Email = novoEmail;
        RegenerateEmailConfirmationCode(emailConfirmationExpiresOnUtc);
    }
    public void RemoveCategoriaDeInteresse(Categoria categoria)
    {
        if (!_categoriasDeInteresse.Any(cat => cat.Id == categoria.Id))
            throw new InvalidOperationException($"O usuario {Id} não possui a categoria {categoria.Id}");
        _categoriasDeInteresse.Remove(categoria);
    }
    public void Delete()
    {
        IsDeleted = true;
        DeletedOnUtc = DateTime.UtcNow;
    }
}
