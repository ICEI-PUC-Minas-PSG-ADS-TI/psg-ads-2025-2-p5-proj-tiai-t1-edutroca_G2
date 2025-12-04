using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Usuarios.Create;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Usuarios.Create;

public class CreateUsuarioCommandHandlerTests
{
    private readonly Mock<IRepository<Usuario>> _mockUsuarioRepository;
    private readonly Mock<IRepository<Role>> _mockRoleRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly CreateUsuarioCommandHandler _handler;

    public CreateUsuarioCommandHandlerTests()
    {
        _mockUsuarioRepository = new Mock<IRepository<Usuario>>();
        _mockRoleRepository = new Mock<IRepository<Role>>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockEmailService = new Mock<IEmailService>();

        _handler = new CreateUsuarioCommandHandler(
            _mockUsuarioRepository.Object,
            _mockRoleRepository.Object,
            _mockPasswordHasher.Object,
            _mockEmailService.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WithDefaultRole_WhenRolesListIsNull()
    {
        // Arrange
        var command = new CreateUsuarioCommand("Joao", "joao@email.com", "123456", null); // Roles null

        _mockUsuarioRepository.Setup(r => r.AnyAsync(It.IsAny<UsuarioByEmail>())).ReturnsAsync(false);
        _mockUsuarioRepository.Setup(r => r.AddAsync(It.IsAny<Usuario>())).ReturnsAsync((Usuario u) => u);
        _mockPasswordHasher.Setup(p => p.HashPassword(It.IsAny<string>())).Returns("hash");

        // Simula encontrar a role padrão "User"
        var defaultRole = new Role((int)ERole.User, "User");
        _mockRoleRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<RoleById>()))
            .ReturnsAsync(defaultRole);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();

        // Verifica se persistiu com a role default
        _mockUsuarioRepository.Verify(r => r.AddAsync(It.Is<Usuario>(u =>
            u.Roles.Count == 1 &&
            u.Roles.First().Id == (int)ERole.User
        )), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WithSpecificRoles_WhenRolesListIsProvided()
    {
        // Arrange
        var rolesIds = new List<ERole> { ERole.Admin };
        var command = new CreateUsuarioCommand("Maria", "maria@email.com", "123456", rolesIds);

        _mockUsuarioRepository.Setup(r => r.AnyAsync(It.IsAny<UsuarioByEmail>())).ReturnsAsync(false);
        _mockUsuarioRepository.Setup(r => r.AddAsync(It.IsAny<Usuario>())).ReturnsAsync((Usuario u) => u);
        _mockPasswordHasher.Setup(p => p.HashPassword(It.IsAny<string>())).Returns("hash");

        // Simula encontrar a role Admin no banco
        var adminRole = new Role((int)ERole.Admin, "Admin");
        _mockRoleRepository.Setup(r => r.ListAsync(It.IsAny<RolesByIdsList>()))
            .ReturnsAsync(new List<Role> { adminRole });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();

        // Verifica se persistiu com a role Admin
        _mockUsuarioRepository.Verify(r => r.AddAsync(It.Is<Usuario>(u =>
            u.Roles.First().Id == (int)ERole.Admin
        )), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnValidation_WhenSpecificRolesDoNotExist()
    {
        // Arrange
        var rolesIds = new List<ERole> { ERole.Admin, ERole.User }; // Pediu 2 roles
        var command = new CreateUsuarioCommand("Teste", "t@t.com", "123", rolesIds);

        _mockUsuarioRepository.Setup(r => r.AnyAsync(It.IsAny<UsuarioByEmail>())).ReturnsAsync(false);

        // Simula que o banco só encontrou 1 role (digamos que Admin foi deletado ou ID errado)
        var foundRoles = new List<Role> { new Role((int)ERole.User, "User") };

        _mockRoleRepository.Setup(r => r.ListAsync(It.IsAny<RolesByIdsList>()))
            .ReturnsAsync(foundRoles);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        result.FirstError.Code.Should().Be("Roles.Invalid");

        // Garante que não salvou
        _mockUsuarioRepository.Verify(r => r.AddAsync(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenEmailExists()
    {
        // Arrange
        var command = new CreateUsuarioCommand("Teste", "existente@email.com", "123", null);

        _mockUsuarioRepository.Setup(r => r.AnyAsync(It.IsAny<UsuarioByEmail>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnexpected_WhenDefaultRoleMissing()
    {
        // Arrange - Enviando NULL nas roles
        var command = new CreateUsuarioCommand("Teste", "t@t.com", "123", null);

        _mockUsuarioRepository.Setup(r => r.AnyAsync(It.IsAny<UsuarioByEmail>())).ReturnsAsync(false);

        // Simula que role 'User' não está no banco (erro de infra/seed)
        _mockRoleRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<RoleById>()))
            .ReturnsAsync((Role?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
    }
}