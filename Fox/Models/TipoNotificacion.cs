using System;
using System.Collections.Generic;

namespace Fox.Models
{
    public partial class TipoNotificacion
    {
        public TipoNotificacion()
        {
            Notificacion = new HashSet<Notificacion>();
        }

        public long Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Notificacion> Notificacion { get; set; }
    }
}
