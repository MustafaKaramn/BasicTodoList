namespace TodoList.API.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string subFolder);
        void DeleteFileAsync(string? relativePath);
    }
}
