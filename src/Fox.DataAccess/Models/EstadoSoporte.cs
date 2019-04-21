using System;
using System.Collections.Generic;

namespace Fox.DataAccess.Models
{
    public partial class EstadoSoporte
    {
        public EstadoSoporte()
        {
            TicketSoporte = new HashSet<TicketSoporte>();
        }

        public long Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<TicketSoporte> TicketSoporte { get; set; }
    }
}
