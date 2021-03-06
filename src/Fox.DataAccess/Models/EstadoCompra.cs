﻿using System;
using System.Collections.Generic;

namespace Fox.DataAccess.Models
{
    public partial class EstadoCompra
    {
        public EstadoCompra()
        {
            Compra = new HashSet<Compra>();
        }

        public long Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Compra> Compra { get; set; }
    }
}
