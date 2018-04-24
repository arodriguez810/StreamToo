using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseDynamicColumnListMap : EntityTypeConfiguration<BaseDynamicColumnList>
    {
        public BaseDynamicColumnListMap()
        {
            // Primary Key
            this.HasKey(t => new { t.listID, t.name });

            // Properties
            this.Property(t => t.listID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("BaseDynamicColumnList");
            this.Property(t => t.listID).HasColumnName("listID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.show).HasColumnName("show");
            this.Property(t => t.orderNum).HasColumnName("orderNum");

            // Relationships
            this.HasRequired(t => t.BaseDynamicList)
                .WithMany(t => t.BaseDynamicColumnLists)
                .HasForeignKey(d => d.listID);

        }
    }
}
