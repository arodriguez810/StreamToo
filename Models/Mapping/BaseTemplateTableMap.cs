using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseTemplateTableMap : EntityTypeConfiguration<BaseTemplateTable>
    {
        public BaseTemplateTableMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.alias)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("BaseTemplateTables");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.alias).HasColumnName("alias");
            this.Property(t => t.templateDataSourceID).HasColumnName("templateDataSourceID");

            // Relationships
            this.HasOptional(t => t.BaseTemplateDataSource)
                .WithMany(t => t.BaseTemplateTables)
                .HasForeignKey(d => d.templateDataSourceID);

        }
    }
}
