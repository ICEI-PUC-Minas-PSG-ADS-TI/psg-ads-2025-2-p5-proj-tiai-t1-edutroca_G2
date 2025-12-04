using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;
using EduTroca.Core.Specifications;
using EduTroca.Tests.Helpers;
using EduTroca.UseCases.Usuarios.AddInterest;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Usuarios.AddInterest;
public class AddInterestCommandHandlerTests
{
    private readonly Mock<IRepository<Usuario>> _mockUsuarioRepository;
    private readonly Mock<IRepository<Categoria>> _mockCategoriaRepository;
    private readonly AddInterestCommandHandler _handler;

    public AddInterestCommandHandlerTests()
    {
        _mockUsuarioRepository = new Mock<IRepository<Usuario>>();
        _mockCategoriaRepository = new Mock<IRepository<Categoria>>();

        _handler = new AddInterestCommandHandler(
            _mockUsuarioRepository.Object,
            _mockCategoriaRepository.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldAddInterest_WhenUserAndCategoryExist_AndNotDuplicate()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();

        var command = new AddInterestCommand(usuarioId, categoriaId);

        var usuario = UsuarioHelpers.CreateUsuario();
        var categoria = new Categoria("Matemática", "Exatas");

        _mockUsuarioRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync(usuario);

        _mockCategoriaRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<CategoriaById>()))
            .ReturnsAsync(categoria);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        usuario.CategoriasDeInteresse.Should().Contain(categoria);

        _mockUsuarioRepository.Verify(r => r.UpdateAsync(usuario), Times.Once);
        _mockUsuarioRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new AddInterestCommand(Guid.NewGuid(), Guid.NewGuid());

        _mockUsuarioRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync((Usuario?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Code.Should().Be("Usuario.NotFound");

        _mockCategoriaRepository.Verify(r => r.FirstOrDefaultAsync(It.IsAny<CategoriaById>()), Times.Never);
        _mockUsuarioRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var command = new AddInterestCommand(Guid.NewGuid(), Guid.NewGuid());
        var usuario = UsuarioHelpers.CreateUsuario();

        _mockUsuarioRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync(usuario);

        _mockCategoriaRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<CategoriaById>()))
            .ReturnsAsync((Categoria?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Code.Should().Be("Categoria.NotFound");

        _mockUsuarioRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenUserAlreadyHasInterest()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        var command = new AddInterestCommand(usuarioId, categoriaId);

        var usuario = UsuarioHelpers.CreateUsuario();
        var categoria = new Categoria("História", "Humanas");

        usuario.AddCategoriaDeInteresse(categoria);

        _mockUsuarioRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync(usuario);

        _mockCategoriaRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<CategoriaById>()))
            .ReturnsAsync(categoria);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        result.FirstError.Code.Should().Be("Usuario.Categoria");
        result.FirstError.Description.Should().Be("Usuario ja possui interesse nessa categoria.");

        _mockUsuarioRepository.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Never);
        _mockUsuarioRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}
