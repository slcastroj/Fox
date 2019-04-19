﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Fox.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Notificacion = new HashSet<Notificacion>();
            OrdenCompraAsignadoNavigation = new HashSet<OrdenCompra>();
            OrdenCompraUsuarioNavigation = new HashSet<OrdenCompra>();
            SolicitudReserva = new HashSet<SolicitudReserva>();
            TicketSoporte = new HashSet<TicketSoporte>();
        }

        public string Rut { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string FechaNacimiento { get; set; }
        public string Roles { get; set; }

        public virtual ICollection<Notificacion> Notificacion { get; set; }
        public virtual ICollection<OrdenCompra> OrdenCompraAsignadoNavigation { get; set; }
        public virtual ICollection<OrdenCompra> OrdenCompraUsuarioNavigation { get; set; }
        public virtual ICollection<SolicitudReserva> SolicitudReserva { get; set; }
        public virtual ICollection<TicketSoporte> TicketSoporte { get; set; }

        public static String ComputeHash(String pwd)
        {
            using (var sha = SHA256.Create())
            {
                var hashedPwd = sha.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                return BitConverter.ToString(hashedPwd).Replace("-", "").ToLower();
            }
        }
    }
}