using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
