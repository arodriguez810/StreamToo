using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseUserMenuMap : EntityTypeConfiguration<BaseUserMenu>
    {
        public BaseUserMenuMap()
        {
            // Primary Key
            this.HasKey(t => new { t.menuID, t.userID });

            // Properties
            this.Property(t => t.menuID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.userID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("BaseUserMenu");
            this.Property(t => t.menuID).HasColumnName("menuID");
            this.Property(t => t.userID).HasColumnName("userID");
            this.Property(t => t.noOrder).HasColumnName("noOrder");

            // Relationships
            this.HasRequired(t => t.BaseMenu)
                .WithMany(t => t.BaseUserMenus)
                .HasForeignKey(d => d.menuID);
            this.HasRequired(t => t.BaseUser)
                .WithMany(t => t.BaseUserMenus)
                .HasForeignKey(d => d.userID);

        }
    }
}
