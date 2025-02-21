using Application.Common;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Services
{
    public class ImagePathService : IImagePathService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImagePathService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string GetImageUrl(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return string.Empty;
            }

            var imageUrl = Path.Combine("images", Path.GetFileName(imagePath));
            return $"{_webHostEnvironment.WebRootPath}/{imageUrl}";
        }
    }
}
