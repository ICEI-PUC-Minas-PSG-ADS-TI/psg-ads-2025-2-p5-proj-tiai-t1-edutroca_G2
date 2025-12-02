using EduTroca.UseCases.Categorias.Create;
using FluentValidation.TestHelper;

namespace EduTroca.Tests.UseCases.Categorias.Create;
public class CreateCategoriaCommandValidatorTests
{
    private readonly CreateCategoriaCommandValidator _validator;

    public CreateCategoriaCommandValidatorTests()
    {
        _validator = new CreateCategoriaCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Nome_Is_Empty()
    {
        var command = new CreateCategoriaCommand("", "Descrição válida");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.nome)
              .WithErrorMessage("'nome' must not be empty.");
    }

    [Fact]
    public void Should_Have_Error_When_Nome_Is_Too_Short()
    {
        var command = new CreateCategoriaCommand("A", "Descrição válida");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.nome);
    }

    [Fact]
    public void Should_Have_Error_When_Nome_Is_Too_Long()
    {
        var nomeGigante = new string('A', 101);
        var command = new CreateCategoriaCommand(nomeGigante, "Descrição válida");

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.nome);
    }

    [Fact]
    public void Should_Have_Error_When_Descricao_Is_Too_Long()
    {
        var descGigante = new string('A', 501);
        var command = new CreateCategoriaCommand("Matemática", descGigante);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.descricao);
    }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        var command = new CreateCategoriaCommand("Física", "Estudo da natureza");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
