using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.ResendEmailConfirmation;
public record ResendEmailConfirmationCommand(string email) : IRequest<ErrorOr<Success>>;
