using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWHAWKColumnMap : EntityTypeConfiguration<VWHAWKColumn>
    {
        public VWHAWKColumnMap()
        {
            // Primary Key
            this.HasKey(t => new { t.TABLE_NAME, t.RELATION_TYPE });

            // Properties
            this.Property(t => t.TABLE_NAME)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.COLUMN_NAME)
                .HasMaxLength(128);

            this.Property(t => t.IS_NULLABLE)
                .HasMaxLength(3);

            this.Property(t => t.DATA_TYPE)
                .HasMaxLength(128);

            this.Property(t => t.RELATION_TYPE)
                .IsRequired()
                .HasMaxLength(6);

            this.Property(t => t.RELATION)
                .HasMaxLength(515);

            // Table & Column Mappings
            this.ToTable("VWHAWKColumn");
            this.Property(t => t.TABLE_NAME).HasColumnName("TABLE_NAME");
            this.Property(t => t.COLUMN_NAME).HasColumnName("COLUMN_NAME");
            this.Property(t => t.IS_NULLABLE).HasColumnName("IS_NULLABLE");
            this.Property(t => t.DATA_TYPE).HasColumnName("DATA_TYPE");
            this.Property(t => t.CHARACTER_MAXIMUM_LENGTH).HasColumnName("CHARACTER_MAXIMUM_LENGTH");
            this.Property(t => t.NUMERIC_PRECISION).HasColumnName("NUMERIC_PRECISION");
            this.Property(t => t.NUMERIC_SCALE).HasColumnName("NUMERIC_SCALE");
            this.Property(t => t.RELATION_TYPE).HasColumnName("RELATION_TYPE");
            this.Property(t => t.RELATION).HasColumnName("RELATION");
        }
    }
}
