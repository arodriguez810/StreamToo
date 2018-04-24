using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWBSAlertTransactionMap : EntityTypeConfiguration<VWBSAlertTransaction>
    {
        public VWBSAlertTransactionMap()
        {
            // Primary Key
            this.HasKey(t => new { t.id, t.comment });

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Creator)
                .HasMaxLength(61);

            this.Property(t => t.Status)
                .HasMaxLength(15);

            this.Property(t => t.comment)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("VWBSAlertTransaction");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.comment).HasColumnName("comment");
            this.Property(t => t.alertID).HasColumnName("alertID");
        }
    }
}
