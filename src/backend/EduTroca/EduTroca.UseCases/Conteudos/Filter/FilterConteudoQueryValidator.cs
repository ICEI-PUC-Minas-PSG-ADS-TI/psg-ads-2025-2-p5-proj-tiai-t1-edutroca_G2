using EduTroca.Core.Common;
using FluentValidation;

namespace EduTroca.UseCases.Conteudos.Filter;
public class FilterConteudoQueryValidator : AbstractValidator<FilterConteudoQuery>
{
    public FilterConteudoQueryValidator()
    {
        RuleFor(x => x.Titulo)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Titulo))
            .WithMessage("O título não pode ter mais de 200 caracteres.");

        RuleFor(x => x.Visualizacoes)
            .Must(BeValidRange)
            .When(x => x.Visualizacoes is not null)
            .WithMessage("O intervalo de visualizações é inválido.");

        RuleFor(x => x.Visualizacoes!.Min)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Visualizacoes?.Min.HasValue == true)
            .WithMessage("O valor mínimo de visualizações não pode ser negativo.");

        RuleFor(x => x.Visualizacoes!.Max)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Visualizacoes?.Max.HasValue == true)
            .WithMessage("O valor máximo de visualizações não pode ser negativo.");

        RuleFor(x => x.Likes)
            .Must(BeValidRange)
            .When(x => x.Likes is not null)
            .WithMessage("O intervalo de likes é inválido.");

        RuleFor(x => x.Likes!.Min)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Likes?.Min.HasValue == true)
            .WithMessage("O valor mínimo de likes não pode ser negativo.");

        RuleFor(x => x.Likes!.Max)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Likes?.Max.HasValue == true)
            .WithMessage("O valor máximo de likes não pode ser negativo.");

        RuleFor(x => x.Periodo)
            .Must(BeValidDateRange)
            .When(x => x.Periodo is not null)
            .WithMessage("O período de datas é inválido.");

        RuleFor(x => x.Periodo!.From)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(x => x.Periodo?.From.HasValue == true)
            .WithMessage("A data inicial não pode ser no futuro.");

        RuleFor(x => x.Periodo!.To)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(x => x.Periodo?.To.HasValue == true)
            .WithMessage("A data final não pode ser no futuro.");

        RuleFor(x => x.CategoriasIds)
            .Must(ids => ids!.Count <= 10)
            .When(x => x.CategoriasIds is not null && x.CategoriasIds.Any())
            .WithMessage("Você pode filtrar por no máximo 10 categorias.");

        RuleFor(x => x.CategoriasIds)
            .Must(ids => ids!.Distinct().Count() == ids.Count)
            .When(x => x.CategoriasIds is not null && x.CategoriasIds.Any())
            .WithMessage("IDs de categorias duplicados não são permitidos.");

        RuleFor(x => x.Pagination)
            .NotNull()
            .WithMessage("Informações de paginação são obrigatórias.");

        RuleFor(x => x.Pagination.PageNumber)
            .GreaterThan(0)
            .WithMessage("O número da página deve ser maior que zero.");

        RuleFor(x => x.Pagination.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("O tamanho da página deve estar entre 1 e 100.");
    }

    private bool BeValidRange<T>(RangeFilter<T>? range) where T : struct, IComparable<T>
    {
        return range?.IsValid() ?? true;
    }

    private bool BeValidDateRange(DateRangeFilter? range)
    {
        return range?.IsValid() ?? true;
    }
}
