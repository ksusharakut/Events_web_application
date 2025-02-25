using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string fileName, string storagePath, CancellationToken cancellationToken);
        Task DeleteFileAsync(string filePath, CancellationToken cancellationToken);
    }
}
