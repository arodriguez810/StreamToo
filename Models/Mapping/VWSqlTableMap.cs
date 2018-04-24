using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWSqlTableMap : EntityTypeConfiguration<VWSqlTable>
    {
        public VWSqlTableMap()
        {
            // Primary Key
            this.HasKey(t => t.name);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("VWSqlTables");
            this.Property(t => t.name).HasColumnName("name");
        }
    }
}
