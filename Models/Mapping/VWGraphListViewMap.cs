using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWGraphListViewMap : EntityTypeConfiguration<VWGraphListView>
    {
        public VWGraphListViewMap()
        {
            // Primary Key
            this.HasKey(t => new { t.id, t.name, t.title, t.query, t.queryGraphID });

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.title)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.query)
                .IsRequired();

            this.Property(t => t.queryGraphID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("VWGraphListView");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.title).HasColumnName("title");
            this.Property(t => t.query).HasColumnName("query");
            this.Property(t => t.queryGraphID).HasColumnName("queryGraphID");
        }
    }
}
