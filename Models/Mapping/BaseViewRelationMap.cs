using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseViewRelationMap : EntityTypeConfiguration<BaseViewRelation>
    {
        public BaseViewRelationMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.tableFrom)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.fkColumn)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.pkColumn)
                .HasMaxLength(50);

            this.Property(t => t.pkColumnName)
                .HasMaxLength(50);

            this.Property(t => t.tableTo)
                .HasMaxLength(50);

            this.Property(t => t.customLabel)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("BaseViewRelation");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.tableFrom).HasColumnName("tableFrom");
            this.Property(t => t.fkColumn).HasColumnName("fkColumn");
            this.Property(t => t.pkColumn).HasColumnName("pkColumn");
            this.Property(t => t.pkColumnName).HasColumnName("pkColumnName");
            this.Property(t => t.tableTo).HasColumnName("tableTo");
            this.Property(t => t.customLabel).HasColumnName("customLabel");
            this.Property(t => t.hide).HasColumnName("hide");
        }
    }
}
