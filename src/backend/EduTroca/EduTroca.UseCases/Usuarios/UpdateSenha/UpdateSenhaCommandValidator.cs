using EduTroca.Core.Abstractions;
using EduTroca.Core.Enums;
using FluentValidation;

namespace EduTroca.UseCases.Usuarios.UpdateSenha;
public class UpdateSenhaCommandValidator : AbstractValidator<UpdateSenhaCommand>
{
    public UpdateSenhaCommandValidator(ICurrentUserService currentUser)
    {
        RuleFor(x => x.senhaNova)
            .NotEmpty().WithMessage("A nova senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve possuir no minimo 6 caracteres.")
            .MaximumLength(100).WithMessage("A senha deve possuir no maximo 100 caracteres.")
            .NotEqual(x => x.senhaAtual).WithMessage("A nova senha não pode ser igual a senha atual.");
        When(x => !currentUser.IsInRole(ERole.Admin) || x.usuarioId == currentUser.UserId, () =>
        {
            RuleFor(x => x.senhaAtual)
                .NotEmpty().WithMessage("A senha atual deve ser informada para alterar a senha.");
        });
    }
}
