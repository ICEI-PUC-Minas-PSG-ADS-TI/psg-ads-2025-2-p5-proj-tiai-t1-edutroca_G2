using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using EduTroca.Tests.Helpers;
using EduTroca.UseCases.Usuarios.DTOs;
using EduTroca.UseCases.Usuarios.Refresh;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Usuarios.Refresh;
public class RefreshTokenCommandHandlerTests
{
    private readonly Mock<IRepository<Usuario>> _mockRepository;
    private readonly Mock<IJwtTokenGenerator> _mockJwtGenerator;
    private readonly RefreshTokenCommandHandler _handler;

    public RefreshTokenCommandHandlerTests()
    {
        _mockRepository = new Mock<IRepository<Usuario>>();
        _mockJwtGenerator = new Mock<IJwtTokenGenerator>();

        _handler = new RefreshTokenCommandHandler(
            _mockRepository.Object,
            _mockJwtGenerator.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldRotateTokens_WhenTokenIsValid()
    {
        // Arrange
        var oldTokenString = "token_velho_valido";
        var command = new RefreshTokenCommand(oldTokenString);

        var usuario = UsuarioHelpers.CreateUsuario();
        var oldRefreshToken = CreateRefreshToken(oldTokenString, isExpired: false, isRevoked: false);
        usuario.AddRefreshToken(oldRefreshToken);

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioByRefreshToken>()))
            .ReturnsAsync(usuario);

        var newTokenEntity = CreateRefreshToken("token_novo_123", false, false);
        _mockJwtGenerator.Setup(j => j.GenerateRefreshToken()).Returns(newTokenEntity);
        _mockJwtGenerator.Setup(j => j.GenerateAccessToken(usuario)).Returns("novo_access_token_jwt");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<LoginDTO>();

        oldRefreshToken.IsRevoked.Should().BeTrue("o token antigo deve ser revogado após o uso");

        usuario.RefreshTokens.Should().Contain(newTokenEntity);

        result.Value.AccessToken.Should().Be("novo_access_token_jwt");
        result.Value.RefreshToken.Should().Be("token_novo_123");

        _mockRepository.Verify(r => r.UpdateAsync(usuario), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenUserOrTokenNotFound()
    {
        // Arrange
        var command = new RefreshTokenCommand("token_inexistente");

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioByRefreshToken>()))
            .ReturnsAsync((Usuario?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unauthorized);
        result.FirstError.Code.Should().Be("RefreshToken.Invalid");

        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenTokenIsExpired()
    {
        // Arrange
        var tokenString = "token_expirado";
        var command = new RefreshTokenCommand(tokenString);

        var usuario = UsuarioHelpers.CreateUsuario();
        var tokenExpirado = CreateRefreshToken(tokenString, isExpired: true, isRevoked: false);
        usuario.AddRefreshToken(tokenExpirado);

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioByRefreshToken>()))
            .ReturnsAsync(usuario);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unauthorized);
        result.FirstError.Code.Should().Be("Token.Expired");

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenTokenIsRevoked()
    {
        // Arrange
        var tokenString = "token_roubado";
        var command = new RefreshTokenCommand(tokenString);

        var usuario = UsuarioHelpers.CreateUsuario();
        var tokenRevogado = CreateRefreshToken(tokenString, isExpired: false, isRevoked: true);
        usuario.AddRefreshToken(tokenRevogado);

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioByRefreshToken>()))
            .ReturnsAsync(usuario);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unauthorized);
        result.FirstError.Code.Should().Be("Token.Revoked");
        result.FirstError.Description.Should().Contain("Tentativa de reuso");

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Never);
    }

    private RefreshToken CreateRefreshToken(string tokenStr, bool isExpired, bool isRevoked)
    {
        var expires = isExpired ? DateTime.UtcNow.AddDays(-1) : DateTime.UtcNow.AddDays(1);

        var token = new RefreshToken(tokenStr, expires);

        if (isRevoked)
            token.Revoke();

        return token;
    }
}
