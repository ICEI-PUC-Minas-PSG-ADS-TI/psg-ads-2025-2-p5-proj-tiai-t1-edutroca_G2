using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using EduTroca.Tests.Helpers;
using EduTroca.UseCases.Usuarios.Login;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Usuarios.Login;
public class LoginCommandHandlerTests
{
    private readonly Mock<IRepository<Usuario>> _mockUsuarioRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _mockUsuarioRepository = new Mock<IRepository<Usuario>>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();

        _handler = new LoginCommandHandler(
            _mockUsuarioRepository.Object,
            _mockPasswordHasher.Object,
            _mockJwtTokenGenerator.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnLoginDTO_WhenCredentialsAreValid_AndEmailConfirmed()
    {
        // Arrange
        var command = new LoginCommand("teste@email.com", "senha123");

        var usuario = UsuarioHelpers.CreateUsuario();
        usuario.ConfirmEmail();

        _mockUsuarioRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioByEmail>()))
            .ReturnsAsync(usuario);

        _mockPasswordHasher.Setup(p => p.VerifyPassword(command.senha, usuario.SenhaHash))
            .Returns(true);

        var expectedAccessToken = "access_token_valido";
        _mockJwtTokenGenerator.Setup(j => j.GenerateAccessToken(usuario))
            .Returns(expectedAccessToken);
        var expectedRefreshToken = new RefreshToken("refresh_token_valido", DateTime.UtcNow.AddDays(1));
        _mockJwtTokenGenerator.Setup(j => j.GenerateRefreshToken())
            .Returns(expectedRefreshToken);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.AccessToken.Should().Be(expectedAccessToken);
        result.Value.RefreshToken.Should().NotBeNullOrEmpty();

        usuario.RefreshTokens.Should().HaveCount(1);

        _mockUsuarioRepository.Verify(r => r.UpdateAsync(usuario), Times.Once);
        _mockUsuarioRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUsuarioDoesNotExist()
    {
        // Arrange
        var command = new LoginCommand("inexistente@email.com", "senha123");

        _mockUsuarioRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioByEmail>()))
            .ReturnsAsync((Usuario?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Description.Should().Contain("Email e/ou senha incorreta");

        _mockJwtTokenGenerator.Verify(j => j.GenerateAccessToken(It.IsAny<Usuario>()), Times.Never);
        _mockUsuarioRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenPasswordIsInvalid()
    {
        // Arrange
        var command = new LoginCommand("teste@email.com", "senha_errada");
        var usuario = UsuarioHelpers.CreateUsuario();

        _mockUsuarioRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioByEmail>()))
            .ReturnsAsync(usuario);

        _mockPasswordHasher.Setup(p => p.VerifyPassword(command.senha, usuario.SenhaHash))
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Description.Should().Contain("Email e/ou senha incorreta");

        _mockJwtTokenGenerator.Verify(j => j.GenerateAccessToken(It.IsAny<Usuario>()), Times.Never);
        _mockUsuarioRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenEmailIsNotConfirmed()
    {
        // Arrange
        var command = new LoginCommand("teste@email.com", "senha123");

        var usuario = UsuarioHelpers.CreateUsuario();

        _mockUsuarioRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioByEmail>()))
            .ReturnsAsync(usuario);

        _mockPasswordHasher.Setup(p => p.VerifyPassword(command.senha, usuario.SenhaHash))
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Forbidden);
        result.FirstError.Code.Should().Be("Usuario.NotConfirmedEmail");

        _mockUsuarioRepository.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Never);
        _mockUsuarioRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}
