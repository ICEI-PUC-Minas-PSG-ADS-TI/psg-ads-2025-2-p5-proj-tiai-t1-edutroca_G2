using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;
using EduTroca.Core.Specifications;
using EduTroca.Tests.Helpers;
using EduTroca.UseCases.Usuarios.ResendEmailConfirmation;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Usuarios.ResendEmailConfirmation;
public class ResendEmailConfirmationCommandHandlerTests
{
    private readonly Mock<IRepository<Usuario>> _mockRepository;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly ResendEmailConfirmationCommandHandler _handler;

    public ResendEmailConfirmationCommandHandlerTests()
    {
        _mockRepository = new Mock<IRepository<Usuario>>();
        _mockEmailService = new Mock<IEmailService>();

        _handler = new ResendEmailConfirmationCommandHandler(
            _mockRepository.Object,
            _mockEmailService.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldRegenerateCode_AndSendEmail_WhenUserExists_And_NotConfirmed()
    {
        // Arrange
        var email = "teste@email.com";
        var command = new ResendEmailConfirmationCommand(email);

        var usuario = UsuarioHelpers.CreateUsuario(isConfirmed: false);

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioByEmail>()))
            .ReturnsAsync(usuario);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        usuario.EmailConfirmationCode.ExpiresOnUtc.Should().BeAfter(DateTime.UtcNow);

        _mockRepository.Verify(r => r.UpdateAsync(usuario), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        _mockEmailService.Verify(s => s.SendConfirmationAsync(usuario), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new ResendEmailConfirmationCommand("inexistente@email.com");

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioByEmail>()))
            .ReturnsAsync((Usuario?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);

        _mockEmailService.Verify(s => s.SendConfirmationAsync(It.IsAny<Usuario>()), Times.Never);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenUserAlreadyConfirmed()
    {
        // Arrange
        var command = new ResendEmailConfirmationCommand("ja@confirmado.com");
        var usuario = UsuarioHelpers.CreateUsuario(isConfirmed: true);

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioByEmail>()))
            .ReturnsAsync(usuario);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);

        _mockEmailService.Verify(s => s.SendConfirmationAsync(It.IsAny<Usuario>()), Times.Never);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Never);
    }
}
