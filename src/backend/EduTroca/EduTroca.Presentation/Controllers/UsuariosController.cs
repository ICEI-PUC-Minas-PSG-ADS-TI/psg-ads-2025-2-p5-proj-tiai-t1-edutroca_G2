using EduTroca.Core.Enums;
using EduTroca.Presentation.Common;
using EduTroca.Presentation.DTOs;
using EduTroca.UseCases.Common.DTOs;
using EduTroca.UseCases.Usuarios.AddInterest;
using EduTroca.UseCases.Usuarios.ConfirmEmail;
using EduTroca.UseCases.Usuarios.Create;
using EduTroca.UseCases.Usuarios.Delete;
using EduTroca.UseCases.Usuarios.Filter;
using EduTroca.UseCases.Usuarios.Get;
using EduTroca.UseCases.Usuarios.Login;
using EduTroca.UseCases.Usuarios.Refresh;
using EduTroca.UseCases.Usuarios.ResendEmailConfirmation;
using EduTroca.UseCases.Usuarios.UpdateEmail;
using EduTroca.UseCases.Usuarios.UpdateProfile;
using EduTroca.UseCases.Usuarios.UpdateRoles;
using EduTroca.UseCases.Usuarios.UpdateSenha;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTroca.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsuariosController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateUsuario([FromBody] CreateUsuarioRequest request)
    {
        var command = new CreateUsuarioCommand(request.nome, request.email, request.senha);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPost("with-roles")]
    [Authorize(Roles = $"{nameof(ERole.Admin)}, ${nameof(ERole.Owner)}")]
    public async Task<IActionResult> CreateUsuarioWithRoles([FromBody] CreateUsuarioWithRolesRequest request)
    {
        var command = new CreateUsuarioCommand(request.nome, request.email, request.senha, request.roles);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand(request.email, request.senha);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var command = new RefreshTokenCommand(request.refreshToken);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPost("add-interest")]
    public async Task<IActionResult> AddInterest([FromBody] AddInterestRequest request)
    {
        var command = new AddInterestCommand(request.usuarioId, request.categoriaId);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUsuario(Guid id)
    {
        var command = new GetUsuarioQuery(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpGet]
    public async Task<IActionResult> FilterUsuarios([FromQuery] FilterUsuariosRequest request)
    {
        var pagination = new PaginationDTO(request.pageNumber, request.pageSize);
        var command = new FilterUsuariosQuery(request.nome, request.categoriasIds, pagination);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile(Guid id, [FromForm] UpdateProfileRequest request)
    {
        PictureDTO? pictureDTO = null;
        if (request.profilePicture is not null)
        {
            var stream = request.profilePicture.OpenReadStream();
            pictureDTO = new PictureDTO(stream,
                request.profilePicture.ContentType,
                request.profilePicture.Length);
        }
        var command = new UpdateProfileCommand(id, request.nome, request.bio, pictureDTO, request.removePicture);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPatch("roles/{id}")]
    [Authorize(Roles = $"{nameof(ERole.Admin)}, ${nameof(ERole.Owner)}")]
    public async Task<IActionResult> UpdateRoles(Guid id, [FromForm] UpdateRolesRequest request)
    {
        var command = new UpdateRolesCommand(id, request.rolesIds);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPatch("email/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateEmail(Guid id, [FromBody] UpdateEmailRequest request)
    {
        var command = new UpdateEmailCommand(id, request.novoEmail);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPatch("password/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateSenha(Guid id, [FromBody] UpdateSenhaRequest request)
    {
        var command = new UpdateSenhaCommand(id, request.senhaAtual, request.senhaNova);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPatch("confirm-email/{code}")]
    public async Task<IActionResult> ConfirmEmail(Guid code)
    {
        var command = new ConfirmEmailCommand(code);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPatch("resend-email")]
    public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendConfirmationEmailRequest request)
    {
        var command = new ResendEmailConfirmationCommand(request.email);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUsuario(Guid id)
    {
        var command = new DeleteUsuarioCommand(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
}
