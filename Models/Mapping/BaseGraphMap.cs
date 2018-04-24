using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseGraphMap : EntityTypeConfiguration<BaseGraph>
    {
        public BaseGraphMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.title)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("BaseGraphs");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.queryGraphID).HasColumnName("queryGraphID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.title).HasColumnName("title");

            // Relationships
            this.HasMany(t => t.BaseTypesGraphs)
                .WithMany(t => t.BaseGraphs)
                .Map(m =>
                    {
                        m.ToTable("BaseGraphsTypes");
                        m.MapLeftKey("GraphID");
                        m.MapRightKey("typesGraphsID");
                    });

            this.HasMany(t => t.BaseUsers)
                .WithMany(t => t.BaseGraphs)
                .Map(m =>
                    {
                        m.ToTable("BaseUserGraph");
                        m.MapLeftKey("GraphsID");
                        m.MapRightKey("UserID");
                    });

            this.HasOptional(t => t.BaseQueryGraph)
                .WithMany(t => t.BaseGraphs)
                .HasForeignKey(d => d.queryGraphID);

        }
    }
}
