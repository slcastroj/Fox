using System;
using System.Collections.Generic;

namespace Fox.DataAccess.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Reserva = new HashSet<Reserva>();
        }

        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FechaNacimiento { get; set; }
        public long Rol { get; set; }

        public virtual Rol RolNavigation { get; set; }
        public virtual ICollection<Reserva> Reserva { get; set; }
    }
}
