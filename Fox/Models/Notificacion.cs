using System;
using System.Collections.Generic;

namespace Fox.Models
{
    public partial class Notificacion
    {
        public long Id { get; set; }
        public string Asunto { get; set; }
        public string Mensaje { get; set; }
        public long Tipo { get; set; }
        public string Link { get; set; }
        public string Usuario { get; set; }
        public string EstaVisto { get; set; }

        public virtual TipoNotificacion TipoNavigation { get; set; }
        public virtual Usuario UsuarioNavigation { get; set; }
    }
}
