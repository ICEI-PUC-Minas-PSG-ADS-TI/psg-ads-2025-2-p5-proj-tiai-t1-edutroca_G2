using FluentValidation;

namespace EduTroca.UseCases.Usuarios.UpdateEmail;
public class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
{
    public UpdateEmailCommandValidator()
    {
        RuleFor(x => x.novoEmail)
            .NotEmpty().WithMessage("O usuario deve possuir um email.")
            .EmailAddress().WithMessage("O email deve ser um endereço de email valido.");
    }
}
