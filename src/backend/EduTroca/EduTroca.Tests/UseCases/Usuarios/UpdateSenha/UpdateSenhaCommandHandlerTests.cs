using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;
using EduTroca.Core.Specifications;
using EduTroca.Tests.Helpers;
using EduTroca.UseCases.Usuarios.UpdateSenha;
using ErrorOr;
using FluentAssertions;
using Moq;

public class UpdateSenhaCommandHandlerTests
{
    private readonly Mock<IRepository<Usuario>> _mockRepository;
    private readonly Mock<IPasswordHasher> _mockHasher;
    private readonly Mock<ICurrentUserService> _mockCurrentUser;
    private readonly UpdateSenhaCommandHandler _handler;

    public UpdateSenhaCommandHandlerTests()
    {
        _mockRepository = new Mock<IRepository<Usuario>>();
        _mockHasher = new Mock<IPasswordHasher>();
        _mockCurrentUser = new Mock<ICurrentUserService>();

        _handler = new UpdateSenhaCommandHandler(
            _mockHasher.Object,
            _mockRepository.Object,
            _mockCurrentUser.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldUpdatePassword_AndRevokeTokens_WhenUserIsSelf_AndOldPasswordIsCorrect()
    {
        // Arrange
        var senhaAntiga = "senha123";
        var senhaNova = "novaSenha123";
        // Cria usuário com hash conhecido
        var usuario = UsuarioHelpers.CreateUsuario(senhaHash: "hashAntigo");

        // Adiciona um token ativo para testar a revogação
        // Assumindo que seu RefreshToken tem esse construtor ou similar
        usuario.AddRefreshToken(new RefreshToken("token_ativo", DateTime.UtcNow.AddDays(1)));

        var command = new UpdateSenhaCommand(usuario.Id, senhaAntiga, senhaNova);

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync(usuario);

        // Não é Admin, então DEVE verificar a senha
        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Admin)).Returns(false);

        _mockHasher.Setup(h => h.VerifyPassword(senhaAntiga, "hashAntigo")).Returns(true);
        _mockHasher.Setup(h => h.HashPassword(senhaNova)).Returns("hashNovo");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        // Verifica atualização da senha
        usuario.SenhaHash.Should().Be("hashNovo");

        // Verifica revogação dos tokens
        usuario.RefreshTokens.Should().OnlyContain(t => t.IsRevoked);

        // Verifica persistência
        _mockRepository.Verify(r => r.UpdateAsync(usuario), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnValidation_WhenOldPasswordIsIncorrect()
    {
        // Arrange
        var usuario = UsuarioHelpers.CreateUsuario(senhaHash: "hashCorreto");
        var command = new UpdateSenhaCommand(usuario.Id, "senhaErrada", "novaSenha");

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>())).ReturnsAsync(usuario);

        // Não é Admin -> Trigger na verificação
        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Admin)).Returns(false);

        // Simula falha na verificação
        _mockHasher.Setup(h => h.VerifyPassword("senhaErrada", "hashCorreto")).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        result.FirstError.Code.Should().Be("Senha.Incorreta");

        // Garante que não salvou
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldSkipVerification_WhenUserIsAdmin()
    {
        // Arrange
        var usuario = UsuarioHelpers.CreateUsuario(senhaHash: "hashQualquer");

        // Admin trocando senha de outro (manda null na senha antiga)
        var command = new UpdateSenhaCommand(usuario.Id, null, "novaSenhaAdmin");

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>())).ReturnsAsync(usuario);

        // É Admin -> Pula verificação
        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Admin)).Returns(true);
        _mockHasher.Setup(h => h.HashPassword("novaSenhaAdmin")).Returns("hashAdmin");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        usuario.SenhaHash.Should().Be("hashAdmin");

        // Garante que VerifyPassword NUNCA foi chamado
        _mockHasher.Verify(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        // Garante que tokens foram revogados mesmo sendo Admin
        usuario.RefreshTokens.Should().OnlyContain(t => t.IsRevoked);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new UpdateSenhaCommand(Guid.NewGuid(), "123", "123");
        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync((Usuario?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }
}