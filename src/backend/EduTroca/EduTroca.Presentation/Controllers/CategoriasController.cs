using EduTroca.Core.Common;
using EduTroca.Core.Enums;
using EduTroca.Presentation.Common;
using EduTroca.Presentation.DTOs.Requests;
using EduTroca.Presentation.DTOs.Responses;
using EduTroca.UseCases.Categorias.Create;
using EduTroca.UseCases.Categorias.Delete;
using EduTroca.UseCases.Categorias.DTOs;
using EduTroca.UseCases.Categorias.Filter;
using EduTroca.UseCases.Categorias.Get;
using EduTroca.UseCases.Categorias.Update;
using EduTroca.UseCases.Common.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduTroca.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoriasController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    [HttpPost]
    [Authorize(Roles = $"{nameof(ERole.Admin)},{nameof(ERole.Owner)}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateCategoria([FromBody] CreateCategoriaRequest request)
    {
        var command = new CreateCategoriaCommand(request.nome, request.descricao);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CompleteCategoriaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategoria(Guid id)
    {
        var query = new GetCategoriaQuery(id);
        var result = await _mediator.Send(query);
        return result.ToActionResult(SimpleCategoriaResponse.FromCategoriaDTO);
    }
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<SimpleCategoriaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FilterCategorias([FromQuery] FilterCategoriasRequest request)
    {
        var pagination = new PaginationDTO(request.pageNumber, request.pageSize);
        var command = new FilterCategoriasQuery(request.nome, pagination);
        var result = await _mediator.Send(command);
        return result.ToActionResult(x => x.Map(SimpleCategoriaResponse.FromCategoriaDTO));
    }
    [HttpPatch("{id}")]
    [Authorize(Roles = $"{nameof(ERole.Admin)},{nameof(ERole.Owner)}")]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateCategoria(Guid id, [FromBody] UpdateCategoriaRequest request)
    {
        var command = new UpdateCategoriaCommand(id, request.nome, request.descricao);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = $"{nameof(ERole.Admin)},{nameof(ERole.Owner)}")]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteCategoria(Guid id)
    {
        var command = new DeleteCategoriaCommand(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
}
