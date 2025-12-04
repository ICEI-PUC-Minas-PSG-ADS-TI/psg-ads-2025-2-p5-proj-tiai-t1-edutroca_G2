using EduTroca.Core.Abstractions;
using EduTroca.Core.Common;
using EduTroca.Core.Entities;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;
using EduTroca.Core.Specifications;
using EduTroca.Tests.Helpers;
using EduTroca.UseCases.Common.DTOs;
using EduTroca.UseCases.Usuarios.DTOs;
using EduTroca.UseCases.Usuarios.Filter;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Usuarios.Filter;
public class FilterUsuariosQueryHandlerTests
{
    private readonly Mock<IRepository<Usuario>> _mockRepository;
    private readonly FilterUsuariosQueryHandler _handler;

    public FilterUsuariosQueryHandlerTests()
    {
        _mockRepository = new Mock<IRepository<Usuario>>();
        _handler = new FilterUsuariosQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagedDTOs_WhenRepositoryReturnsEntities()
    {
        // Arrange
        var nomeFilter = "Teste";
        var page = 1;
        var size = 10;

        var query = new FilterUsuariosQuery(
            nomeFilter,
            null,
            new PaginationDTO(page, size)
        );

        var usuariosList = new List<Usuario>
        {
            UsuarioHelpers.CreateUsuario(nome:"Teste 1"),
            UsuarioHelpers.CreateUsuario(nome: "Teste 2")
        };

        var pagedEntities = new PagedResult<Usuario>(usuariosList, 2, page, size);

        _mockRepository.Setup(r => r.ListPagedAsync(
                It.IsAny<UsuarioByFilter>(),
                page,
                size))
            .ReturnsAsync(pagedEntities);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();

        var pagedDTOs = result.Value;
        pagedDTOs.Should().BeOfType<PagedResult<UsuarioDTO>>();
        pagedDTOs.Items.Should().HaveCount(2);
        pagedDTOs.Items.First().Nome.Should().Be("Teste 1");
        pagedDTOs.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task Handle_ShouldCallRepository_WithCorrectFilters()
    {
        // Arrange
        var categoriasIds = new List<Guid> { Guid.NewGuid() };
        var query = new FilterUsuariosQuery(
            "Busca",
            categoriasIds,
            new PaginationDTO(2, 20)
        );

        var emptyResult = new PagedResult<Usuario>(new List<Usuario>(), 0, 2, 20);

        _mockRepository.Setup(r => r.ListPagedAsync(It.IsAny<UsuarioByFilter>(), 2, 20))
            .ReturnsAsync(emptyResult);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockRepository.Verify(r => r.ListPagedAsync(
            It.IsAny<UsuarioByFilter>(),
            2,
            20), Times.Once);
    }
}
