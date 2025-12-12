using FluentValidation;

namespace EduTroca.UseCases.Conteudos.Comment;
public class CommentCommandValidator : AbstractValidator<CommentCommand>
{
    public CommentCommandValidator()
    {
        RuleFor(x => x.texto.Length)
            .LessThanOrEqualTo(3000).WithMessage("O comentario deve ter no maximo 3000 caracteres.");
    }
}
