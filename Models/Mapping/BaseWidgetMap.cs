using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseWidgetMap : EntityTypeConfiguration<BaseWidget>
    {
        public BaseWidgetMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.description)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.actionContent)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("BaseWidget");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.actionID).HasColumnName("actionID");
            this.Property(t => t.width).HasColumnName("width");
            this.Property(t => t.actionContent).HasColumnName("actionContent");
            this.Property(t => t.html).HasColumnName("html");
            this.Property(t => t.hasRange).HasColumnName("hasRange");

            // Relationships
            this.HasMany(t => t.BaseUsers)
                .WithMany(t => t.BaseWidgets)
                .Map(m =>
                    {
                        m.ToTable("BaseUserWidget");
                        m.MapLeftKey("widgetID");
                        m.MapRightKey("userID");
                    });

            this.HasMany(t => t.BaseGraphs)
                .WithMany(t => t.BaseWidgets)
                .Map(m =>
                    {
                        m.ToTable("BaseWidgetGraph");
                        m.MapLeftKey("WidgetID");
                        m.MapRightKey("GraphsID");
                    });

            this.HasMany(t => t.BaseUsers1)
                .WithMany(t => t.BaseWidgets1)
                .Map(m =>
                    {
                        m.ToTable("BaseWidgetUser");
                        m.MapLeftKey("WidgetID");
                        m.MapRightKey("UserID");
                    });

            this.HasOptional(t => t.BaseAction)
                .WithMany(t => t.BaseWidgets)
                .HasForeignKey(d => d.actionID);

        }
    }
}
