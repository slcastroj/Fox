using System;
using System.Collections.Generic;

namespace Fox.DataAccess.Models
{
    public partial class OrdenCompra
    {
        public long Id { get; set; }
        public string Usuario { get; set; }
        public string Asignado { get; set; }
        public long Cantidad { get; set; }
        public long Estado { get; set; }
        public long Producto { get; set; }

        public virtual Usuario AsignadoNavigation { get; set; }
        public virtual EstadoCompra EstadoNavigation { get; set; }
        public virtual Producto ProductoNavigation { get; set; }
        public virtual Usuario UsuarioNavigation { get; set; }
    }
}
