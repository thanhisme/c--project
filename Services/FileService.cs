using Azure.Storage.Blobs;
using System.Text.RegularExpressions;
using WebApplication1.Interfaces.Repositories;

namespace WebApplication1.Services
{
    public class FileService : IFileService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private static string _containerName = string.Empty;

        public FileService(BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _blobServiceClient = blobServiceClient;
            _containerName = configuration.GetSection("BlobContainerName").Value!;
        }

        private string NormalizeFileName(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string fileNameWithoutExtension = Regex.Replace(
                Path.GetFileNameWithoutExtension(fileName),
                @"[^a-zA-Z0-9_]",
                ""
            );

            return fileNameWithoutExtension + extension;
        }

        public async Task<string> Upload(IFormFile file)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileName = NormalizeFileName(file.FileName);
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient($"{timestamp}_{fileName}");

            await blobClient.UploadAsync(file.OpenReadStream());

            return blobClient.Uri.AbsoluteUri;
        }

        private async Task<BlobClient?> GetOne(string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(NormalizeFileName(fileName));

            if (!(await blobClient.ExistsAsync()))
            {
                return null;
            }

            return blobClient;
        }

        public async Task<byte[]?> GetByName(string fileName)
        {
            var blobClient = await GetOne(fileName);

            if (blobClient == null)
            {
                return null;
            }

            var downloadedContent = await blobClient.DownloadContentAsync();

            return downloadedContent.Value.Content.ToArray();
        }

        public async Task<string?> Remove(string fileName)
        {
            var blobClient = await GetOne(fileName);

            if (blobClient == null)
            {
                return null;
            }

            await blobClient.DeleteAsync();

            return fileName;
        }
    }
}
