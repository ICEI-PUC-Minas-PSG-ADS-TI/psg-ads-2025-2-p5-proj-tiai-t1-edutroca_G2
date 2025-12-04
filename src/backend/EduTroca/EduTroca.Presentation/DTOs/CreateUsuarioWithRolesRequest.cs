using EduTroca.Core.Enums;

namespace EduTroca.Presentation.DTOs;
public record CreateUsuarioWithRolesRequest(string nome, string email, string senha, List<ERole> roles)
    : CreateUsuarioRequest(nome, email, senha);
