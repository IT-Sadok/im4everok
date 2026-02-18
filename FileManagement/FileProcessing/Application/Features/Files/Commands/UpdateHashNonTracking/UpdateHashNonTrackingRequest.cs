namespace Application.Features.Files.Commands.UpdateHashNonTracking
{
    public record UpdateHashNonTrackingRequest(Guid FileId, string Hash);
}
