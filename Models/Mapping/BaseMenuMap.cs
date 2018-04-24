using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseMenuMap : EntityTypeConfiguration<BaseMenu>
    {
        public BaseMenuMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.title)
                .HasMaxLength(50);

            this.Property(t => t.href)
                .HasMaxLength(500);

            this.Property(t => t.css)
                .HasMaxLength(100);

            this.Property(t => t.icon)
                .HasMaxLength(100);

            this.Property(t => t.text)
                .HasMaxLength(100);

            this.Property(t => t.badgeQuery)
                .HasMaxLength(300);

            this.Property(t => t.badgeText)
                .HasMaxLength(100);

            this.Property(t => t.badgeColor)
                .HasMaxLength(100);

            this.Property(t => t.backGroundColor)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("BaseMenu");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.menuID).HasColumnName("menuID");
            this.Property(t => t.title).HasColumnName("title");
            this.Property(t => t.noOrder).HasColumnName("noOrder");
            this.Property(t => t.href).HasColumnName("href");
            this.Property(t => t.actionID).HasColumnName("actionID");
            this.Property(t => t.directLink).HasColumnName("directLink");
            this.Property(t => t.css).HasColumnName("css");
            this.Property(t => t.icon).HasColumnName("icon");
            this.Property(t => t.text).HasColumnName("text");
            this.Property(t => t.badgeQuery).HasColumnName("badgeQuery");
            this.Property(t => t.badgeText).HasColumnName("badgeText");
            this.Property(t => t.badgeColor).HasColumnName("badgeColor");
            this.Property(t => t.backGroundColor).HasColumnName("backGroundColor");
            this.Property(t => t.translatable).HasColumnName("translatable");
            this.Property(t => t.hidden).HasColumnName("hidden");

            // Relationships
            this.HasOptional(t => t.BaseAction)
                .WithMany(t => t.BaseMenus)
                .HasForeignKey(d => d.actionID);
            this.HasOptional(t => t.BaseMenu2)
                .WithMany(t => t.BaseMenu1)
                .HasForeignKey(d => d.menuID);

        }
    }
}
