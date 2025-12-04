using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;
using EduTroca.Core.Specifications;
using EduTroca.Tests.Helpers;
using EduTroca.UseCases.Common.Guards;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Common.Guards;
public class HierarchyGuardTests
{
    private readonly Mock<ICurrentUserService> _mockCurrentUser;
    private readonly Mock<IRepository<Usuario>> _mockRepository;
    private readonly HierarchyGuard _guard;

    public HierarchyGuardTests()
    {
        _mockCurrentUser = new Mock<ICurrentUserService>();
        _mockRepository = new Mock<IRepository<Usuario>>();

        _guard = new HierarchyGuard(_mockCurrentUser.Object, _mockRepository.Object);
    }

    [Fact]
    public async Task Validate_ShouldReturnSuccess_WhenUserIsSelf()
    {
        // Arrange
        var myId = Guid.NewGuid();

        _mockCurrentUser.Setup(c => c.UserId).Returns(myId);

        _mockCurrentUser.Setup(c => c.IsInRole(It.IsAny<ERole>())).Returns(false);

        // Act
        var result = await _guard.ValidateUserModificationAsync(myId);

        // Assert
        result.IsError.Should().BeFalse();

        _mockRepository.Verify(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()), Times.Never);
    }


    [Fact]
    public async Task Validate_ShouldReturnSuccess_WhenUserIsOwner_ModifyingAnyone()
    {
        // Arrange
        var myId = Guid.NewGuid();
        var targetId = Guid.NewGuid();

        _mockCurrentUser.Setup(c => c.UserId).Returns(myId);
        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Owner)).Returns(true);

        // Act
        var result = await _guard.ValidateUserModificationAsync(targetId);

        // Assert
        result.IsError.Should().BeFalse();

        _mockRepository.Verify(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()), Times.Never);
    }


    [Fact]
    public async Task Validate_ShouldReturnForbidden_WhenUserIsCommon_ModifyingOther()
    {
        // Arrange
        var myId = Guid.NewGuid();
        var targetId = Guid.NewGuid();

        _mockCurrentUser.Setup(c => c.UserId).Returns(myId);

        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Admin)).Returns(false);
        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Owner)).Returns(false);

        // Act
        var result = await _guard.ValidateUserModificationAsync(targetId);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Forbidden);

        _mockRepository.Verify(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()), Times.Never);
    }


    [Fact]
    public async Task Validate_ShouldReturnSuccess_WhenAdmin_ModifiesCommonUser()
    {
        // Arrange
        var myId = Guid.NewGuid();
        var targetId = Guid.NewGuid();

        _mockCurrentUser.Setup(c => c.UserId).Returns(myId);
        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Admin)).Returns(true);

        var targetUser = UsuarioHelpers.CreateUsuario(ERole.User);
        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync(targetUser);

        // Act
        var result = await _guard.ValidateUserModificationAsync(targetId);

        // Assert
        result.IsError.Should().BeFalse();
    }


    [Fact]
    public async Task Validate_ShouldReturnForbidden_WhenAdmin_ModifiesAnotherAdmin()
    {
        // Arrange
        var myId = Guid.NewGuid();
        var targetId = Guid.NewGuid();

        _mockCurrentUser.Setup(c => c.UserId).Returns(myId);
        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Admin)).Returns(true);

        var targetUser = UsuarioHelpers.CreateUsuario(ERole.Admin);
        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync(targetUser);

        // Act
        var result = await _guard.ValidateUserModificationAsync(targetId);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Forbidden);
    }

    [Fact]
    public async Task Validate_ShouldReturnForbidden_WhenAdmin_ModifiesOwner()
    {
        // Arrange
        var myId = Guid.NewGuid();
        var targetId = Guid.NewGuid();

        _mockCurrentUser.Setup(c => c.UserId).Returns(myId);
        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Admin)).Returns(true);

        var targetUser = UsuarioHelpers.CreateUsuario(ERole.Owner);
        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync(targetUser);

        // Act
        var result = await _guard.ValidateUserModificationAsync(targetId);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Forbidden);
    }

    [Fact]
    public async Task Validate_ShouldReturnSuccess_WhenTargetUserDoesNotExist()
    {
        // Arrange
        _mockCurrentUser.Setup(c => c.IsInRole(ERole.Admin)).Returns(true);

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync((Usuario?)null);

        // Act
        var result = await _guard.ValidateUserModificationAsync(Guid.NewGuid());

        // Assert
        result.IsError.Should().BeFalse();
    }
}
