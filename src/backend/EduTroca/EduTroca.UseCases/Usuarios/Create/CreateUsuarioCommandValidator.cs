using EduTroca.Core.Enums;
using FluentValidation;

namespace EduTroca.UseCases.Usuarios.Create;
public class CreateUsuarioCommandValidator : AbstractValidator<CreateUsuarioCommand>
{
    public CreateUsuarioCommandValidator()
    {
        RuleFor(x => x.nome)
            .NotEmpty().WithMessage("O usuario deve possuir um nome.")
            .MinimumLength(3).WithMessage("O nome deve possuir no minimo 3 caracteres.")
            .MaximumLength(100).WithMessage("O nome deve possuir no maximo 100 caracteres.");

        RuleFor(x => x.email)
            .NotEmpty().WithMessage("O usuario deve possuir um email.")
            .EmailAddress().WithMessage("O email deve ser um endereço de email valido.");

        RuleFor(x => x.senha)
            .NotEmpty().WithMessage("O usuario deve possuir uma senha.")
            .MinimumLength(6).WithMessage("A senha deve possuir no minimo 6 caracteres.")
            .MaximumLength(100).WithMessage("A senha deve possuir no maximo 100 caracteres.");

        When(x => x.rolesIds is not null && x.rolesIds.Count > 0, () =>
        {
            RuleFor(x => x.rolesIds)
                .Must(ids => ids!.Distinct().Count() == ids!.Count)
                .WithMessage("Não envie cargos duplicados.");

            RuleFor(x => x.rolesIds)
                .Must(ids => !ids!.Contains(ERole.Owner))
                .WithMessage("Não é possível criar um usuário Owner.");

            RuleFor(x => x.rolesIds)
                .Must(ids => ids!.All(id => Enum.IsDefined(typeof(ERole), id)))
                .WithMessage("Um ou mais cargos informados são inválidos.");
        });
    }
}
