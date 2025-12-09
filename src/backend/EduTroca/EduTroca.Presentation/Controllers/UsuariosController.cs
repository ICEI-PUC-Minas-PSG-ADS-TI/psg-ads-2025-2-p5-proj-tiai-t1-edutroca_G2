using EduTroca.Core.Common;
using EduTroca.Core.Enums;
using EduTroca.Presentation.Common;
using EduTroca.Presentation.DTOs.Requests;
using EduTroca.Presentation.DTOs.Responses;
using EduTroca.UseCases.Common.DTOs;
using EduTroca.UseCases.Usuarios.AddInterest;
using EduTroca.UseCases.Usuarios.Certify;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduTroca.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsuariosController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(CompleteUsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUsuario([FromBody] CreateUsuarioRequest request)
    {
        var command = new CreateUsuarioCommand(request.nome, request.email, request.senha);
        var result = await _mediator.Send(command);
        return result.ToActionResult(CompleteUsuarioResponse.FromUsuarioDTO);
    }
    [HttpPost("with-roles")]
    [Authorize(Roles = $"{nameof(ERole.Admin)},{nameof(ERole.Owner)}")]
    [ProducesResponseType(typeof(CompleteUsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateUsuarioWithRoles([FromBody] CreateUsuarioWithRolesRequest request)
    {
        var command = new CreateUsuarioCommand(request.nome, request.email, request.senha, request.roles);
        var result = await _mediator.Send(command);
        return result.ToActionResult(CompleteUsuarioResponse.FromUsuarioDTO);
    }
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand(request.email, request.senha);
        var result = await _mediator.Send(command);
        return result.ToActionResult(LoginResponse.FromLoginDTO);
    }
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var command = new RefreshTokenCommand(request.refreshToken);
        var result = await _mediator.Send(command);
        return result.ToActionResult(LoginResponse.FromLoginDTO);
    }
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CompleteUsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUsuario(Guid id)
    {
        var command = new GetUsuarioQuery(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult(CompleteUsuarioResponse.FromUsuarioDTO);
    }
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<SimpleUsuarioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FilterUsuarios([FromQuery] FilterUsuariosRequest request)
    {
        var pagination = new PaginationDTO(request.pageNumber, request.pageSize);
        var query = new FilterUsuariosQuery(request.nome, request.categoriasIds, pagination);
        var result = await _mediator.Send(query);
        return result.ToActionResult(x => x.Map(SimpleUsuarioResponse.FromUsuarioDTO));
    }
    [HttpPatch("add-interest")]
    [Authorize]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddInterest([FromBody] AddInterestRequest request)
    {
        var command = new AddInterestCommand(request.usuarioId, request.categoriaId);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPatch("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(CompleteUsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateProfile(Guid id, [FromForm] UpdateProfileRequest request)
    {
        FileDTO? pictureDTO = null;
        if (request.profilePicture is not null)
        {
            var stream = request.profilePicture.OpenReadStream();
            pictureDTO = new FileDTO(stream,
                request.profilePicture.ContentType,
                request.profilePicture.Length);
        }
        var command = new UpdateProfileCommand(id, request.nome, request.bio, pictureDTO, request.removePicture);
        var result = await _mediator.Send(command);
        return result.ToActionResult(CompleteUsuarioResponse.FromUsuarioDTO);
    }
    [HttpPatch("roles/{id}")]
    [Authorize(Roles = $"{nameof(ERole.Admin)}, ${nameof(ERole.Owner)}")]
    [ProducesResponseType(typeof(CompleteUsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateRoles(Guid id, [FromBody] UpdateRolesRequest request)
    {
        var command = new UpdateRolesCommand(id, request.rolesIds);
        var result = await _mediator.Send(command);
        return result.ToActionResult(CompleteUsuarioResponse.FromUsuarioDTO);
    }
    [HttpPatch("certify/{id}")]
    [Authorize(Roles = $"{nameof(ERole.Admin)}, ${nameof(ERole.Owner)}")]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CertifyUsuario(Guid id)
    {
        var command = new CertifyUsuarioCommand(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPatch("email/{id}")]
    [Authorize]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateEmail(Guid id, [FromBody] UpdateEmailRequest request)
    {
        var command = new UpdateEmailCommand(id, request.novoEmail);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPatch("password")]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateSenha([FromBody] UpdateSenhaRequest request)
    {
        var command = new UpdateSenhaCommand(request.usuarioEmail, request.senhaAtual, request.senhaNova);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPatch("confirm-email/{code}")]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmEmail(Guid code)
    {
        var command = new ConfirmEmailCommand(code);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPatch("resend-email")]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendConfirmationEmailRequest request)
    {
        var command = new ResendEmailConfirmationCommand(request.email);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteUsuario(Guid id)
    {
        var command = new DeleteUsuarioCommand(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
}
