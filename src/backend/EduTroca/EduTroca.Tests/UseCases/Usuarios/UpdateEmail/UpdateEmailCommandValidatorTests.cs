using EduTroca.UseCases.Usuarios.UpdateEmail;
using FluentValidation.TestHelper;

namespace EduTroca.Tests.UseCases.Usuarios.UpdateEmail;
public class UpdateEmailCommandValidatorTests
{
    private readonly UpdateEmailCommandValidator _validator;

    public UpdateEmailCommandValidatorTests()
    {
        _validator = new UpdateEmailCommandValidator();
    }

    [Fact]
    public void Should_Pass_When_NewEmail_Is_Valid()
    {
        var command = new UpdateEmailCommand(Guid.NewGuid(), "valido@email.com");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_When_NewEmail_Is_Empty()
    {
        var command = new UpdateEmailCommand(Guid.NewGuid(), "");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.novoEmail)
              .WithErrorMessage("O usuario deve possuir um email.");
    }

    [Theory]
    [InlineData("emailinvalido")]
    [InlineData("teste@")]
    [InlineData("@teste.com")]
    public void Should_Have_Error_When_NewEmail_Format_Is_Invalid(string invalidEmail)
    {
        var command = new UpdateEmailCommand(Guid.NewGuid(), invalidEmail);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.novoEmail)
              .WithErrorMessage("O email deve ser um endereço de email valido.");
    }
}
