using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseSystemTableMap : EntityTypeConfiguration<BaseSystemTable>
    {
        public BaseSystemTableMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.tableName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.why)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("BaseSystemTable");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.tableName).HasColumnName("tableName");
            this.Property(t => t.why).HasColumnName("why");
        }
    }
}
