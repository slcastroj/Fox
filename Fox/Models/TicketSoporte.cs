using System;
using System.Collections.Generic;

namespace Fox.Models
{
    public partial class TicketSoporte
    {
        public long Id { get; set; }
        public string Asignado { get; set; }
        public string Consultante { get; set; }
        public long Estado { get; set; }

        public virtual Usuario AsignadoNavigation { get; set; }
        public virtual EstadoSoporte EstadoNavigation { get; set; }
    }
}
