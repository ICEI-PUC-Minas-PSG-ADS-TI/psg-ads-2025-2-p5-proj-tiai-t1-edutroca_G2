using FluentValidation;

namespace EduTroca.UseCases.Usuarios.ResendEmailConfirmation;
public class ResendEmailConfirmationCommandValidator : AbstractValidator<ResendEmailConfirmationCommand>
{
    public ResendEmailConfirmationCommandValidator()
    {
        RuleFor(x => x.email)
            .NotEmpty().WithMessage("O usuario deve possuir um email.")
            .EmailAddress().WithMessage("O email deve ser um endereço de email valido.");
    }
}
