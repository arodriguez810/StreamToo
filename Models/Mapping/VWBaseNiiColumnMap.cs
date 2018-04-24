using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWBaseNiiColumnMap : EntityTypeConfiguration<VWBaseNiiColumn>
    {
        public VWBaseNiiColumnMap()
        {
            // Primary Key
            this.HasKey(t => new { t.column_id, t.max_length, t.precision, t.scale, t.is_identity, t.is_computed, t.tableName, t.type });

            // Properties
            this.Property(t => t.column_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.name)
                .HasMaxLength(128);

            this.Property(t => t.max_length)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.tableName)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.type)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("VWBaseNiiColumns");
            this.Property(t => t.column_id).HasColumnName("column_id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.max_length).HasColumnName("max_length");
            this.Property(t => t.precision).HasColumnName("precision");
            this.Property(t => t.scale).HasColumnName("scale");
            this.Property(t => t.is_nullable).HasColumnName("is_nullable");
            this.Property(t => t.is_identity).HasColumnName("is_identity");
            this.Property(t => t.is_computed).HasColumnName("is_computed");
            this.Property(t => t.tableName).HasColumnName("tableName");
            this.Property(t => t.type).HasColumnName("type");
        }
    }
}
