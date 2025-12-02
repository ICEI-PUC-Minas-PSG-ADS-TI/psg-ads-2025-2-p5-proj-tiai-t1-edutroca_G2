using EduTroca.Core.Entities.UsuarioAggregate;

namespace EduTroca.Core.Abstractions;
public interface IJwtTokenGenerator
{
    string GenerateAccessToken(Usuario usuario);
}
