using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseUserSessionMap : EntityTypeConfiguration<BaseUserSession>
    {
        public BaseUserSessionMap()
        {
            // Primary Key
            this.HasKey(t => new { t.userID, t.created });

            // Properties
            this.Property(t => t.userID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ip)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("BaseUserSession");
            this.Property(t => t.userID).HasColumnName("userID");
            this.Property(t => t.created).HasColumnName("created");
            this.Property(t => t.ip).HasColumnName("ip");

            // Relationships
            this.HasRequired(t => t.BaseUser)
                .WithMany(t => t.BaseUserSessions)
                .HasForeignKey(d => d.userID);

        }
    }
}
