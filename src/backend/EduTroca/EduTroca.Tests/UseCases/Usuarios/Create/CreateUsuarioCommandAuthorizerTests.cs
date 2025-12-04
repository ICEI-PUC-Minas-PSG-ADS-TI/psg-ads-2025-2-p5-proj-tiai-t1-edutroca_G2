using EduTroca.Core.Abstractions;
using EduTroca.Core.Enums;
using EduTroca.UseCases.Usuarios.Create;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Usuarios.Create;
public class CreateUsuarioCommandAuthorizerTests
{
    private readonly Mock<ICurrentUserService> _mockCurrentUser;
    private readonly CreateUsuarioCommandAuthorizer _authorizer;

    public CreateUsuarioCommandAuthorizerTests()
    {
        _mockCurrentUser = new Mock<ICurrentUserService>();
        _authorizer = new CreateUsuarioCommandAuthorizer(_mockCurrentUser.Object);
    }

    [Fact]
    public async Task Authorize_ShouldReturnSuccess_WhenNoRolesProvided()
    {
        // Usuário comum criando conta (Registro público)
        var command = new CreateUsuarioCommand("User", "u@u.com", "123", null);

        // Nem precisa estar logado
        _mockCurrentUser.Setup(c => c.IsInRole(It.IsAny<ERole>())).Returns(false);

        var result = await _authorizer.AuthorizeAsync(command, CancellationToken.None);

        result.IsError.Should().BeFalse();
    }

    [Fact]
    public async Task Authorize_ShouldReturnForbidden_WhenCommonUser_TriesToAssignRoles()
    {
        // Usuário comum (ou hacker) tentando criar um Admin
        var roles = new List<ERole> { ERole.Admin };
        var command = new CreateUsuarioCommand("Hacker", "h@h.com", "123", roles);

        // Não é Admin nem Owner
        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Admin)).Returns(false);
        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Owner)).Returns(false);

        var result = await _authorizer.AuthorizeAsync(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Forbidden);
    }

    [Fact]
    public async Task Authorize_ShouldReturnSuccess_WhenAdmin_AssignsRoles()
    {
        // Admin criando outro usuário com cargo
        var roles = new List<ERole> { ERole.Admin };
        var command = new CreateUsuarioCommand("NewAdmin", "a@a.com", "123", roles);

        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Admin)).Returns(true);

        var result = await _authorizer.AuthorizeAsync(command, CancellationToken.None);

        result.IsError.Should().BeFalse();
    }
}
