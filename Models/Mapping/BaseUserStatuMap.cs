using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseUserStatuMap : EntityTypeConfiguration<BaseUserStatu>
    {
        public BaseUserStatuMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.description)
                .HasMaxLength(100);

            this.Property(t => t.message)
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("BaseUserStatus");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.canLogin).HasColumnName("canLogin");
            this.Property(t => t.defaultAction).HasColumnName("defaultAction");
            this.Property(t => t.message).HasColumnName("message");

            // Relationships
            this.HasOptional(t => t.BaseAction)
                .WithMany(t => t.BaseUserStatus)
                .HasForeignKey(d => d.defaultAction);

        }
    }
}
