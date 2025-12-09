using MediatR;

namespace EduTroca.UseCases.Common.Events;
public record LikeRegistradoEvent(Guid autorId) : INotification;
