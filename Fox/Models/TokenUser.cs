using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fox.Models
{
    public class TokenUser
    {
        public String Username { get; set; }
        public String Token { get; set; }
        public DateTime Creacion { get; set; }
        public Int32 Expiracion { get; set; }
    }
}
