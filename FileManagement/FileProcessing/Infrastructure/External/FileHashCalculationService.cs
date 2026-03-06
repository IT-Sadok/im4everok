
using System.Security.Cryptography;

using Application.Interfaces.External;

namespace Infrastructure.External
{
    internal class FileHashCalculationService : IFileHashCalculationService
    {
        public string CalculateHash(Stream file)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(file);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
