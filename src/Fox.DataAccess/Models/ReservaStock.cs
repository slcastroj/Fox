using System;
using System.Collections.Generic;

namespace Fox.DataAccess.Models
{
    public partial class ReservaStock
    {
        public long Id { get; set; }
        public long Cantidad { get; set; }
        public long Reserva { get; set; }
        public long Producto { get; set; }

        public virtual Producto ProductoNavigation { get; set; }
        public virtual Reserva ReservaNavigation { get; set; }
    }
}
