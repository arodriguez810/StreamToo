using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseBGColorMap : EntityTypeConfiguration<BaseBGColor>
    {
        public BaseBGColorMap()
        {
            // Primary Key
            this.HasKey(t => new { t.id, t.name, t.text });

            // Properties
            this.Property(t => t.id)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(19);

            this.Property(t => t.text)
                .IsRequired()
                .HasMaxLength(13);

            // Table & Column Mappings
            this.ToTable("BaseBGColors");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.text).HasColumnName("text");
        }
    }
}
