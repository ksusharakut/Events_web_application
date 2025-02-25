using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NotRegisteredException : Exception
    {
        public NotRegisteredException(string message) : base(message) { }

        public NotRegisteredException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
