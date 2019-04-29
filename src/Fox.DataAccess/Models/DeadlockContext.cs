using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;

namespace Fox.DataAccess.Models
{
    public partial class DeadlockContext : DbContext
    {
		private AppSettings Settings { get; }

		public DeadlockContext(IOptions<AppSettings> settings)
		{
			Settings = settings.Value;
		}

		public DeadlockContext(DbContextOptions<DeadlockContext> options, IOptions<AppSettings> settings)
			: base(options)
		{
			Settings = settings.Value;
		}

		public virtual DbSet<Compra> Compra { get; set; }
        public virtual DbSet<EstadoCompra> EstadoCompra { get; set; }
        public virtual DbSet<Laboratorio> Laboratorio { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<Reserva> Reserva { get; set; }
        public virtual DbSet<ReservaStock> ReservaStock { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<TipoProducto> TipoProducto { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder
					.UseLazyLoadingProxies()
					.UseSqlite($"DataSource={Settings.DataSource}");
			}
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Compra>(entity =>
            {
                entity.ToTable("COMPRA");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Creacion)
                    .IsRequired()
                    .HasColumnName("creacion")
                    .HasColumnType("DATETIME")
                    .HasDefaultValueSql("date('now')");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.Reserva).HasColumnName("reserva");

                entity.HasOne(d => d.EstadoNavigation)
                    .WithMany(p => p.Compra)
                    .HasForeignKey(d => d.Estado);

                entity.HasOne(d => d.ReservaNavigation)
                    .WithMany(p => p.Compra)
                    .HasForeignKey(d => d.Reserva);
            });

            modelBuilder.Entity<EstadoCompra>(entity =>
            {
                entity.ToTable("ESTADO_COMPRA");

                entity.HasIndex(e => e.Nombre)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("VARCHAR (20)");
            });

            modelBuilder.Entity<Laboratorio>(entity =>
            {
                entity.ToTable("LABORATORIO");

                entity.HasIndex(e => e.Nombre)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("VARCHAR (30)");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("PRODUCTO");

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

                entity.Property(e => e.MaximoSemanal).HasColumnName("maximo_semanal");

                entity.Property(e => e.NecesitaReceta)
                    .IsRequired()
                    .HasColumnName("necesita_receta")
                    .HasColumnType("BOOLEAN");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("VARCHAR (50)");

                entity.Property(e => e.PesoGr).HasColumnName("peso_gr");

                entity.Property(e => e.PrecioUnidad).HasColumnName("precio_unidad");

                entity.Property(e => e.Stock).HasColumnName("stock");

                entity.Property(e => e.Tipo).HasColumnName("tipo");

                entity.HasOne(d => d.LaboratorioNavigation)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.Laboratorio);

                entity.HasOne(d => d.TipoNavigation)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.Tipo);
            });

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.ToTable("RESERVA");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Creacion)
                    .IsRequired()
                    .HasColumnName("creacion")
                    .HasColumnType("DATETIME")
                    .HasDefaultValueSql("date('now')");

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasColumnName("usuario")
                    .HasColumnType("VARCHAR (9)");

                entity.HasOne(d => d.UsuarioNavigation)
                    .WithMany(p => p.Reserva)
                    .HasForeignKey(d => d.Usuario);
            });

            modelBuilder.Entity<ReservaStock>(entity =>
            {
                entity.ToTable("RESERVA_STOCK");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.Producto).HasColumnName("producto");

                entity.Property(e => e.Reserva).HasColumnName("reserva");

                entity.HasOne(d => d.ProductoNavigation)
                    .WithMany(p => p.ReservaStock)
                    .HasForeignKey(d => d.Producto);

                entity.HasOne(d => d.ReservaNavigation)
                    .WithMany(p => p.ReservaStock)
                    .HasForeignKey(d => d.Reserva);
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("ROL");

                entity.HasIndex(e => e.Nombre)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("VARCHAR (20)");
            });

            modelBuilder.Entity<TipoProducto>(entity =>
            {
                entity.ToTable("TIPO_PRODUCTO");

                entity.HasIndex(e => e.Nombre)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("VARCHAR (20)");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Rut);

                entity.ToTable("USUARIO");

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.HasIndex(e => e.Nombre)
                    .IsUnique();

                entity.Property(e => e.Rut)
                    .HasColumnName("rut")
                    .HasColumnType("VARCHAR (9)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("VARCHAR (40)");

                entity.Property(e => e.FechaNacimiento)
                    .IsRequired()
                    .HasColumnName("fecha_nacimiento")
                    .HasColumnType("DATE");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("VARCHAR (50)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("VARCHAR (64)");

                entity.Property(e => e.Rol).HasColumnName("rol");

                entity.HasOne(d => d.RolNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.Rol);
            });
        }
    }
}
