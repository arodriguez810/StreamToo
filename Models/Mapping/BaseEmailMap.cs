using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseEmailMap : EntityTypeConfiguration<BaseEmail>
    {
        public BaseEmailMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.code)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.subject)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.bodyHTML)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("BaseEmail");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.code).HasColumnName("code");
            this.Property(t => t.subject).HasColumnName("subject");
            this.Property(t => t.bodyHTML).HasColumnName("bodyHTML");
            this.Property(t => t.isHtml).HasColumnName("isHtml");
        }
    }
}
