using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.UpdateSenha;
public record UpdateSenhaCommand(Guid usuarioId, string? senhaAtual, string senhaNova) : IRequest<ErrorOr<Success>>;
