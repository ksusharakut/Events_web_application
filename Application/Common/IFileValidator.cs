using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public interface IFileValidator
    {
        bool BeAValidFileType(IFormFile file);
        bool BeUnderMaxSize(IFormFile file);
    }
}
