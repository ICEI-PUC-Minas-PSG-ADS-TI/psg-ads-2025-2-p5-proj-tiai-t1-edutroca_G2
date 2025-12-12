using EduTroca.Core.Enums;

namespace EduTroca.Presentation.DTOs.Requests;
public record UpdateRolesRequest(List<ERole> rolesIds);
