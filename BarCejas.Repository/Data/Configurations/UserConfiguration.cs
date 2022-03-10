using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BarCejas.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using BarCejas.Core.Enumerations;

namespace BarCejas.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Usuario");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
               .HasColumnName("IdUsuario");

            builder.Property(e => e.IsActive)
                .HasColumnName("Activo");

            builder.Property(e => e.FirstName)
                .HasColumnName("Nombres")
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode(false);
            
            builder.Property(e => e.LastName)
                .HasColumnName("Apellidos")
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode(false);

            builder.Property(e => e.Gender)
                .HasColumnName("Genero")
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.DateOfBrith)
                .HasColumnName("FechaNacimiento")
                .HasColumnType("date");

            builder.Property(e => e.Telephone)
                .HasColumnName("Telefono")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Password)
                .HasColumnName("Clave")
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.Role)
                .HasColumnName("Rol")
                .IsRequired()
                .HasMaxLength(15)
                .HasConversion(
                    x => x.ToString(),
                    x => (RoleType)Enum.Parse(typeof(RoleType), x)
                );
        }
    }
}
