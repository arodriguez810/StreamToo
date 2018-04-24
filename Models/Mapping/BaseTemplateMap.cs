using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseTemplateMap : EntityTypeConfiguration<BaseTemplate>
    {
        public BaseTemplateMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.code)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.html)
                .IsRequired();

            this.Property(t => t.styles)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("BaseTemplates");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.code).HasColumnName("code");
            this.Property(t => t.html).HasColumnName("html");
            this.Property(t => t.isHeader).HasColumnName("isHeader");
            this.Property(t => t.styles).HasColumnName("styles");
        }
    }
}
