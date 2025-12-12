using EduTroca.Core.Common;
using EduTroca.Presentation.Common;
using EduTroca.Presentation.DTOs.Requests;
using EduTroca.Presentation.DTOs.Responses;
using EduTroca.UseCases.Common.DTOs;
using EduTroca.UseCases.Conteudos.Comment;
using EduTroca.UseCases.Conteudos.Dislike;
using EduTroca.UseCases.Conteudos.Filter;
using EduTroca.UseCases.Conteudos.Get;
using EduTroca.UseCases.Conteudos.Like;
using EduTroca.UseCases.Conteudos.Perguntas.Create;
using EduTroca.UseCases.Conteudos.Perguntas.Delete;
using EduTroca.UseCases.Conteudos.Perguntas.Update;
using EduTroca.UseCases.Conteudos.Videos.Create;
using EduTroca.UseCases.Conteudos.Videos.Delete;
using EduTroca.UseCases.Conteudos.Videos.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduTroca.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ConteudosController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("video")]
    [Authorize]
    [RequestSizeLimit(2L * 1024 * 1024 * 1024)]
    [RequestFormLimits(MultipartBodyLengthLimit = 2L * 1024 * 1024 * 1024)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateVideo([FromForm] CreateVideoRequest request)
    {
        var videoStream = request.video.OpenReadStream();
        var videoDTO = new FileDTO(videoStream,
            request.video.ContentType,
            request.video.Length);

        var imageStream = request.imagem.OpenReadStream();
        var imageDTO = new FileDTO(imageStream,
            request.imagem.ContentType,
            request.imagem.Length);

        var command = new CreateVideoCommand(request.titulo, request.descricao, request.categoriaId, videoDTO, imageDTO);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPost("pergunta")]
    [Authorize]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreatePergunta([FromForm] CreatePerguntaRequest request)
    {
        var command = new CreatePerguntaCommand(request.titulo, request.descricao, request.categoriaId, request.texto);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPost("comment/{id}")]
    [Authorize]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CommentConteudo(Guid id, [FromBody] CommentRequest request)
    {
        var command = new CommentCommand(id, request.texto);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(PagedResult<SimpleConteudoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> FilterConteudos([FromQuery] FilterConteudosRequest request)
    {
        var pagination = new PaginationDTO(request.pageNumber, request.pageSize);
        var query = new FilterConteudoQuery(
            request.Titulo,
            request.Visualizacoes,
            request.Likes,
            request.Periodo,
            request.NivelUsuario,
            request.AutorId,
            request.CategoriasIds,
            request.Tipo,
            request.OrderBy,
            pagination);
        var result = await _mediator.Send(query);
        return result.ToActionResult(x => x.Map(SimpleConteudoResponse.FromConteudoDTO));
    }
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(CompleteConteudoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetConteudo(Guid id)
    {
        var query = new GetConteudoQuery(id);
        var result = await _mediator.Send(query);
        return result.ToActionResult(CompleteConteudoResponse.FromConteudoDTO);
    }
    [HttpPatch("video/{id}")]
    [Authorize]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateVideo(Guid id, [FromForm] UpdateVideoRequest request)
    {

        var imageStream = request.imagem.OpenReadStream();
        var imageDTO = new FileDTO(imageStream,
            request.imagem.ContentType,
            request.imagem.Length);

        var command = new UpdateVideoCommand(
            id,
            request.titulo,
            request.descricao,
            request.categoriaId,
            imageDTO);

        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPatch("pergunta/{id}")]
    [Authorize]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdatePergunta(Guid id, [FromForm] UpdatePerguntaRequest request)
    {

        var command = new UpdatePerguntaCommand(
            id,
            request.titulo,
            request.descricao,
            request.categoriaId,
            request.texto);

        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPatch("toggle-like/{id}")]
    [Authorize]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ToggleLikeConteudo(Guid id)
    {
        var command = new ToggleLikeCommand(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPatch("toggle-dislike/{id}")]
    [Authorize]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ToggleDislikeConteudo(Guid id)
    {
        var command = new ToggleDislikeCommand(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpDelete("video/{id}")]
    [Authorize]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteVideo(Guid id)
    {
        var command = new DeleteVideoCommand(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpDelete("pergunta/{id}")]
    [Authorize]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeletePergunta(Guid id)
    {
        var command = new DeletePerguntaCommand(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
}
