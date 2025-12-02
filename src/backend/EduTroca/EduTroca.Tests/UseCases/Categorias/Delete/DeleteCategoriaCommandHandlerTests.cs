using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Categorias.Delete;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Categorias.Delete;
public class DeleteCategoriaCommandHandlerTests
{
    private readonly Mock<IRepository<Categoria>> _mockRepository;
    private readonly DeleteCategoriaCommandHandler _handler;

    public DeleteCategoriaCommandHandlerTests()
    {
        _mockRepository = new Mock<IRepository<Categoria>>();
        _handler = new DeleteCategoriaCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCategoriaExists()
    {
        // Arrange
        var categoriaId = Guid.NewGuid();
        var command = new DeleteCategoriaCommand(categoriaId);

        var categoria = new Categoria("Nome", "Descricao");

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<CategoriaById>()))
            .ReturnsAsync(categoria);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Success);

        _mockRepository.Verify(r => r.RemoveAsync(categoria), Times.Once);

        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCategoriaDoesNotExist()
    {
        // Arrange
        var command = new DeleteCategoriaCommand(Guid.NewGuid());

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<CategoriaById>()))
            .ReturnsAsync((Categoria?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Code.Should().Be("Categoria.NotFound");

        _mockRepository.Verify(r => r.RemoveAsync(It.IsAny<Categoria>()), Times.Never);

        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}
