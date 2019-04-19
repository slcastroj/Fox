using System;
using System.Collections.Generic;

namespace Fox.Models
{
    public partial class SolicitudReserva
    {
        public long Id { get; set; }
        public string Usuario { get; set; }
        public long Producto { get; set; }

        public virtual Producto ProductoNavigation { get; set; }
        public virtual Usuario UsuarioNavigation { get; set; }
    }
}
