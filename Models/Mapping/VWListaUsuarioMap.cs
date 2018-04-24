using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWListaUsuarioMap : EntityTypeConfiguration<VWListaUsuario>
    {
        public VWListaUsuarioMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Nombre, t.username, t.Estado });

            // Properties
            this.Property(t => t.Nombre)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.Primer_Apellido)
                .HasMaxLength(30);

            this.Property(t => t.Segundo_Apellido)
                .HasMaxLength(30);

            this.Property(t => t.Fecha_de_Nacimiento)
                .HasMaxLength(10);

            this.Property(t => t.Identificacion)
                .HasMaxLength(20);

            this.Property(t => t.Calle_y_Número)
                .HasMaxLength(300);

            this.Property(t => t.sector)
                .HasMaxLength(50);

            this.Property(t => t.Ciudad)
                .HasMaxLength(30);

            this.Property(t => t.Provincia)
                .HasMaxLength(30);

            this.Property(t => t.País)
                .HasMaxLength(50);

            this.Property(t => t.Celular)
                .HasMaxLength(20);

            this.Property(t => t.Teléfono)
                .HasMaxLength(20);

            this.Property(t => t.Correo_Electrónico)
                .HasMaxLength(100);

            this.Property(t => t.username)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Estado)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Fecha_de_Registro)
                .HasMaxLength(10);

            this.Property(t => t.Última_Actualización)
                .HasMaxLength(10);

            this.Property(t => t.Última_Visita)
                .HasMaxLength(10);

            this.Property(t => t.Última_IP)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("VWListaUsuarios");
            this.Property(t => t.Nombre).HasColumnName("Nombre");
            this.Property(t => t.Primer_Apellido).HasColumnName("Primer Apellido");
            this.Property(t => t.Segundo_Apellido).HasColumnName("Segundo Apellido");
            this.Property(t => t.Fecha_de_Nacimiento).HasColumnName("Fecha de Nacimiento");
            this.Property(t => t.Identificacion).HasColumnName("Identificacion");
            this.Property(t => t.Calle_y_Número).HasColumnName("Calle y Número");
            this.Property(t => t.sector).HasColumnName("sector");
            this.Property(t => t.Ciudad).HasColumnName("Ciudad");
            this.Property(t => t.Provincia).HasColumnName("Provincia");
            this.Property(t => t.País).HasColumnName("País");
            this.Property(t => t.Celular).HasColumnName("Celular");
            this.Property(t => t.Teléfono).HasColumnName("Teléfono");
            this.Property(t => t.Correo_Electrónico).HasColumnName("Correo Electrónico");
            this.Property(t => t.username).HasColumnName("username");
            this.Property(t => t.Estado).HasColumnName("Estado");
            this.Property(t => t.Fecha_de_Registro).HasColumnName("Fecha de Registro");
            this.Property(t => t.Última_Actualización).HasColumnName("Última Actualización");
            this.Property(t => t.Última_Visita).HasColumnName("Última Visita");
            this.Property(t => t.Última_IP).HasColumnName("Última IP");
        }
    }
}
