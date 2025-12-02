using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Categorias.DTOs;
using EduTroca.UseCases.Categorias.Get;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Categorias.Get;
public class GetCategoriaQueryHandlerTests
{
    private readonly Mock<IRepository<Categoria>> _mockRepository;
    private readonly GetCategoriaQueryHandler _handler;

    public GetCategoriaQueryHandlerTests()
    {
        _mockRepository = new Mock<IRepository<Categoria>>();
        _handler = new GetCategoriaQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCategoriaDTO_WhenCategoriaExists()
    {
        // Arrange
        var categoriaId = Guid.NewGuid();
        var query = new GetCategoriaQuery(categoriaId);

        var categoria = new Categoria("Ciências", "Matérias científicas");

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<CategoriaById>()))
            .ReturnsAsync(categoria);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();

        result.Value.Should().BeOfType<CategoriaDTO>();
        result.Value.Nome.Should().Be("Ciências");
        result.Value.Descricao.Should().Be("Matérias científicas");

        _mockRepository.Verify(r => r.FirstOrDefaultAsync(It.IsAny<CategoriaById>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCategoriaDoesNotExist()
    {
        // Arrange
        var query = new GetCategoriaQuery(Guid.NewGuid());

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<CategoriaById>()))
            .ReturnsAsync((Categoria?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Code.Should().Be("Categoria.NotFound");
        result.FirstError.Description.Should().Be("Categoria inexistente.");

        _mockRepository.Verify(r => r.FirstOrDefaultAsync(It.IsAny<CategoriaById>()), Times.Once);
    }
}
