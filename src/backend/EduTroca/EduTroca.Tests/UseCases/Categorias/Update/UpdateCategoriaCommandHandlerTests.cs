using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Categorias.Update;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Categorias.Update;
public class UpdateCategoriaCommandHandlerTests
{
    private readonly Mock<IRepository<Categoria>> _mockRepository;
    private readonly UpdateCategoriaCommandHandler _handler;

    public UpdateCategoriaCommandHandlerTests()
    {
        _mockRepository = new Mock<IRepository<Categoria>>();
        _handler = new UpdateCategoriaCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenNameAlreadyExists()
    {
        // Arrange
        var command = new UpdateCategoriaCommand(Guid.NewGuid(), "Nome Duplicado", "Nova Descrição");
        var categoria = new Categoria("Nome Antigo", "Desc Antiga");

        _mockRepository.Setup(r => r.AnyAsync(It.IsAny<CategoriaByNome>()))
            .ReturnsAsync(true);
        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<CategoriaById>()))
            .ReturnsAsync(categoria);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        result.FirstError.Code.Should().Be("Categoria.Nome");
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
    [Fact]
    public async Task Handle_ShouldUpdate_WhenNameIsUnchanged_Or_Unique()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new UpdateCategoriaCommand(id, "Matemática", "Nova Descrição");

        var categoriaExistente = new Categoria("Matemática", "Desc Antiga");
        _mockRepository.Setup(r => r.AnyAsync(It.IsAny<CategoriaByNome>()))
            .ReturnsAsync(false);

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<CategoriaById>()))
            .ReturnsAsync(categoriaExistente);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Categoria>()), Times.Once);
    }
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        // Arrange
        var command = new UpdateCategoriaCommand(Guid.NewGuid(), "Nome Novo", "Nova Descrição");

        _mockRepository.Setup(r => r.AnyAsync(It.IsAny<CategoriaByNome>()))
            .ReturnsAsync(false);

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<CategoriaById>()))
            .ReturnsAsync((Categoria?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Code.Should().Be("Categoria.NotFound");

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Categoria>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldUpdateCategoria_WhenRequestIsValid()
    {
        // Arrange
        var categoriaId = Guid.NewGuid();
        var categoriaExistente = new Categoria("Nome Antigo", "Desc Antiga");

        var command = new UpdateCategoriaCommand(categoriaId, "Nome Atualizado", "Desc Atualizada");

        _mockRepository.Setup(r => r.AnyAsync(It.IsAny<CategoriaByNome>()))
            .ReturnsAsync(false);

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<CategoriaById>()))
            .ReturnsAsync(categoriaExistente);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        categoriaExistente.Nome.Should().Be("Nome Atualizado");
        categoriaExistente.Descricao.Should().Be("Desc Atualizada");

        _mockRepository.Verify(r => r.UpdateAsync(categoriaExistente), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
