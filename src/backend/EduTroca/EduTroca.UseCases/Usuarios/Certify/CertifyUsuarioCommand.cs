using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.Certify;
public record CertifyUsuarioCommand(Guid usuarioId) : IRequest<ErrorOr<Success>>;
