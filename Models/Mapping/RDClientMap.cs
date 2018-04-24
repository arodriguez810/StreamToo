using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class RDClientMap : EntityTypeConfiguration<RDClient>
    {
        public RDClientMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.email)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.lastName)
                .HasMaxLength(50);

            this.Property(t => t.location)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("RDClient");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.active).HasColumnName("active");
            this.Property(t => t.creationDate).HasColumnName("creationDate");
            this.Property(t => t.email).HasColumnName("email");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.lastName).HasColumnName("lastName");
            this.Property(t => t.birthDate).HasColumnName("birthDate");
            this.Property(t => t.location).HasColumnName("location");
            this.Property(t => t.device).HasColumnName("device");
        }
    }
}
