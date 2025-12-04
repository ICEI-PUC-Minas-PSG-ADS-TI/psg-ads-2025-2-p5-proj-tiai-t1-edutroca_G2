using EduTroca.Core.Enums;
using EduTroca.UseCases.Usuarios.Create;
using FluentValidation.TestHelper;

namespace EduTroca.Tests.UseCases.Usuarios.Create;
public class CreateUsuarioCommandValidatorTests
{
    private readonly CreateUsuarioCommandValidator _validator;

    public CreateUsuarioCommandValidatorTests()
    {
        _validator = new CreateUsuarioCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Nome_Is_Empty()
    {
        var command = new CreateUsuarioCommand("", "email@teste.com", "123456");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.nome);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var command = new CreateUsuarioCommand("Nome Valido", "email-invalido", "123456");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.email)
              .WithErrorMessage("O email deve ser um endereço de email valido.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreateUsuarioCommand("Nome Correto", "teste@email.com", "senhaForte123");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
    [Fact]
    public void Should_Have_Error_When_Roles_Contain_Duplicates()
    {
        // Duas vezes Admin
        var roles = new List<ERole> { ERole.Admin, ERole.Admin };
        var command = new CreateUsuarioCommand("Valid", "valid@e.com", "123456", roles);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.rolesIds)
              .WithErrorMessage("Não envie cargos duplicados.");
    }

    [Fact]
    public void Should_Have_Error_When_Roles_Contain_Owner()
    {
        var roles = new List<ERole> { ERole.Owner };
        var command = new CreateUsuarioCommand("Valid", "valid@e.com", "123456", roles);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.rolesIds)
              .WithErrorMessage("Não é possível criar um usuário Owner.");
    }

    [Fact]
    public void Should_Have_Error_When_Roles_Contain_Invalid_Enum_Value()
    {
        // Cast forçado de um inteiro que não existe no Enum (ex: 999)
        var roles = new List<ERole> { (ERole)999 };
        var command = new CreateUsuarioCommand("Valid", "valid@e.com", "123456", roles);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.rolesIds)
              .WithErrorMessage("Um ou mais cargos informados são inválidos.");
    }

    [Fact]
    public void Should_Pass_When_Roles_Are_Valid()
    {
        var roles = new List<ERole> { ERole.Admin, ERole.User };
        var command = new CreateUsuarioCommand("Valid", "valid@e.com", "123456", roles);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
