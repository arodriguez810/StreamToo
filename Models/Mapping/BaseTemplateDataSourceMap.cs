using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseTemplateDataSourceMap : EntityTypeConfiguration<BaseTemplateDataSource>
    {
        public BaseTemplateDataSourceMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            // Table & Column Mappings
            this.ToTable("BaseTemplateDataSources");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.templateID).HasColumnName("templateID");
            this.Property(t => t.dataSourceID).HasColumnName("dataSourceID");

            // Relationships
            this.HasRequired(t => t.BaseTemplate)
                .WithMany(t => t.BaseTemplateDataSources)
                .HasForeignKey(d => d.templateID);

        }
    }
}
