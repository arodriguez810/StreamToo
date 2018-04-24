using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseProfileMap : EntityTypeConfiguration<BaseProfile>
    {
        public BaseProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(50);

            this.Property(t => t.description)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("BaseProfile");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.creationDate).HasColumnName("creationDate");
            this.Property(t => t.defaultActionID).HasColumnName("defaultActionID");

            // Relationships
            this.HasMany(t => t.BaseWidgets)
                .WithMany(t => t.BaseProfiles)
                .Map(m =>
                    {
                        m.ToTable("BaseProfileWidget");
                        m.MapLeftKey("profileID");
                        m.MapRightKey("widgetID");
                    });

            this.HasMany(t => t.BaseUsers)
                .WithMany(t => t.BaseProfiles)
                .Map(m =>
                    {
                        m.ToTable("BaseUserProfile");
                        m.MapLeftKey("profileID");
                        m.MapRightKey("userID");
                    });

            this.HasMany(t => t.BaseGraphs)
                .WithMany(t => t.BaseProfiles)
                .Map(m =>
                    {
                        m.ToTable("BaseUserProfileGraph");
                        m.MapLeftKey("ProfileID");
                        m.MapRightKey("GraphsID");
                    });

            this.HasMany(t => t.BaseWidgets1)
                .WithMany(t => t.BaseProfiles1)
                .Map(m =>
                    {
                        m.ToTable("BaseWidgetUserProfile");
                        m.MapLeftKey("ProfileID");
                        m.MapRightKey("WidgetID");
                    });

            this.HasOptional(t => t.BaseAction)
                .WithMany(t => t.BaseProfiles)
                .HasForeignKey(d => d.defaultActionID);

        }
    }
}
