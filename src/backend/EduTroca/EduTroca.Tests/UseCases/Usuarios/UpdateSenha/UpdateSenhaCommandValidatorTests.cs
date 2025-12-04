using EduTroca.Core.Abstractions;
using EduTroca.Core.Enums;
using EduTroca.UseCases.Usuarios.Create;
using EduTroca.UseCases.Usuarios.UpdateSenha;
using FluentValidation.TestHelper;
using Moq;

namespace EduTroca.Tests.UseCases.Usuarios.UpdateSenha;
public class UpdateSenhaCommandValidatorTests
{
    private readonly Mock<ICurrentUserService> _mockCurrentUser;
    private UpdateSenhaCommandValidator _validator;
    // Instanciamos o validator dentro de cada teste pois o mock muda

    public UpdateSenhaCommandValidatorTests()
    {
        _mockCurrentUser = new Mock<ICurrentUserService>();
        _validator = new UpdateSenhaCommandValidator(_mockCurrentUser.Object);
    }

    private void SetupValidator()
    {
        _validator = new UpdateSenhaCommandValidator(_mockCurrentUser.Object);
    }

    [Fact]
    public void Should_Have_Error_When_SenhaAtual_IsEmpty_And_User_Is_Not_Admin()
    {
        // Arrange: Usuário comum
        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Admin)).Returns(false);
        _mockCurrentUser.Setup(c => c.UserId).Returns(Guid.NewGuid());
        SetupValidator();

        var command = new UpdateSenhaCommand(Guid.NewGuid(), null, "novasenha123");

        // Act
        var result = _validator!.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.senhaAtual)
              .WithErrorMessage("A senha atual deve ser informada para alterar a senha.");
    }

    [Fact]
    public void Should_Pass_When_SenhaAtual_IsEmpty_And_User_Is_Admin_Changing_Other()
    {
        // Arrange: Admin alterando OUTRA pessoa
        var adminId = Guid.NewGuid();
        var targetId = Guid.NewGuid(); // IDs diferentes

        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Admin)).Returns(true);
        _mockCurrentUser.Setup(c => c.UserId).Returns(adminId);
        SetupValidator();

        var command = new UpdateSenhaCommand(targetId, null, "novasenha123");

        // Act
        var result = _validator!.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.senhaAtual);
    }

    [Fact]
    public void Should_Have_Error_When_Admin_Changes_Own_Password_Without_OldPassword()
    {
        // Arrange: Admin alterando A SI MESMO
        var adminId = Guid.NewGuid();

        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Admin)).Returns(true);
        _mockCurrentUser.Setup(c => c.UserId).Returns(adminId);
        SetupValidator();

        // Target ID == Current ID
        var command = new UpdateSenhaCommand(adminId, null, "novasenha123");

        // Act
        var result = _validator!.TestValidate(command);

        // Assert
        // A condição do validator é: !Admin OR (Admin AND Self)
        // Aqui é (Admin AND Self), então entra no bloco e exige senhaAtual
        result.ShouldHaveValidationErrorFor(x => x.senhaAtual);
    }

    [Fact]
    public void Should_Have_Error_When_NewPassword_Is_Same_As_Old()
    {
        // Arrange
        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Admin)).Returns(false);
        SetupValidator();

        var senha = "mesmasenha123";
        var command = new UpdateSenhaCommand(Guid.NewGuid(), senha, senha);

        // Act
        var result = _validator!.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.senhaNova)
              .WithErrorMessage("A nova senha não pode ser igual a senha atual.");
    }
}
