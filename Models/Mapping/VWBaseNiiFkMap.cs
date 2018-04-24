using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWBaseNiiFkMap : EntityTypeConfiguration<VWBaseNiiFk>
    {
        public VWBaseNiiFkMap()
        {
            // Primary Key
            this.HasKey(t => new { t.parent, t.child });

            // Properties
            this.Property(t => t.parent)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.child)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.parentColumn)
                .HasMaxLength(128);

            this.Property(t => t.parentChild)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("VWBaseNiiFks");
            this.Property(t => t.parent).HasColumnName("parent");
            this.Property(t => t.child).HasColumnName("child");
            this.Property(t => t.parentColumn).HasColumnName("parentColumn");
            this.Property(t => t.parentChild).HasColumnName("parentChild");
        }
    }
}
