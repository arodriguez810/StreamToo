using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseUserActionMap : EntityTypeConfiguration<BaseUserAction>
    {
        public BaseUserActionMap()
        {
            // Primary Key
            this.HasKey(t => new { t.actionID, t.userID });

            // Properties
            this.Property(t => t.actionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.userID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.password)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("BaseUserAction");
            this.Property(t => t.actionID).HasColumnName("actionID");
            this.Property(t => t.userID).HasColumnName("userID");
            this.Property(t => t.forever).HasColumnName("forever");
            this.Property(t => t.untilDate).HasColumnName("untilDate");
            this.Property(t => t.password).HasColumnName("password");
            this.Property(t => t.passwordAccess).HasColumnName("passwordAccess");
            this.Property(t => t.leftSeconds).HasColumnName("leftSeconds");

            // Relationships
            this.HasRequired(t => t.BaseAction)
                .WithMany(t => t.BaseUserActions)
                .HasForeignKey(d => d.actionID);
            this.HasRequired(t => t.BaseUser)
                .WithMany(t => t.BaseUserActions)
                .HasForeignKey(d => d.userID);

        }
    }
}
