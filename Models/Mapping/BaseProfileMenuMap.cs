using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseProfileMenuMap : EntityTypeConfiguration<BaseProfileMenu>
    {
        public BaseProfileMenuMap()
        {
            // Primary Key
            this.HasKey(t => new { t.menuID, t.userProfileID });

            // Properties
            this.Property(t => t.menuID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.userProfileID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("BaseProfileMenu");
            this.Property(t => t.menuID).HasColumnName("menuID");
            this.Property(t => t.userProfileID).HasColumnName("userProfileID");
            this.Property(t => t.noOrder).HasColumnName("noOrder");

            // Relationships
            this.HasRequired(t => t.BaseMenu)
                .WithMany(t => t.BaseProfileMenus)
                .HasForeignKey(d => d.menuID);
            this.HasRequired(t => t.BaseProfile)
                .WithMany(t => t.BaseProfileMenus)
                .HasForeignKey(d => d.userProfileID);

        }
    }
}
