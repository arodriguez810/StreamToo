using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWDataSourceColumnMap : EntityTypeConfiguration<VWDataSourceColumn>
    {
        public VWDataSourceColumnMap()
        {
            // Primary Key
            this.HasKey(t => new { t.viewId, t.columnId, t.viewName });

            // Properties
            this.Property(t => t.viewId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.columnId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.columnName)
                .HasMaxLength(128);

            this.Property(t => t.viewName)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("VWDataSourceColumns");
            this.Property(t => t.viewId).HasColumnName("viewId");
            this.Property(t => t.columnId).HasColumnName("columnId");
            this.Property(t => t.columnName).HasColumnName("columnName");
            this.Property(t => t.viewName).HasColumnName("viewName");
        }
    }
}
