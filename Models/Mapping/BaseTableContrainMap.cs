using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseTableContrainMap : EntityTypeConfiguration<BaseTableContrain>
    {
        public BaseTableContrainMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.description)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.query)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("BaseTableContrain");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.basetable_table).HasColumnName("basetable_table");
            this.Property(t => t.query).HasColumnName("query");
            this.Property(t => t.active).HasColumnName("active");

            // Relationships
            this.HasRequired(t => t.BaseTable)
                .WithMany(t => t.BaseTableContrains)
                .HasForeignKey(d => d.basetable_table);

        }
    }
}
