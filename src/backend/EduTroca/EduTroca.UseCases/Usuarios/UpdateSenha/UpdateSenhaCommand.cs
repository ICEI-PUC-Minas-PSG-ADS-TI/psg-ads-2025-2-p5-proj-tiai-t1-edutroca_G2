using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.UpdateSenha;
public record UpdateSenhaCommand(string usuarioEmail, string senhaAtual, string senhaNova) : IRequest<ErrorOr<Success>>;
