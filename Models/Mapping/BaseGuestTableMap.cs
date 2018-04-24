using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseGuestTableMap : EntityTypeConfiguration<BaseGuestTable>
    {
        public BaseGuestTableMap()
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

            this.Property(t => t.TABLE_TYPE)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("BaseGuestTable");
            this.Property(t => t.TABLE_CATALOG).HasColumnName("TABLE_CATALOG");
            this.Property(t => t.TABLE_SCHEMA).HasColumnName("TABLE_SCHEMA");
            this.Property(t => t.TABLE_NAME).HasColumnName("TABLE_NAME");
            this.Property(t => t.TABLE_TYPE).HasColumnName("TABLE_TYPE");
        }
    }
}
