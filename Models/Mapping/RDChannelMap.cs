using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class RDChannelMap : EntityTypeConfiguration<RDChannel>
    {
        public RDChannelMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.displayName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.description)
                .HasMaxLength(200);

            this.Property(t => t.url)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("RDChannel");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.active).HasColumnName("active");
            this.Property(t => t.creationDate).HasColumnName("creationDate");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.displayName).HasColumnName("displayName");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.url).HasColumnName("url");
            this.Property(t => t.orderNum).HasColumnName("orderNum");
            this.Property(t => t.isTemporal).HasColumnName("isTemporal");
            this.Property(t => t.iconImage).HasColumnName("iconImage");
            this.Property(t => t.logoImage).HasColumnName("logoImage");
            this.Property(t => t.category_category).HasColumnName("category_category");
            this.Property(t => t.type_type).HasColumnName("type_type");

            // Relationships
            this.HasOptional(t => t.RDCategory)
                .WithMany(t => t.RDChannels)
                .HasForeignKey(d => d.category_category);
            this.HasOptional(t => t.RDType)
                .WithMany(t => t.RDChannels)
                .HasForeignKey(d => d.type_type);

        }
    }
}
