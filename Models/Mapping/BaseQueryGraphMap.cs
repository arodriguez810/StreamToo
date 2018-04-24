using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseQueryGraphMap : EntityTypeConfiguration<BaseQueryGraph>
    {
        public BaseQueryGraphMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.query)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("BaseQueryGraph");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.query).HasColumnName("query");
        }
    }
}
