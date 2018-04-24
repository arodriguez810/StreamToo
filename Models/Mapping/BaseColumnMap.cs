using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseColumnMap : EntityTypeConfiguration<BaseColumn>
    {
        public BaseColumnMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.columnName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.dataType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.defaultValue)
                .HasMaxLength(200);

            this.Property(t => t.referenceTable)
                .HasMaxLength(50);

            this.Property(t => t.referenceField)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("BaseColumn");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.basetable_table).HasColumnName("basetable_table");
            this.Property(t => t.columnName).HasColumnName("columnName");
            this.Property(t => t.dataType).HasColumnName("dataType");
            this.Property(t => t.allowNull).HasColumnName("allowNull");
            this.Property(t => t.defaultValue).HasColumnName("defaultValue");
            this.Property(t => t.ordinalPosition).HasColumnName("ordinalPosition");
            this.Property(t => t.characterMaximumLength).HasColumnName("characterMaximumLength");
            this.Property(t => t.numericPrecision).HasColumnName("numericPrecision");
            this.Property(t => t.numericScale).HasColumnName("numericScale");
            this.Property(t => t.isIndex).HasColumnName("isIndex");
            this.Property(t => t.isUnique).HasColumnName("isUnique");
            this.Property(t => t.active).HasColumnName("active");
            this.Property(t => t.referenceTable).HasColumnName("referenceTable");
            this.Property(t => t.referenceField).HasColumnName("referenceField");

            // Relationships
            this.HasRequired(t => t.BaseTable)
                .WithMany(t => t.BaseColumns)
                .HasForeignKey(d => d.basetable_table);

        }
    }
}
