using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.ConfirmEmail;
public record ConfirmEmailCommand(Guid id) : IRequest<ErrorOr<Success>>;
