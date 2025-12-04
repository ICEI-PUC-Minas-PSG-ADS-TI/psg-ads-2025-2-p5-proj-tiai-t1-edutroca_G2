using EduTroca.UseCases.Common.DTOs;
using EduTroca.UseCases.Usuarios.Filter;
using FluentValidation.TestHelper;

namespace EduTroca.Tests.UseCases.Usuarios.Filter;
public class FilterUsuariosQueryValidatorTests
{
    private readonly FilterUsuariosQueryValidator _validator;

    public FilterUsuariosQueryValidatorTests()
    {
        _validator = new FilterUsuariosQueryValidator();
    }

    [Fact]
    public void Should_Pass_When_Pagination_Is_Valid()
    {
        var pagination = new PaginationDTO(1, 20);
        var query = new FilterUsuariosQuery("Teste", null, pagination);

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Should_Fail_When_PageSize_Is_Zero_Or_Negative(int invalidSize)
    {
        var pagination = new PaginationDTO(1, invalidSize);
        var query = new FilterUsuariosQuery("Teste", null, pagination);

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.pagination.PageSize)
              .WithErrorMessage("O numero minimo de itens por página é 1.");
    }

    [Theory]
    [InlineData(100)]
    [InlineData(101)]
    [InlineData(500)]
    public void Should_Fail_When_PageSize_Is_Too_Large(int invalidSize)
    {
        var pagination = new PaginationDTO(1, invalidSize);
        var query = new FilterUsuariosQuery("Teste", null, pagination);

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.pagination.PageSize)
              .WithErrorMessage("O numero maximo de itens por página é 100.");
    }
}
