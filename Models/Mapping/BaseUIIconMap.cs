using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseUIIconMap : EntityTypeConfiguration<BaseUIIcon>
    {
        public BaseUIIconMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.css)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("BaseUIIcon");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.css).HasColumnName("css");
        }
    }
}
