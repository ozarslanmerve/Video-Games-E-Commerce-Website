using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using VideoGames.Business.FileManagement.Abstract;

namespace VideoGames.Business.FileManagement.Concrete
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Yeni resmi kaydeder, eğer eski dosya yolu varsa onu siler.
        /// </summary>
        public async Task<string> SaveImageAsync(IFormFile imageFile, string existingFilePath = null)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Geçersiz dosya.");

            // images klasörü
            var folderPath = Path.Combine(_env.WebRootPath, "images");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Yeni dosya ismi
            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            // Yeni resmi kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Eski resmi sil (varsa)
            if (!string.IsNullOrEmpty(existingFilePath))
            {
                DeleteImage(existingFilePath);
            }

            return Path.Combine("images", fileName).Replace("\\", "/");
        }

        /// <summary>
        /// Belirtilen dosya yolundaki resmi siler.
        /// </summary>
        public void DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return;

            var fullPath = Path.Combine(_env.WebRootPath, imagePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
