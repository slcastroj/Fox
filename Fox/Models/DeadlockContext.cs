using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;

namespace Fox.Models
{
    public partial class DeadlockContext : DbContext
    {
        private AppSettings Settings { get; }
        public DeadlockContext(IOptions<AppSettings> set)
        {
            Settings = set.Value;
        }

        public DeadlockContext(DbContextOptions<DeadlockContext> options, IOptions<AppSettings> set)
            : base(options)
        {
            Settings = set.Value;
        }

        public virtual DbSet<EstadoCompra> EstadoCompra { get; set; }
        public virtual DbSet<EstadoSoporte> EstadoSoporte { get; set; }
        public virtual DbSet<Laboratorio> Laboratorio { get; set; }
        public virtual DbSet<Notificacion> Notificacion { get; set; }
        public virtual DbSet<OrdenCompra> OrdenCompra { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<SolicitudReserva> SolicitudReserva { get; set; }
        public virtual DbSet<TicketSoporte> TicketSoporte { get; set; }
        public virtual DbSet<TipoNotificacion> TipoNotificacion { get; set; }
        public virtual DbSet<TipoProducto> TipoProducto { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("DataSource=" + Settings.DataSource);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<EstadoCompra>(entity =>
            {
                entity.ToTable("ESTADO_COMPRA");

                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.HasIndex(e => e.Nombre)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("VARCHAR");
            });

            modelBuilder.Entity<EstadoSoporte>(entity =>
            {
                entity.ToTable("ESTADO_SOPORTE");

                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.HasIndex(e => e.Nombre)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("VARCHAR");
            });

            modelBuilder.Entity<Laboratorio>(entity =>
            {
                entity.ToTable("LABORATORIO");

                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.HasIndex(e => e.Nombre)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("VARCHAR");
            });

            modelBuilder.Entity<Notificacion>(entity =>
            {
                entity.ToTable("NOTIFICACION");

                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Asunto)
                    .IsRequired()
                    .HasColumnName("asunto")
                    .HasColumnType("VARCHAR (40)");

                entity.Property(e => e.EstaVisto)
                    .IsRequired()
                    .HasColumnName("esta_visto")
                    .HasColumnType("BOOLEAN");

                entity.Property(e => e.Link)
                    .IsRequired()
                    .HasColumnName("link")
                    .HasColumnType("VARCHAR (60)");

                entity.Property(e => e.Mensaje)
                    .IsRequired()
                    .HasColumnName("mensaje")
                    .HasColumnType("VARCHAR (400)");

                entity.Property(e => e.Tipo).HasColumnName("tipo");

                entity.Property(e => e.Usuario)
                    .HasColumnName("usuario")
                    .HasColumnType("VARCHAR (8)");

                entity.HasOne(d => d.TipoNavigation)
                    .WithMany(p => p.Notificacion)
                    .HasForeignKey(d => d.Tipo)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.Notificacion)
                    .HasForeignKey(d => d.Usuario);
            });

            modelBuilder.Entity<OrdenCompra>(entity =>
            {
                entity.ToTable("ORDEN_COMPRA");

                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Asignado)
                    .HasColumnName("asignado")
                    .HasColumnType("VARCHAR (8)");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.Producto).HasColumnName("producto");

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasColumnName("usuario")
                    .HasColumnType("VARCHAR (10)");

                entity.HasOne(d => d.AsignadoNavigation)
                    .WithMany(p => p.OrdenCompraAsignadoNavigation)
                    .HasForeignKey(d => d.Asignado);

                entity.HasOne(d => d.EstadoNavigation)
                    .WithMany(p => p.OrdenCompra)
                    .HasForeignKey(d => d.Estado)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ProductoNavigation)
                    .WithMany(p => p.OrdenCompra)
                    .HasForeignKey(d => d.Producto)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.OrdenCompraUsuarioNavigation)
                    .HasForeignKey(d => d.Usuario)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("PRODUCTO");

                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.HasIndex(e => e.Nombre)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("descripcion")
                    .HasColumnType("VARCHAR (400)");

                entity.Property(e => e.Laboratorio).HasColumnName("laboratorio");

                entity.Property(e => e.MaximoSemanal)
                    .HasColumnName("maximo_semanal")
                    .HasColumnType("INTEGER (2)");

                entity.Property(e => e.NecesitaReceta)
                    .IsRequired()
                    .HasColumnName("necesita_receta")
                    .HasColumnType("BOOLEAN");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("VARCHAR (50)");

                entity.Property(e => e.PesoGr).HasColumnName("peso_gr");

                entity.Property(e => e.PrecioUnidad)
                    .HasColumnName("precio_unidad")
                    .HasColumnType("INTEGER (6)");

                entity.Property(e => e.Stock)
                    .HasColumnName("stock")
                    .HasColumnType("INTEGER (4)");

                entity.Property(e => e.Tipo).HasColumnName("tipo");

                entity.HasOne(d => d.LaboratorioNavigation)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.Laboratorio)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TipoNavigation)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.Tipo)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<SolicitudReserva>(entity =>
            {
                entity.ToTable("SOLICITUD_RESERVA");

                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Producto).HasColumnName("producto");

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasColumnName("usuario")
                    .HasColumnType("VARCHAR (8)");

                entity.HasOne(d => d.ProductoNavigation)
                    .WithMany(p => p.SolicitudReserva)
                    .HasForeignKey(d => d.Producto)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.SolicitudReserva)
                    .HasForeignKey(d => d.Usuario)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TicketSoporte>(entity =>
            {
                entity.ToTable("TICKET_SOPORTE");

                entity.HasIndex(e => e.Consultante)
                    .IsUnique();

                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Asignado)
                    .HasColumnName("asignado")
                    .HasColumnType("VARCHAR (8)");

                entity.Property(e => e.Consultante)
                    .IsRequired()
                    .HasColumnName("consultante")
                    .HasColumnType("VARCHAR (60)");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.HasOne(d => d.AsignadoNavigation)
                    .WithMany(p => p.TicketSoporte)
                    .HasForeignKey(d => d.Asignado);

                entity.HasOne(d => d.EstadoNavigation)
                    .WithMany(p => p.TicketSoporte)
                    .HasForeignKey(d => d.Estado)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TipoNotificacion>(entity =>
            {
                entity.ToTable("TIPO_NOTIFICACION");

                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.HasIndex(e => e.Nombre)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("VARCHAR");
            });

            modelBuilder.Entity<TipoProducto>(entity =>
            {
                entity.ToTable("TIPO_PRODUCTO");

                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.HasIndex(e => e.Nombre)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("VARCHAR");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Rut);

                entity.ToTable("USUARIO");

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.Property(e => e.Rut)
                    .HasColumnName("rut")
                    .HasColumnType("VARCHAR (8)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("VARCHAR (40)");

                entity.Property(e => e.FechaNacimiento)
                    .IsRequired()
                    .HasColumnName("fecha_nacimiento")
                    .HasColumnType("DATE");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password");

                entity.Property(e => e.Roles)
                    .IsRequired()
                    .HasColumnName("roles")
                    .HasColumnType("VARCHAR");

                entity.Property(e => e.Token).HasColumnName("token");
            });
        }
    }
}
