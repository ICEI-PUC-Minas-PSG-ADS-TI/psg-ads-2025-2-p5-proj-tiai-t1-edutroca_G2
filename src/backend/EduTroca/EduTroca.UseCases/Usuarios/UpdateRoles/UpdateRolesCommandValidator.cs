using EduTroca.Core.Enums;
using FluentValidation;

namespace EduTroca.UseCases.Usuarios.UpdateRoles;
public class UpdateRolesCommandValidator : AbstractValidator<UpdateRolesCommand>
{
    public UpdateRolesCommandValidator()
    {
        RuleFor(x => x.rolesIds.Count)
            .GreaterThan(0).WithMessage("São necessarios um ou mais cargos.");

        RuleFor(x => x.rolesIds)
            .Must(ids => ids!.Distinct().Count() == ids!.Count)
            .WithMessage("Não envie cargos duplicados.")
            .Must(ids => !ids!.Contains(ERole.Owner))
            .WithMessage("Não é possível atualizar um usuário para Owner.")
            .Must(ids => ids!.All(id => Enum.IsDefined(typeof(ERole), id)))
            .WithMessage("Um ou mais cargos informados são inválidos.");
    }
}
