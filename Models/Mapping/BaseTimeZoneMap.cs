using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseTimeZoneMap : EntityTypeConfiguration<BaseTimeZone>
    {
        public BaseTimeZoneMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.abbreviation)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.fullName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.location)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.timeZone)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("BaseTimeZones");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.abbreviation).HasColumnName("abbreviation");
            this.Property(t => t.fullName).HasColumnName("fullName");
            this.Property(t => t.location).HasColumnName("location");
            this.Property(t => t.timeZone).HasColumnName("timeZone");
        }
    }
}
