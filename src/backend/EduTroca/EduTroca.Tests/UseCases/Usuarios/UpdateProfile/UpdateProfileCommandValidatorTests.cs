using EduTroca.UseCases.Common.DTOs;
using EduTroca.UseCases.Usuarios.UpdateProfile;
using FluentValidation.TestHelper;

namespace EduTroca.Tests.UseCases.Usuarios.UpdateProfile;
public class UpdateProfileCommandValidatorTests
{
    private readonly UpdateProfileCommandValidator _validator;

    public UpdateProfileCommandValidatorTests()
    {
        _validator = new UpdateProfileCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_SendingPicture_AND_DeleteFlagTrue()
    {
        // Arrange
        var stream = new MemoryStream();
        var picture = new PictureDTO(stream, "image/jpeg", 100);

        var command = new UpdateProfileCommand(Guid.NewGuid(), "Nome", "Bio", picture, DeletePicture: true);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
              .WithErrorMessage("Não é possível enviar uma nova foto e excluir a antiga simultaneamente.");
    }

    [Fact]
    public void Should_Have_Error_When_Picture_Format_Is_Invalid()
    {
        // Arrange
        var stream = new MemoryStream();
        var picture = new PictureDTO(stream, "application/pdf", 100);

        var command = new UpdateProfileCommand(Guid.NewGuid(), "Nome", "Bio", picture, DeletePicture: false);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.picture);
    }

    [Fact]
    public void Should_Pass_When_Updating_Only_Profile_Info()
    {
        // Arrange
        var command = new UpdateProfileCommand(Guid.NewGuid(), "Nome Válido", "Bio Válida", null, false);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
