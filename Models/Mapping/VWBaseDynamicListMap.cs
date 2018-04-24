using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWBaseDynamicListMap : EntityTypeConfiguration<VWBaseDynamicList>
    {
        public VWBaseDynamicListMap()
        {
            // Primary Key
            this.HasKey(t => new { t.id, t.cookieKey, t.Save_Columns, t.Export, t.enableControl });

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.name)
                .HasMaxLength(100);

            this.Property(t => t.cookieKey)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Save_Columns)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.Export)
                .IsRequired()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("VWBaseDynamicList");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.cookieKey).HasColumnName("cookieKey");
            this.Property(t => t.Save_Columns).HasColumnName("Save Columns");
            this.Property(t => t.Export).HasColumnName("Export");
            this.Property(t => t.enableControl).HasColumnName("enableControl");
        }
    }
}
