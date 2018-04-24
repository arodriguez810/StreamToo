using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWBSAlertRuleConditionMap : EntityTypeConfiguration<VWBSAlertRuleCondition>
    {
        public VWBSAlertRuleConditionMap()
        {
            // Primary Key
            this.HasKey(t => new { t.id, t.entity });

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.entity)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.from)
                .HasMaxLength(50);

            this.Property(t => t.rule)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VWBSAlertRuleCondition");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.entity).HasColumnName("entity");
            this.Property(t => t.from).HasColumnName("from");
            this.Property(t => t.rule).HasColumnName("rule");
        }
    }
}
