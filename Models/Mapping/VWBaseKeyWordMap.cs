using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWBaseKeyWordMap : EntityTypeConfiguration<VWBaseKeyWord>
    {
        public VWBaseKeyWordMap()
        {
            // Primary Key
            this.HasKey(t => t.keyword);

            // Properties
            this.Property(t => t.keyword)
                .IsRequired()
                .HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("VWBaseKeyWords");
            this.Property(t => t.keyword).HasColumnName("keyword");
        }
    }
}
