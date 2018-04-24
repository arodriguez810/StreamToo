using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseTemplateVariableMap : EntityTypeConfiguration<BaseTemplateVariable>
    {
        public BaseTemplateVariableMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            // Table & Column Mappings
            this.ToTable("BaseTemplateVariables");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.templateDataSourceID).HasColumnName("templateDataSourceID");
            this.Property(t => t.dataSourceColumnID).HasColumnName("dataSourceColumnID");
            this.Property(t => t.description).HasColumnName("description");

            // Relationships
            this.HasRequired(t => t.BaseTemplateDataSource)
                .WithMany(t => t.BaseTemplateVariables)
                .HasForeignKey(d => d.templateDataSourceID);

        }
    }
}
