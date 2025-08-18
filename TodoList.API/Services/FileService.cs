using TodoList.API.Services.Interfaces;

namespace TodoList.API.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string subFolder)
        {
            var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new InvalidOperationException("Invalid file type. Only .jpg, .png, and .jpeg files are allowed.");
            }

            if (file.Length > 5 * 1024 * 1024)
            {
                throw new InvalidOperationException("File size exceeds the maximum limit of 5 MB.");
            }

            var uniqeFileName = Guid.NewGuid().ToString() + fileExtension;
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", subFolder);
            var filePath = Path.Combine(uploadsFolder, uniqeFileName);

            Directory.CreateDirectory(uploadsFolder);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"uploads/{subFolder}/{uniqeFileName}";
        }

        public void DeleteFileAsync(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) { return; }

            var pathToDelete = relativePath.TrimStart('/');
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
