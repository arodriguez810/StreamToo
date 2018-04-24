using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseDiagnosticBaseMap : EntityTypeConfiguration<BaseDiagnosticBase>
    {
        public BaseDiagnosticBaseMap()
        {
            // Primary Key
            this.HasKey(t => new { t.endWithS, t.isKeyWord, t.hasUnderScore, t.correctNameRelation, t.noHasPrimaryKey, t.hasActiveField });

            // Properties
            this.Property(t => t.TableOrColumn)
                .HasMaxLength(4000);

            this.Property(t => t.Input)
                .HasMaxLength(4000);

            this.Property(t => t.endWithS)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.isKeyWord)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.hasUnderScore)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.correctNameRelation)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.noHasPrimaryKey)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.hasActiveField)
                .IsRequired()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("BaseDiagnosticBase");
            this.Property(t => t.TableOrColumn).HasColumnName("TableOrColumn");
            this.Property(t => t.Input).HasColumnName("Input");
            this.Property(t => t.endWithS).HasColumnName("endWithS");
            this.Property(t => t.isKeyWord).HasColumnName("isKeyWord");
            this.Property(t => t.hasUnderScore).HasColumnName("hasUnderScore");
            this.Property(t => t.correctNameRelation).HasColumnName("correctNameRelation");
            this.Property(t => t.noHasPrimaryKey).HasColumnName("noHasPrimaryKey");
            this.Property(t => t.hasActiveField).HasColumnName("hasActiveField");
        }
    }
}
