using EduTroca.Core.Abstractions;
using EduTroca.Core.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EduTroca.Presentation.Common;
public class HttpCurrentUserService(IHttpContextAccessor context) : ICurrentUserService
{
    private readonly IHttpContextAccessor _context = context;
    public Guid UserId => Guid.Parse(_context.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    public bool IsInRole(ERole role)
    {
        return _context.HttpContext?.User?.IsInRole(role.ToString()) ?? false;
    }
}
