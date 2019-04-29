using System;
using System.Collections.Generic;

namespace Fox.DataAccess.Models
{
    public partial class Reserva
    {
        public Reserva()
        {
            Compra = new HashSet<Compra>();
            ReservaStock = new HashSet<ReservaStock>();
        }

        public long Id { get; set; }
        public string Creacion { get; set; }
        public string Usuario { get; set; }

        public virtual Usuario UsuarioNavigation { get; set; }
        public virtual ICollection<Compra> Compra { get; set; }
        public virtual ICollection<ReservaStock> ReservaStock { get; set; }
    }
}
