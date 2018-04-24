using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWBaseColumnDescriptionMap : EntityTypeConfiguration<VWBaseColumnDescription>
    {
        public VWBaseColumnDescriptionMap()
        {
            // Primary Key
            this.HasKey(t => t.Table);

            // Properties
            this.Property(t => t.Table)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Column)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("VWBaseColumnDescription");
            this.Property(t => t.Table).HasColumnName("Table");
            this.Property(t => t.Column).HasColumnName("Column");
        }
    }
}
