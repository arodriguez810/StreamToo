using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWUserStatuMap : EntityTypeConfiguration<VWUserStatu>
    {
        public VWUserStatuMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Nombre, t.Estado, t.ID });

            // Properties
            this.Property(t => t.Nombre)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.Primer_Apellido)
                .HasMaxLength(30);

            this.Property(t => t.Segundo_Apellido)
                .HasMaxLength(30);

            this.Property(t => t.Estado)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Identificacion)
                .HasMaxLength(20);

            this.Property(t => t.País)
                .HasMaxLength(50);

            this.Property(t => t.Celular)
                .HasMaxLength(20);

            this.Property(t => t.Teléfono)
                .HasMaxLength(20);

            this.Property(t => t.Correo_Electrónico)
                .HasMaxLength(100);

            this.Property(t => t.Última_IP)
                .HasMaxLength(20);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("VWUserStatus");
            this.Property(t => t.Nombre).HasColumnName("Nombre");
            this.Property(t => t.Primer_Apellido).HasColumnName("Primer Apellido");
            this.Property(t => t.Segundo_Apellido).HasColumnName("Segundo Apellido");
            this.Property(t => t.Estado).HasColumnName("Estado");
            this.Property(t => t.Identificacion).HasColumnName("Identificacion");
            this.Property(t => t.País).HasColumnName("País");
            this.Property(t => t.Celular).HasColumnName("Celular");
            this.Property(t => t.Teléfono).HasColumnName("Teléfono");
            this.Property(t => t.Correo_Electrónico).HasColumnName("Correo Electrónico");
            this.Property(t => t.Última_IP).HasColumnName("Última IP");
            this.Property(t => t.ID).HasColumnName("ID");
        }
    }
}
