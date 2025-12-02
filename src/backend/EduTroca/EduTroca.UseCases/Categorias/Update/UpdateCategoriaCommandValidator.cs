using FluentValidation;

namespace EduTroca.UseCases.Categorias.Update;
public class UpdateCategoriaCommandValidator : AbstractValidator<UpdateCategoriaCommand>
{
    public UpdateCategoriaCommandValidator()
    {
        RuleFor(x => x.nome).NotEmpty()
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.")
            .MinimumLength(2).WithMessage("O nome deve ter no minimo 2 caracteres.");
        RuleFor(x => x.descricao)
            .MaximumLength(500).WithMessage("A descrição não pode exceder 500 caracteres.");
    }
}
