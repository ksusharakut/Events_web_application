using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public interface IAgeValidationService
    {
        bool IsAtLeast18YearsOld(DateTime birthDay);
    }
}
