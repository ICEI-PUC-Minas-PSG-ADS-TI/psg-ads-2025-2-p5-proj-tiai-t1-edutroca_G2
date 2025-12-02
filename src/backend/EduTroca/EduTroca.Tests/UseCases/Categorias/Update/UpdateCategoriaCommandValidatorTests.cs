using EduTroca.UseCases.Categorias.Update;
using FluentValidation.TestHelper;

namespace EduTroca.Tests.UseCases.Categorias.Update;
public class UpdateCategoriaCommandValidatorTests
{
    private readonly UpdateCategoriaCommandValidator _validator;

    public UpdateCategoriaCommandValidatorTests()
    {
        _validator = new UpdateCategoriaCommandValidator();
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")]
    public void Should_Have_Error_When_Nome_Is_Invalid(string invalidName)
    {
        var command = new UpdateCategoriaCommand(Guid.NewGuid(), invalidName, "Desc");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.nome);
    }

    [Fact]
    public void Should_Have_Error_When_Nome_Is_Too_Long()
    {
        var nomeGigante = new string('A', 101);
        var command = new UpdateCategoriaCommand(Guid.NewGuid(), nomeGigante, "Desc");

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.nome)
              .WithErrorMessage("O nome deve ter no máximo 100 caracteres.");
    }

    [Fact]
    public void Should_Have_Error_When_Descricao_Is_Too_Long()
    {
        var descGigante = new string('B', 501);
        var command = new UpdateCategoriaCommand(Guid.NewGuid(), "Nome Válido", descGigante);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.descricao)
              .WithErrorMessage("A descrição não pode exceder 500 caracteres.");
    }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        var command = new UpdateCategoriaCommand(Guid.NewGuid(), "Matemática", "Aulas de calculo");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
