using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWBaseProfileMap : EntityTypeConfiguration<VWBaseProfile>
    {
        public VWBaseProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.name)
                .HasMaxLength(50);

            this.Property(t => t.description)
                .HasMaxLength(200);

            this.Property(t => t.displayName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VWBaseProfile");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.displayName).HasColumnName("displayName");
        }
    }
}
