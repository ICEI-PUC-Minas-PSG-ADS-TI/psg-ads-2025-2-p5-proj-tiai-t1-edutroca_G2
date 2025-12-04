using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using EduTroca.Tests.Helpers;
using EduTroca.UseCases.Usuarios.UpdateEmail;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Usuarios.UpdateEmail;
public class UpdateEmailCommandHandlerTests
{
    private readonly Mock<IRepository<Usuario>> _mockRepository;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly UpdateEmailCommandHandler _handler;

    public UpdateEmailCommandHandlerTests()
    {
        _mockRepository = new Mock<IRepository<Usuario>>();
        _mockEmailService = new Mock<IEmailService>();

        _handler = new UpdateEmailCommandHandler(
            _mockRepository.Object,
            _mockEmailService.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldUpdateEmail_AndRevokeTokens_WhenRequestIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var emailAntigo = "antigo@email.com";
        var novoEmail = "novo@email.com";

        var command = new UpdateEmailCommand(userId, novoEmail);
        var usuario = UsuarioHelpers.CreateUsuario(email: emailAntigo);

        var tokenAtivo = new RefreshToken("token_valido_123", DateTime.UtcNow.AddDays(7));
        usuario.AddRefreshToken(tokenAtivo);

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync(usuario);

        _mockRepository.Setup(r => r.AnyAsync(It.IsAny<UsuarioByEmail>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        usuario.Email.Should().Be(novoEmail);

        tokenAtivo.IsRevoked.Should().BeTrue("todos os tokens devem ser revogados ao trocar o email");
        usuario.RefreshTokens.Should().OnlyContain(t => t.IsRevoked);

        _mockRepository.Verify(r => r.UpdateAsync(usuario), Times.Once);
        _mockEmailService.Verify(s => s.SendConfirmationAsync(usuario), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenNewEmailAlreadyExistsInDatabase()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var novoEmail = "jaexiste@email.com";

        var command = new UpdateEmailCommand(userId, novoEmail);
        var usuario = UsuarioHelpers.CreateUsuario(email: "meu@email.com");

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync(usuario);

        _mockRepository.Setup(r => r.AnyAsync(It.IsAny<UsuarioByEmail>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        result.FirstError.Code.Should().Be("Usuario.Email");
        result.FirstError.Description.Should().Be("Email já cadastrado no banco de dados.");

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Never);
        _mockEmailService.Verify(s => s.SendConfirmationAsync(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenNewEmailIsSameAsCurrent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var emailIgual = "mesmo@email.com";

        var command = new UpdateEmailCommand(userId, emailIgual);
        var usuario = UsuarioHelpers.CreateUsuario(email: emailIgual);

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync(usuario);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Description.Should().Be("Novo email não pode ser igual ao atual.");

        _mockRepository.Verify(r => r.AnyAsync(It.IsAny<UsuarioByEmail>()), Times.Never);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new UpdateEmailCommand(Guid.NewGuid(), "novo@teste.com");

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync((Usuario?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);

        _mockRepository.Verify(r => r.AnyAsync(It.IsAny<UsuarioByEmail>()), Times.Never);
        _mockEmailService.Verify(s => s.SendConfirmationAsync(It.IsAny<Usuario>()), Times.Never);
    }
}
