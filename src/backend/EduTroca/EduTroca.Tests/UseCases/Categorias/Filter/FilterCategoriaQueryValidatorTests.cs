using EduTroca.UseCases.Categorias.Filter;
using EduTroca.UseCases.Common.DTOs;
using FluentValidation.TestHelper;

namespace EduTroca.Tests.UseCases.Categorias.Filter;
public class FilterCategoriaQueryValidatorTests
{
    private readonly FilterCategoriaQueryValidator _validator;

    public FilterCategoriaQueryValidatorTests()
    {
        _validator = new FilterCategoriaQueryValidator();
    }

    [Fact]
    public void Should_Pass_When_PageSize_Is_Valid()
    {
        // Arrange
        var pagination = new PaginationDTO(1,20);
        var query = new FilterCategoriasQuery("Teste", pagination);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Should_Fail_When_PageSize_Is_Zero_Or_Negative(int invalidSize)
    {
        // Arrange
        var pagination = new PaginationDTO(1, invalidSize);
        var query = new FilterCategoriasQuery("Teste", pagination);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.pagination.PageSize)
              .WithErrorMessage("O numero minimo de itens por página é 1.");
    }

    [Theory]
    [InlineData(100)]
    [InlineData(101)]
    [InlineData(500)]
    public void Should_Fail_When_PageSize_Is_Too_Large(int invalidSize)
    {
        // Arrange
        var pagination = new PaginationDTO (1, invalidSize);
        var query = new FilterCategoriasQuery("Teste", pagination);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.pagination.PageSize)
              .WithErrorMessage("O numero maximo de itens por página é 100.");
    }
}
