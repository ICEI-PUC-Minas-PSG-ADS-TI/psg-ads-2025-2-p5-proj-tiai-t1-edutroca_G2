using EduTroca.Core.Abstractions;
using EduTroca.Core.Common;
using EduTroca.Core.Entities;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Categorias.DTOs;
using EduTroca.UseCases.Categorias.Filter;
using EduTroca.UseCases.Common.DTOs;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Categorias.Filter;
public class FilterCategoriaQueryHandlerTests
{
    private readonly Mock<IRepository<Categoria>> _mockRepository;
    private readonly FilterCategoriaQueryHandler _handler;

    public FilterCategoriaQueryHandlerTests()
    {
        _mockRepository = new Mock<IRepository<Categoria>>();
        _handler = new FilterCategoriaQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagedDTOs_WhenRepositoryReturnsEntities()
    {
        // Arrange
        var queryNome = "Tecnologia";
        var page = 1;
        var size = 10;
        var pagination = new PaginationDTO(page, size);
        var query = new FilterCategoriasQuery(queryNome, pagination);

        var categoriasEntidade = new List<Categoria>
        {
            new Categoria("Tecnologia", "Desc 1"),
            new Categoria("Tecnologia Agrícola", "Desc 2")
        };

        var pagedResultEntities = new PagedResult<Categoria>(
            items: categoriasEntidade,
            totalCount: 2,
            pageNumber: page,
            pageSize: size);

        _mockRepository.Setup(r => r.ListPagedAsync(
                It.IsAny<CategoriaByFilter>(),
                page,
                size))
            .ReturnsAsync(pagedResultEntities);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();

        var pagedResultDto = result.Value;
        pagedResultDto.Should().BeOfType<PagedResult<CategoriaDTO>>();

        pagedResultDto.TotalCount.Should().Be(2);
        pagedResultDto.PageNumber.Should().Be(1);

        pagedResultDto.Items.Should().HaveCount(2);
        pagedResultDto.Items.First().Nome.Should().Be("Tecnologia");
    }

    [Fact]
    public async Task Handle_ShouldCallRepository_WithCorrectParameters()
    {
        // Arrange
        var query = new FilterCategoriasQuery(null, new PaginationDTO(2,50));

        var emptyResult = new PagedResult<Categoria>(new List<Categoria>(), 0, 2, 50);

        _mockRepository.Setup(r => r.ListPagedAsync(It.IsAny<Specification<Categoria>>(), 2, 50))
            .ReturnsAsync(emptyResult);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockRepository.Verify(r => r.ListPagedAsync(
            It.IsAny<CategoriaByFilter>(),
            2,
            50), Times.Once);
    }
}
