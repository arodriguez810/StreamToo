using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseControllerMap : EntityTypeConfiguration<BaseController>
    {
        public BaseControllerMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.fullName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.infoName)
                .HasMaxLength(50);

            this.Property(t => t.Discriminator)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("BaseController");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.fullName).HasColumnName("fullName");
            this.Property(t => t.infoName).HasColumnName("infoName");
            this.Property(t => t.Discriminator).HasColumnName("Discriminator");
        }
    }
}
