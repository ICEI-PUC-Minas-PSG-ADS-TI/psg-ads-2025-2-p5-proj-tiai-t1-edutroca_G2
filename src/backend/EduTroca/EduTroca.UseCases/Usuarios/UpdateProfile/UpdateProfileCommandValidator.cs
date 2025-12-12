using FluentValidation;

namespace EduTroca.UseCases.Usuarios.UpdateProfile;
public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    private const long MaxFileSizeInBytes = 10 * 1024 * 1024;
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.nome)
            .NotEmpty().WithMessage("O usuario deve possuir um nome.")
            .MinimumLength(3).WithMessage("O nome deve possuir no minimo 3 caracteres.")
            .MaximumLength(100).WithMessage("O nome deve possuir no maximo 100 caracteres.");

        RuleFor(x => x.bio)
            .MaximumLength(300).WithMessage("A bio não pode exceder 300 caracteres.");

        RuleFor(x => x.picture)
            .Must(p => p == null || p.ContentType.StartsWith("image/"))
            .WithMessage("Arquivo deve ser uma imagem.")
            .Must(p => p == null || (p.Length > 0 && p.Length < MaxFileSizeInBytes))
            .WithMessage($"O arquivo excede o limite de {MaxFileSizeInBytes / 1024 / 1024} MB.");

        RuleFor(x => x)
            .Must(x => !(x.picture is not null && x.DeletePicture))
            .WithMessage("Não é possível enviar uma nova foto e excluir a antiga simultaneamente.");
    }
}
