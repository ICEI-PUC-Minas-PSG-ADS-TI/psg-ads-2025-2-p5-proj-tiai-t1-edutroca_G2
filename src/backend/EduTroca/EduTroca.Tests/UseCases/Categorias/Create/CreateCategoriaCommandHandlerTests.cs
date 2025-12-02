using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Categorias.Create;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Categorias.Create;
public class CreateCategoriaCommandHandlerTests
{
    private readonly Mock<IRepository<Categoria>> _mockRepository;
    private readonly CreateCategoriaCommandHandler _handler;

    public CreateCategoriaCommandHandlerTests()
    {
        _mockRepository = new Mock<IRepository<Categoria>>();
        _handler = new CreateCategoriaCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateCategoria_WhenNameIsUnique()
    {
        // Arrange
        var command = new CreateCategoriaCommand("Matemática", "Materias de exatas");

        _mockRepository.Setup(r => r.AnyAsync(It.IsAny<CategoriaByNome>()))
            .ReturnsAsync(false);

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Categoria>()))
            .ReturnsAsync((Categoria c) => c);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Nome.Should().Be("Matemática");
        result.Value.Descricao.Should().Be("Materias de exatas");

        _mockRepository.Verify(r => r.AddAsync(It.Is<Categoria>(c =>
            c.Nome == command.nome &&
            c.Descricao == command.descricao)), Times.Once);

        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenNameAlreadyExists()
    {
        // Arrange
        var command = new CreateCategoriaCommand("História", "Humanas");

        _mockRepository.Setup(r => r.AnyAsync(It.IsAny<CategoriaByNome>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        result.FirstError.Description.Should().Contain("ja existe");

        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Categoria>()), Times.Never);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}
