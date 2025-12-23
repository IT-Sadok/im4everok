using System.Security.Cryptography;
﻿using Application.Interfaces;
using Application.Interfaces.FileStorage;
using Application.Interfaces.Repositories;

using Domain.Entities;

namespace Application.Features.Files.Commands.Upload
{
    public class UploadFileService(IFileRepository fileRepository, IUnitOfWork unitOfWork, IFileStorage fileStorage)
    {
        string CalculateMD5(Stream file)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(file);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        public async Task<UploadFileResponse> Execute(UploadFileRequest request, CancellationToken ct)
        {
            try
            {
                Guid fileId = Guid.NewGuid();

                var checksum = CalculateMD5(request.Content);
                var file = new FileEntity
                {
                    Id = fileId,
                    CreatedAt = DateTime.Now,
                    FileName = request.FileName,
                    SizeBytes = request.Content.Length,
                    ContentType = request.ContentType,
                    BlobPath = fileId.ToString(),
                    Checksum = checksum
                };

                await fileRepository.Add(file);
                await unitOfWork.SaveChangesAsync();

                await fileStorage.UploadAsync(fileId.ToString(), request.Content, "files", request.ContentType, ct);

                return new UploadFileResponse(fileId.ToString());
            }
            catch
            {
                return new UploadFileResponse(null);
            }
        }
    }
}
