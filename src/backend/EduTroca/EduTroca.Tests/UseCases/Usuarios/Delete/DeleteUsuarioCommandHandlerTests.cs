using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using EduTroca.Tests.Helpers;
using EduTroca.UseCases.Usuarios.Delete;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Usuarios.Delete;
public class DeleteUsuarioCommandHandlerTests
{
    private readonly Mock<IRepository<Usuario>> _mockUsuarioRepository;
    private readonly DeleteUsuarioCommandHandler _handler;

    public DeleteUsuarioCommandHandlerTests()
    {
        _mockUsuarioRepository = new Mock<IRepository<Usuario>>();
        _handler = new DeleteUsuarioCommandHandler(_mockUsuarioRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUsuarioExists()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var command = new DeleteUsuarioCommand(usuarioId);

        var usuario = UsuarioHelpers.CreateUsuario();

        _mockUsuarioRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync(usuario);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        _mockUsuarioRepository.Verify(r => r.RemoveAsync(usuario), Times.Once);
        _mockUsuarioRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUsuarioDoesNotExist()
    {
        // Arrange
        var command = new DeleteUsuarioCommand(Guid.NewGuid());

        _mockUsuarioRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync((Usuario?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);

        _mockUsuarioRepository.Verify(r => r.RemoveAsync(It.IsAny<Usuario>()), Times.Never);
        _mockUsuarioRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}
