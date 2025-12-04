using EduTroca.UseCases.Usuarios.ResendEmailConfirmation;
using FluentValidation.TestHelper;

namespace EduTroca.Tests.UseCases.Usuarios.ResendEmailConfirmation;
public class ResendEmailConfirmationCommandValidatorTests
{
    private readonly ResendEmailConfirmationCommandValidator _validator;

    public ResendEmailConfirmationCommandValidatorTests()
    {
        _validator = new ResendEmailConfirmationCommandValidator();
    }

    [Fact]
    public void Should_Pass_When_Email_Is_Valid()
    {
        var command = new ResendEmailConfirmationCommand("valido@email.com");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var command = new ResendEmailConfirmationCommand("");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.email)
              .WithErrorMessage("O usuario deve possuir um email.");
    }

    [Theory]
    [InlineData("email-sem-arroba")]
    [InlineData("email@com@doisarrobas")]
    [InlineData("apenastexto")]
    public void Should_Have_Error_When_Format_Is_Invalid(string invalidEmail)
    {
        var command = new ResendEmailConfirmationCommand(invalidEmail);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.email)
              .WithErrorMessage("O email deve ser um endereço de email valido.");
    }
}
