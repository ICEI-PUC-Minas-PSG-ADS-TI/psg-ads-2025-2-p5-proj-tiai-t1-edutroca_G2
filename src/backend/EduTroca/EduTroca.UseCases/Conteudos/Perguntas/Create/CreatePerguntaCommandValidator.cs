using FluentValidation;

namespace EduTroca.UseCases.Conteudos.Perguntas.Create;
public class CreatePerguntaCommandValidator : AbstractValidator<CreatePerguntaCommand>
{
    public CreatePerguntaCommandValidator()
    {
        RuleFor(x => x.titulo)
            .NotEmpty().WithMessage("O vídeo deve possuir um título.")
            .Length(5, 100).WithMessage("O título deve ter entre 5 e 100 caracteres.");

        RuleFor(x => x.descricao)
            .MaximumLength(3000).WithMessage("A descrição não pode exceder 3000 caracteres.");

        RuleFor(x => x.texto)
            .MaximumLength(5000).WithMessage("A pergunta não pode exceder 5000 caracteres.");
    }
}
