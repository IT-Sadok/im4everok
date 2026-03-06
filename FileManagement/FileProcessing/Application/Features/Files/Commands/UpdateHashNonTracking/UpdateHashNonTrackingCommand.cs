using Application.Interfaces.Repositories;

using Microsoft.Extensions.Logging;

namespace Application.Features.Files.Commands.UpdateHashNonTracking
{
    public class UpdateHashNonTrackingCommand(IFileRepository fileRepository,
        ILogger<UpdateHashNonTrackingCommand> logger)
    {
        public async Task<bool> Execute(UpdateHashNonTrackingRequest request, CancellationToken cancellationToken)
        {
            if (request.FileId == Guid.Empty)
            {
                //logger.LogError("UpdateHashNonTrackingRequest FileId is empty");
                return false;
            }

            if (string.IsNullOrEmpty(request.Hash))
            {
                //logger.LogError("UpdateHashNonTrackingRequest Hash is empty");
                return false;
            }

            bool updated = await fileRepository.UpdateHash(request.FileId, request.Hash, cancellationToken);

            return updated;
        }
    }
}
