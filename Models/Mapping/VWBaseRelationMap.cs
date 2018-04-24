using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWBaseRelationMap : EntityTypeConfiguration<VWBaseRelation>
    {
        public VWBaseRelationMap()
        {
            // Primary Key
            this.HasKey(t => new { t.child, t.type });

            // Properties
            this.Property(t => t.parent)
                .HasMaxLength(128);

            this.Property(t => t.child)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.fk)
                .HasMaxLength(128);

            this.Property(t => t.type)
                .IsRequired()
                .HasMaxLength(8);

            // Table & Column Mappings
            this.ToTable("VWBaseRelations");
            this.Property(t => t.parent).HasColumnName("parent");
            this.Property(t => t.child).HasColumnName("child");
            this.Property(t => t.fk).HasColumnName("fk");
            this.Property(t => t.type).HasColumnName("type");
        }
    }
}
