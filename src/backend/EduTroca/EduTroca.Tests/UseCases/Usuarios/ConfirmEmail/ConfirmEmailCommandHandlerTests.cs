using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using EduTroca.Tests.Helpers;
using EduTroca.UseCases.Usuarios.ConfirmEmail;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Usuarios.ConfirmEmail;
public class ConfirmEmailCommandHandlerTests
{
    private readonly Mock<IRepository<Usuario>> _mockRepository;
    private readonly ConfirmEmailCommandHandler _handler;

    public ConfirmEmailCommandHandlerTests()
    {
        _mockRepository = new Mock<IRepository<Usuario>>();
        _handler = new ConfirmEmailCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldConfirmEmail_WhenCodeIsValid_AndNotExpired()
    {
        // Arrange
        var codeId = Guid.NewGuid();
        var command = new ConfirmEmailCommand(codeId);

        var usuario = UsuarioHelpers.CreateUsuario(emailConfirmationExpiresOnUtc: DateTime.UtcNow.AddHours(1));

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioByEmailConfirmationId>()))
            .ReturnsAsync(usuario);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        usuario.IsConfirmed.Should().BeTrue();

        _mockRepository.Verify(r => r.UpdateAsync(usuario), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCodeDoesNotExist()
    {
        // Arrange
        var command = new ConfirmEmailCommand(Guid.NewGuid());

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioByEmailConfirmationId>()))
            .ReturnsAsync((Usuario?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Code.Should().Be("Usuario.NotFound");

        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenUserAlreadyConfirmed()
    {
        // Arrange
        var command = new ConfirmEmailCommand(Guid.NewGuid());

        var usuario = UsuarioHelpers.CreateUsuario(emailConfirmationExpiresOnUtc: DateTime.UtcNow.AddHours(-1));
        usuario.ConfirmEmail();

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioByEmailConfirmationId>()))
            .ReturnsAsync(usuario);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        result.FirstError.Code.Should().Be("Usuario.IsConfirmed");
        result.FirstError.Description.Should().Be("Email de usuario ja confirmado.");

        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenCodeIsExpired()
    {
        // Arrange
        var command = new ConfirmEmailCommand(Guid.NewGuid());

        var usuario = UsuarioHelpers.CreateUsuario(emailConfirmationExpiresOnUtc: DateTime.UtcNow.AddHours(-1));

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioByEmailConfirmationId>()))
            .ReturnsAsync(usuario);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        result.FirstError.Code.Should().Be("Usuario.Expired");
        result.FirstError.Description.Should().Be("Email de confirmação expirado.");

        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}
