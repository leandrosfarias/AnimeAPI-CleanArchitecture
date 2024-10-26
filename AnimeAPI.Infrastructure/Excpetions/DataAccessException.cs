using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeAPI.Infrastructure.Excpetions;
class DataAccessException : Exception
{
    public DataAccessException()
    {
    }

    public DataAccessException(string message) : base(message) { }

    public DataAccessException(string message, Exception innerException) : base(message, innerException) { }
}
