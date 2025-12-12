using FluentValidation;

namespace EduTroca.UseCases.Conteudos.Videos.Update;
public class UpdateVideoCommandValidator : AbstractValidator<UpdateVideoCommand>
{
    private const long MaxImageSize = 5L * 1024 * 1024;
    public UpdateVideoCommandValidator()
    {
        RuleFor(x => x.titulo)
            .NotEmpty().WithMessage("O vídeo deve possuir um título.")
            .Length(5, 100).WithMessage("O título deve ter entre 5 e 100 caracteres.");

        RuleFor(x => x.descricao)
            .MaximumLength(3000).WithMessage("A descrição não pode exceder 3000 caracteres.");

        RuleFor(x => x.imagem)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("A imagem de capa é obrigatória.")
            .Must(p => p.ContentType.StartsWith("image/"))
                .WithMessage("O arquivo deve ser uma imagem válida.")
            .Must(p => p.Length > 0 && p.Length <= MaxImageSize)
                .WithMessage("A imagem excede o limite de 5MB.");
    }
}
