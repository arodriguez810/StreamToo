using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWBSStageStatuStageMap : EntityTypeConfiguration<VWBSStageStatuStage>
    {
        public VWBSStageStatuStageMap()
        {
            // Primary Key
            this.HasKey(t => new { t.id, t.name, t.stage });

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.stage)
                .IsRequired()
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("VWBSStageStatuStage");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.stage).HasColumnName("stage");
        }
    }
}
