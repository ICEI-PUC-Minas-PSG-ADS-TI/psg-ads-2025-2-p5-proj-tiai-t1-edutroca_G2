using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;
using EduTroca.Core.Specifications;
using EduTroca.Tests.Helpers;
using EduTroca.UseCases.Usuarios.DTOs;
using EduTroca.UseCases.Usuarios.Get;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Usuarios.Get;
public class GetUsuarioQueryHandlerTests
{
    private readonly Mock<IRepository<Usuario>> _mockRepository;
    private readonly GetUsuarioQueryHandler _handler;

    public GetUsuarioQueryHandlerTests()
    {
        _mockRepository = new Mock<IRepository<Usuario>>();
        _handler = new GetUsuarioQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUsuarioDTO_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetUsuarioQuery(userId);

        var usuario = UsuarioHelpers.CreateUsuario();

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync(usuario);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<UsuarioDTO>();

        result.Value.Nome.Should().Be("Teste");
        result.Value.Email.Should().Be("teste@email.com");

        _mockRepository.Verify(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var query = new GetUsuarioQuery(Guid.NewGuid());

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync((Usuario?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Code.Should().Be("Usuario.NotFound");
    }
}
