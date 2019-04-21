using System;
using System.Collections.Generic;

namespace Fox.DataAccess.Models
{
    public partial class EstadoCompra
    {
        public EstadoCompra()
        {
            OrdenCompra = new HashSet<OrdenCompra>();
        }

        public long Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<OrdenCompra> OrdenCompra { get; set; }
    }
}
