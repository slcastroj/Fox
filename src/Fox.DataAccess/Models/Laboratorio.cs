using System;
using System.Collections.Generic;

namespace Fox.DataAccess.Models
{
    public partial class Laboratorio
    {
        public Laboratorio()
        {
            Producto = new HashSet<Producto>();
        }

        public long Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Producto> Producto { get; set; }
    }
}
