using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWWidgetGraphMap : EntityTypeConfiguration<VWWidgetGraph>
    {
        public VWWidgetGraphMap()
        {
            // Primary Key
            this.HasKey(t => new { t.id, t.name, t.description });

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.description)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.url)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("VWWidgetGraph");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.url).HasColumnName("url");
            this.Property(t => t.Graph).HasColumnName("Graph");
        }
    }
}
