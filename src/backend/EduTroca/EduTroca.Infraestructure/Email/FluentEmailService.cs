using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using FluentEmail.Core;
using Microsoft.Extensions.Configuration;

namespace EduTroca.Infraestructure.Email;
public class FluentEmailService(IFluentEmail fluentEmail, IConfiguration configuration) : IEmailService
{
    private readonly IFluentEmail _fluentEmail = fluentEmail;
    private readonly IConfiguration _configuration = configuration;
    public async Task SendConfirmationAsync(Usuario usuario)
    {
        var link = $"{_configuration["FrontendUrl"]}/confirmar?token={usuario.EmailConfirmationCode.Id}";

        var model = new EmailModel
        {
            Nome = usuario.Nome,
            LinkConfirmacao = link
        };
        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "ConfirmacaoEmail.cshtml");
        var response = await _fluentEmail
            .To(usuario.Email)
            .Subject("Confirme seu email no EduTroca")
            .UsingTemplateFromFile(templatePath, model)
            .SendAsync();
    }
}
