using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseAuditMap : EntityTypeConfiguration<BaseAudit>
    {
        public BaseAuditMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Execute_at, t.Entity, t.Action });

            // Properties
            this.Property(t => t.Entity)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.RecordID)
                .HasMaxLength(30);

            this.Property(t => t.User)
                .HasMaxLength(61);

            this.Property(t => t.Action)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("BaseAudit");
            this.Property(t => t.Execute_at).HasColumnName("Execute at");
            this.Property(t => t.Entity).HasColumnName("Entity");
            this.Property(t => t.RecordID).HasColumnName("RecordID");
            this.Property(t => t.User).HasColumnName("User");
            this.Property(t => t.Action).HasColumnName("Action");
        }
    }
}
