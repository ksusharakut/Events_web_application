using Application.Common;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            // Предположим, что изображения находятся в папке wwwroot/images
            var imageUrl = Path.Combine("images", Path.GetFileName(imagePath));
            return $"{_webHostEnvironment.WebRootPath}/{imageUrl}";
        }
    }
}
