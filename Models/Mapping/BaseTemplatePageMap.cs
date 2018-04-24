using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseTemplatePageMap : EntityTypeConfiguration<BaseTemplatePage>
    {
        public BaseTemplatePageMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            // Table & Column Mappings
            this.ToTable("BaseTemplatePages");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.templateID).HasColumnName("templateID");
            this.Property(t => t.pageNumber).HasColumnName("pageNumber");

            // Relationships
            this.HasRequired(t => t.BaseTemplate)
                .WithMany(t => t.BaseTemplatePages)
                .HasForeignKey(d => d.templateID);

        }
    }
}
