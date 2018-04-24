using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseGuestColumnMap : EntityTypeConfiguration<BaseGuestColumn>
    {
        public BaseGuestColumnMap()
        {
            // Primary Key
            this.HasKey(t => t.TABLE_NAME);

            // Properties
            this.Property(t => t.TABLE_CATALOG)
                .HasMaxLength(128);

            this.Property(t => t.TABLE_SCHEMA)
                .HasMaxLength(128);

            this.Property(t => t.TABLE_NAME)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.COLUMN_NAME)
                .HasMaxLength(128);

            this.Property(t => t.COLUMN_DEFAULT)
                .HasMaxLength(4000);

            this.Property(t => t.IS_NULLABLE)
                .HasMaxLength(3);

            this.Property(t => t.DATA_TYPE)
                .HasMaxLength(128);

            this.Property(t => t.CHARACTER_SET_CATALOG)
                .HasMaxLength(128);

            this.Property(t => t.CHARACTER_SET_SCHEMA)
                .HasMaxLength(128);

            this.Property(t => t.CHARACTER_SET_NAME)
                .HasMaxLength(128);

            this.Property(t => t.COLLATION_CATALOG)
                .HasMaxLength(128);

            this.Property(t => t.COLLATION_SCHEMA)
                .HasMaxLength(128);

            this.Property(t => t.COLLATION_NAME)
                .HasMaxLength(128);

            this.Property(t => t.DOMAIN_CATALOG)
                .HasMaxLength(128);

            this.Property(t => t.DOMAIN_SCHEMA)
                .HasMaxLength(128);

            this.Property(t => t.DOMAIN_NAME)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("BaseGuestColumn");
            this.Property(t => t.TABLE_CATALOG).HasColumnName("TABLE_CATALOG");
            this.Property(t => t.TABLE_SCHEMA).HasColumnName("TABLE_SCHEMA");
            this.Property(t => t.TABLE_NAME).HasColumnName("TABLE_NAME");
            this.Property(t => t.COLUMN_NAME).HasColumnName("COLUMN_NAME");
            this.Property(t => t.ORDINAL_POSITION).HasColumnName("ORDINAL_POSITION");
            this.Property(t => t.COLUMN_DEFAULT).HasColumnName("COLUMN_DEFAULT");
            this.Property(t => t.IS_NULLABLE).HasColumnName("IS_NULLABLE");
            this.Property(t => t.DATA_TYPE).HasColumnName("DATA_TYPE");
            this.Property(t => t.CHARACTER_MAXIMUM_LENGTH).HasColumnName("CHARACTER_MAXIMUM_LENGTH");
            this.Property(t => t.CHARACTER_OCTET_LENGTH).HasColumnName("CHARACTER_OCTET_LENGTH");
            this.Property(t => t.NUMERIC_PRECISION).HasColumnName("NUMERIC_PRECISION");
            this.Property(t => t.NUMERIC_PRECISION_RADIX).HasColumnName("NUMERIC_PRECISION_RADIX");
            this.Property(t => t.NUMERIC_SCALE).HasColumnName("NUMERIC_SCALE");
            this.Property(t => t.DATETIME_PRECISION).HasColumnName("DATETIME_PRECISION");
            this.Property(t => t.CHARACTER_SET_CATALOG).HasColumnName("CHARACTER_SET_CATALOG");
            this.Property(t => t.CHARACTER_SET_SCHEMA).HasColumnName("CHARACTER_SET_SCHEMA");
            this.Property(t => t.CHARACTER_SET_NAME).HasColumnName("CHARACTER_SET_NAME");
            this.Property(t => t.COLLATION_CATALOG).HasColumnName("COLLATION_CATALOG");
            this.Property(t => t.COLLATION_SCHEMA).HasColumnName("COLLATION_SCHEMA");
            this.Property(t => t.COLLATION_NAME).HasColumnName("COLLATION_NAME");
            this.Property(t => t.DOMAIN_CATALOG).HasColumnName("DOMAIN_CATALOG");
            this.Property(t => t.DOMAIN_SCHEMA).HasColumnName("DOMAIN_SCHEMA");
            this.Property(t => t.DOMAIN_NAME).HasColumnName("DOMAIN_NAME");
        }
    }
}
