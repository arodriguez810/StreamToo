using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseActionMap : EntityTypeConfiguration<BaseAction>
    {
        public BaseActionMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.displayName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("BaseAction");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.controllerID).HasColumnName("controllerID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.displayName).HasColumnName("displayName");
            this.Property(t => t.isSystem).HasColumnName("isSystem");

            // Relationships
            this.HasMany(t => t.BaseProfiles1)
                .WithMany(t => t.BaseActions)
                .Map(m =>
                    {
                        m.ToTable("BaseProfileAction");
                        m.MapLeftKey("actionID");
                        m.MapRightKey("profileID");
                    });

            this.HasRequired(t => t.BaseController)
                .WithMany(t => t.BaseActions)
                .HasForeignKey(d => d.controllerID);

        }
    }
}
