using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using EduTroca.Tests.Helpers;
using EduTroca.UseCases.Common.DTOs;
using EduTroca.UseCases.Usuarios.UpdateProfile;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace EduTroca.Tests.UseCases.Usuarios.UpdateProfile;
public class UpdateProfileCommandHandlerTests
{
    private readonly Mock<IRepository<Usuario>> _mockRepository;
    private readonly Mock<IFileService> _mockFileService;
    private readonly UpdateProfileCommandHandler _handler;

    public UpdateProfileCommandHandlerTests()
    {
        _mockRepository = new Mock<IRepository<Usuario>>();
        _mockFileService = new Mock<IFileService>();

        _handler = new UpdateProfileCommandHandler(
            _mockRepository.Object,
            _mockFileService.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new UpdateProfileCommand(Guid.NewGuid(), "Nome", "Bio", null, false);

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync((Usuario?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldUpdateTextOnly_WhenNoPictureProvided()
    {
        // Arrange
        var usuario = UsuarioHelpers.CreateUsuario();
        var command = new UpdateProfileCommand(usuario.Id, "Novo Nome", "Nova Bio", null, false);

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync(usuario);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();

        usuario.Nome.Should().Be("Novo Nome");

        _mockFileService.Verify(f => f.SaveFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockFileService.Verify(f => f.RemoveFileAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);

        _mockRepository.Verify(r => r.UpdateAsync(usuario), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReplacePicture_WhenNewPictureProvided()
    {
        // Arrange
        var usuario = UsuarioHelpers.CreateUsuario(currentPicturePath: "Imagens/antiga.jpg");

        var stream = new MemoryStream();
        var pictureDto = new PictureDTO(stream, "image/png", 1000);

        var command = new UpdateProfileCommand(usuario.Id, "Nome", "Bio", pictureDto, false);

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync(usuario);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockFileService.Verify(f => f.RemoveFileAsync("Imagens/antiga.jpg", It.IsAny<CancellationToken>()), Times.Once);

        _mockFileService.Verify(f => f.SaveFileAsync(
            "Imagens",
            $"{usuario.Id}.png", 
            stream,
            It.IsAny<CancellationToken>()), Times.Once);

        usuario.CaminhoImagem.Should().Be($"Imagens/{usuario.Id}.png");
    }

    [Fact]
    public async Task Handle_ShouldDeletePicture_WhenDeleteFlagIsTrue()
    {
        // Arrange
        var usuario = UsuarioHelpers.CreateUsuario(currentPicturePath: "Imagens/foto.jpg");

        var command = new UpdateProfileCommand(usuario.Id, "Nome", "Bio", null, DeletePicture: true);

        _mockRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UsuarioById>()))
            .ReturnsAsync(usuario);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockFileService.Verify(f => f.RemoveFileAsync("Imagens/foto.jpg", It.IsAny<CancellationToken>()), Times.Once);

        usuario.CaminhoImagem.Should().BeNullOrEmpty();
    }
}
