﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BarCejas.Entities;

namespace BarCejas.Data.DataContext
{
    public partial class BardecejasContext : DbContext
    {
        public BardecejasContext()
        {
        }

        public BardecejasContext(DbContextOptions<BardecejasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Banner> Banner { get; set; }
        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<ContactoLocal> ContactoLocal { get; set; }
        public virtual DbSet<CredencialMercadoPago> CredencialMercadoPago { get; set; }
        public virtual DbSet<Dia> Dia { get; set; }
        public virtual DbSet<FormaPago> FormaPago { get; set; }
        public virtual DbSet<FormaPagoServicio> FormaPagoServicio { get; set; }
        public virtual DbSet<HistoricoIngresos> HistoricoIngresos { get; set; }
        public virtual DbSet<HorarioAtencionLocal> HorarioAtencionLocal { get; set; }
        public virtual DbSet<HorarioAtencionProfesional> HorarioAtencionProfesional { get; set; }
        public virtual DbSet<HorarioBloqueado> HorarioBloqueado { get; set; }
        public virtual DbSet<MediosContactoEmpresa> MediosContactoEmpresa { get; set; }
        public virtual DbSet<MensajeMasivo> MensajeMasivo { get; set; }
        public virtual DbSet<ModalidadPago> ModalidadPago { get; set; }
        public virtual DbSet<ModalidadPagoServicio> ModalidadPagoServicio { get; set; }
        public virtual DbSet<Novedades> Novedades { get; set; }
        public virtual DbSet<Orden> Orden { get; set; }
        public virtual DbSet<OrdenItem> OrdenItem { get; set; }
        public virtual DbSet<Paquete> Paquete { get; set; }
        public virtual DbSet<PreguntasFrecuentes> PreguntasFrecuentes { get; set; }
        public virtual DbSet<Profesional> Profesional { get; set; }
        public virtual DbSet<RolTipoUsuarioMensajeMasivo> RolTipoUsuarioMensajeMasivo { get; set; }
        public virtual DbSet<Servicio> Servicio { get; set; }
        public virtual DbSet<ServicioContactoLocal> ServicioContactoLocal { get; set; }
        public virtual DbSet<ServicioPaquete> ServicioPaquete { get; set; }
        public virtual DbSet<ServicioProfesional> ServicioProfesional { get; set; }
        public virtual DbSet<Testimonios> Testimonios { get; set; }
        public virtual DbSet<TipoRegistro> TipoRegistro { get; set; }
        public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer("Data Source=184.168.147.58,1433\\AMEDIASERVER;Initial Catalog=BarDeCejas;Persist Security Info=True;User ID=amediaRemoto;Password=#TxSgD!92");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Banner>(entity =>
            {
                entity.Property(e => e.EsActivo).HasDefaultValueSql("((1))");

                entity.Property(e => e.FechaAlta).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FechaModif).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.LinkBoton)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RutaImagenDestok)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RutaImagenMobile)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Texto)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TextoBoton)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Titulo)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasIndex(e => e.Nombre)
                    .HasName("UQ__Categori__75E3EFCF53FB4896")
                    .IsUnique();
            });

            modelBuilder.Entity<ContactoLocal>(entity =>
            {
                entity.Property(e => e.CoordenadaLatitud).IsUnicode(false);

                entity.Property(e => e.CoordenadaLongitud).IsUnicode(false);

                entity.Property(e => e.Direccion).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FechaAlta).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FechaModif).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Nombre).IsUnicode(false);

                entity.Property(e => e.Telefono).IsUnicode(false);
            });

            modelBuilder.Entity<CredencialMercadoPago>(entity =>
            {
                entity.Property(e => e.AccessToken)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ClientId)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ClientSecret)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Cuenta).IsUnicode(false);

                entity.Property(e => e.PublicKey)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RefreshToken)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Dia>(entity =>
            {
                entity.HasIndex(e => e.Nombre)
                    .HasName("UQ__Dia__75E3EFCFD12C69BB")
                    .IsUnique();

                entity.Property(e => e.Nombre).IsUnicode(false);
            });

            modelBuilder.Entity<FormaPago>(entity =>
            {
                entity.HasIndex(e => e.Nombre)
                    .HasName("UQ__FormaPag__75E3EFCFCB43CECF")
                    .IsUnique();
            });

            modelBuilder.Entity<FormaPagoServicio>(entity =>
            {
                entity.Property(e => e.EsActivo).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdFormaPagoNavigation)
                    .WithMany(p => p.FormaPagoServicio)
                    .HasForeignKey(d => d.IdFormaPago)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FormaPago__IdFor__1F98B2C1");

                entity.HasOne(d => d.IdServicioNavigation)
                    .WithMany(p => p.FormaPagoServicio)
                    .HasForeignKey(d => d.IdServicio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FormaPago__IdSer__208CD6FA");
            });

            modelBuilder.Entity<HistoricoIngresos>(entity =>
            {
                entity.Property(e => e.FechaIngreso).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<HorarioAtencionLocal>(entity =>
            {
                entity.HasKey(e => new { e.IdDia, e.IdContactoLocal })
                    .HasName("PK__HorarioA__E691D17CFCE74B43");

                entity.HasOne(d => d.IdContactoLocalNavigation)
                    .WithMany(p => p.HorarioAtencionLocal)
                    .HasForeignKey(d => d.IdContactoLocal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HorarioAt__IdCon__2180FB33");

                entity.HasOne(d => d.IdDiaNavigation)
                    .WithMany(p => p.HorarioAtencionLocal)
                    .HasForeignKey(d => d.IdDia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HorarioAt__IdDia__22751F6C");
            });

            modelBuilder.Entity<HorarioAtencionProfesional>(entity =>
            {
                entity.HasOne(d => d.IdDiaNavigation)
                    .WithMany(p => p.HorarioAtencionProfesional)
                    .HasForeignKey(d => d.IdDia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HorarioAt__IdDia__1F98B2C1");

                entity.HasOne(d => d.IdProfesionalNavigation)
                    .WithMany(p => p.HorarioAtencionProfesional)
                    .HasForeignKey(d => d.IdProfesional)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HorarioAt__IdPro__208CD6FA");
            });

            modelBuilder.Entity<HorarioBloqueado>(entity =>
            {
                entity.HasOne(d => d.IdProfesionalNavigation)
                    .WithMany(p => p.HorarioBloqueado)
                    .HasForeignKey(d => d.IdProfesional)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HorarioBloqueado_Profesional");
            });

            modelBuilder.Entity<MediosContactoEmpresa>(entity =>
            {
                entity.Property(e => e.LinkFacebook).HasDefaultValueSql("('')");

                entity.Property(e => e.LinkInstagram).HasDefaultValueSql("('')");

                entity.Property(e => e.Whatsapp).HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MensajeMasivo>(entity =>
            {
                entity.Property(e => e.Asunto).IsUnicode(false);

                entity.Property(e => e.FechaAlta).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FechaModificacion).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Mensaje).IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioAltaNavigation)
                    .WithMany(p => p.MensajeMasivoIdUsuarioAltaNavigation)
                    .HasForeignKey(d => d.IdUsuarioAlta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MensajeMa__IdUsu__25518C17");

                entity.HasOne(d => d.IdUsuarioModificacionNavigation)
                    .WithMany(p => p.MensajeMasivoIdUsuarioModificacionNavigation)
                    .HasForeignKey(d => d.IdUsuarioModificacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MensajeMa__IdUsu__2645B050");
            });

            modelBuilder.Entity<ModalidadPago>(entity =>
            {
                entity.HasIndex(e => e.Nombre)
                    .HasName("UQ__Modalida__75E3EFCF50B1CBFA")
                    .IsUnique();
            });

            modelBuilder.Entity<ModalidadPagoServicio>(entity =>
            {
                entity.Property(e => e.EsActivo).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdModalidadPagoNavigation)
                    .WithMany(p => p.ModalidadPagoServicio)
                    .HasForeignKey(d => d.IdModalidadPago)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Modalidad__IdMod__2739D489");

                entity.HasOne(d => d.IdServicioNavigation)
                    .WithMany(p => p.ModalidadPagoServicio)
                    .HasForeignKey(d => d.IdServicio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Modalidad__IdSer__282DF8C2");
            });

            modelBuilder.Entity<Novedades>(entity =>
            {
                entity.Property(e => e.Contenido).IsUnicode(false);

                entity.Property(e => e.Copete).IsUnicode(false);

                entity.Property(e => e.Link).IsUnicode(false);

                entity.Property(e => e.Medio).IsUnicode(false);

                entity.Property(e => e.RutaImagen).IsUnicode(false);

                entity.Property(e => e.Titulo).IsUnicode(false);
            });

            modelBuilder.Entity<Orden>(entity =>
            {
                entity.Property(e => e.FechaDeCreacion).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<OrdenItem>(entity =>
            {
                entity.Property(e => e.FechaDeCita).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Hora).IsUnicode(false);
            });

            modelBuilder.Entity<PreguntasFrecuentes>(entity =>
            {
                entity.Property(e => e.Pregunta).IsUnicode(false);

                entity.Property(e => e.Respuesta).IsUnicode(false);
            });

            modelBuilder.Entity<Profesional>(entity =>
            {
                entity.HasOne(d => d.IdContactoLocalNavigation)
                    .WithMany(p => p.Profesional)
                    .HasForeignKey(d => d.IdContactoLocal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Profesion__IdCon__29221CFB");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Profesional)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Profesion__IdUsu__2A164134");
            });

            modelBuilder.Entity<RolTipoUsuarioMensajeMasivo>(entity =>
            {
                entity.HasKey(e => new { e.IdMensajeMasivo, e.IdTipoUsuario })
                    .HasName("PK__RolTipoU__04903DD95A55FFF9");

                entity.Property(e => e.EsActivo).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdMensajeMasivoNavigation)
                    .WithMany(p => p.RolTipoUsuarioMensajeMasivo)
                    .HasForeignKey(d => d.IdMensajeMasivo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RolTipoUs__IdMen__2B0A656D");
            });

            modelBuilder.Entity<Servicio>(entity =>
            {
                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Servicio)
                    .HasForeignKey(d => d.IdCategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Servicio__IdCate__2BFE89A6");

                entity.HasOne(d => d.IdUsuarioAltaNavigation)
                    .WithMany(p => p.Servicio)
                    .HasForeignKey(d => d.IdUsuarioAlta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Servicio__IdUsua__2CF2ADDF");
            });

            modelBuilder.Entity<ServicioContactoLocal>(entity =>
            {
                entity.Property(e => e.EsActivo).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdContactoLocalNavigation)
                    .WithMany(p => p.ServicioContactoLocal)
                    .HasForeignKey(d => d.IdContactoLocal)
                    .HasConstraintName("FK__ServicioC__IdCon__2DE6D218");

                entity.HasOne(d => d.IdServicioNavigation)
                    .WithMany(p => p.ServicioContactoLocal)
                    .HasForeignKey(d => d.IdServicio)
                    .HasConstraintName("FK__ServicioC__IdSer__2EDAF651");
            });

            modelBuilder.Entity<ServicioPaquete>(entity =>
            {
                entity.Property(e => e.EsActivo).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdPaqueteNavigation)
                    .WithMany(p => p.ServicioPaquete)
                    .HasForeignKey(d => d.IdPaquete)
                    .HasConstraintName("FK__ServicioP__IdPaq__2FCF1A8A");

                entity.HasOne(d => d.IdServicioNavigation)
                    .WithMany(p => p.ServicioPaquete)
                    .HasForeignKey(d => d.IdServicio)
                    .HasConstraintName("FK__ServicioP__IdSer__30C33EC3");
            });

            modelBuilder.Entity<ServicioProfesional>(entity =>
            {
                entity.Property(e => e.EsActivo).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdProfesionalNavigation)
                    .WithMany(p => p.ServicioProfesional)
                    .HasForeignKey(d => d.IdProfesional)
                    .HasConstraintName("FK__ServicioP__IdPro__31B762FC");

                entity.HasOne(d => d.IdServicioNavigation)
                    .WithMany(p => p.ServicioProfesional)
                    .HasForeignKey(d => d.IdServicio)
                    .HasConstraintName("FK__ServicioP__IdSer__32AB8735");
            });

            modelBuilder.Entity<Testimonios>(entity =>
            {
                entity.Property(e => e.Autor).IsUnicode(false);

                entity.Property(e => e.RutaImagen).IsUnicode(false);

                entity.Property(e => e.Testimonio).IsUnicode(false);
            });

            modelBuilder.Entity<TipoRegistro>(entity =>
            {
                entity.HasIndex(e => e.Nombre)
                    .HasName("UQ__TipoRegi__75E3EFCFC202A383")
                    .IsUnique();
            });

            modelBuilder.Entity<TipoUsuario>(entity =>
            {
                entity.HasIndex(e => e.Nombre)
                    .HasName("UQ__TipoUsua__75E3EFCFE6BC068E")
                    .IsUnique();
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("UQ__Usuario__A9D105345C39C2F2")
                    .IsUnique();
            });

            OnModelCreatingGeneratedProcedures(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}