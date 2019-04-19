using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fox.Models
{
    public class RequestError
    {
        public String Message { get; set; }

        public RequestError(String msg)
        {
            Message = msg;
        }
    }
}
