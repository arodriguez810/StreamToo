using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class RDCategoryMap : EntityTypeConfiguration<RDCategory>
    {
        public RDCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.description)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("RDCategory");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.active).HasColumnName("active");
            this.Property(t => t.creationDate).HasColumnName("creationDate");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.description).HasColumnName("description");
        }
    }
}
