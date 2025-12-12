using FluentValidation;

namespace EduTroca.UseCases.Categorias.Create;
public class CreateCategoriaCommandValidator : AbstractValidator<CreateCategoriaCommand>
{
    public CreateCategoriaCommandValidator()
    {
        RuleFor(x => x.nome).NotEmpty()
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.")
            .MinimumLength(2).WithMessage("O nome deve ter no minimo 2 caracteres.");
        RuleFor(x => x.descricao)
            .MaximumLength(500).WithMessage("A descrição não pode exceder 500 caracteres.");
    }
}
