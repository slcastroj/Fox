using System;
using System.Collections.Generic;

namespace Fox.DataAccess.Models
{
    public partial class Producto
    {
        public Producto()
        {
            ReservaStock = new HashSet<ReservaStock>();
        }

        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string NecesitaReceta { get; set; }
        public long MaximoSemanal { get; set; }
        public long Stock { get; set; }
        public long PesoGr { get; set; }
        public long PrecioUnidad { get; set; }
        public long Laboratorio { get; set; }
        public long Tipo { get; set; }

        public virtual Laboratorio LaboratorioNavigation { get; set; }
        public virtual TipoProducto TipoNavigation { get; set; }
        public virtual ICollection<ReservaStock> ReservaStock { get; set; }
    }
}
