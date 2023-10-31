namespace WebApplication1.Interfaces.Repositories
{
    public interface IFileService
    {
        public Task<string> Upload(IFormFile file);

        public Task<byte[]?> GetByName(string name);

        public Task<string?> Remove(string fileName);
    }
}
