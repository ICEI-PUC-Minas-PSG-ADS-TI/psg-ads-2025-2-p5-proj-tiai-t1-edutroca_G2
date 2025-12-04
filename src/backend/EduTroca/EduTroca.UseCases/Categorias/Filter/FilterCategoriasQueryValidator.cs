using FluentValidation;

namespace EduTroca.UseCases.Categorias.Filter;
public class FilterCategoriasQueryValidator : AbstractValidator<FilterCategoriasQuery>
{
    public FilterCategoriasQueryValidator()
    {
        RuleFor(x => x.pagination.PageSize)
            .GreaterThan(0).WithMessage("O numero minimo de itens por página é 1.")
            .LessThan(100).WithMessage("O numero maximo de itens por página é 100.");
    }
}
