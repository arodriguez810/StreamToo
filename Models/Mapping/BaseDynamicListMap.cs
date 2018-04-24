using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseDynamicListMap : EntityTypeConfiguration<BaseDynamicList>
    {
        public BaseDynamicListMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.nameToDisplay)
                .HasMaxLength(100);

            this.Property(t => t.cookieKey)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("BaseDynamicList");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.nameToDisplay).HasColumnName("nameToDisplay");
            this.Property(t => t.cookieKey).HasColumnName("cookieKey");
            this.Property(t => t.allowSaveConfig).HasColumnName("allowSaveConfig");
            this.Property(t => t.allowExport).HasColumnName("allowExport");
            this.Property(t => t.enableControl).HasColumnName("enableControl");
            this.Property(t => t.allowLog).HasColumnName("allowLog");
            this.Property(t => t.audit).HasColumnName("audit");
        }
    }
}
