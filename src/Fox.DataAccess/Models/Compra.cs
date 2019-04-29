using System;
using System.Collections.Generic;

namespace Fox.DataAccess.Models
{
    public partial class Compra
    {
        public long Id { get; set; }
        public string Creacion { get; set; }
        public long Reserva { get; set; }
        public long Estado { get; set; }

        public virtual EstadoCompra EstadoNavigation { get; set; }
        public virtual Reserva ReservaNavigation { get; set; }
    }
}
