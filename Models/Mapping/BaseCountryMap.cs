using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseCountryMap : EntityTypeConfiguration<BaseCountry>
    {
        public BaseCountryMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.officialShortForm)
                .IsRequired()
                .HasMaxLength(70);

            this.Property(t => t.officialLongForm)
                .HasMaxLength(150);

            this.Property(t => t.ISOCode)
                .HasMaxLength(20);

            this.Property(t => t.ISOShort)
                .HasMaxLength(20);

            this.Property(t => t.ISOLong)
                .HasMaxLength(20);

            this.Property(t => t.UNCode)
                .HasMaxLength(20);

            this.Property(t => t.CapitalMajorCity)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("BaseCountry");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.officialShortForm).HasColumnName("officialShortForm");
            this.Property(t => t.officialLongForm).HasColumnName("officialLongForm");
            this.Property(t => t.ISOCode).HasColumnName("ISOCode");
            this.Property(t => t.ISOShort).HasColumnName("ISOShort");
            this.Property(t => t.ISOLong).HasColumnName("ISOLong");
            this.Property(t => t.UNCode).HasColumnName("UNCode");
            this.Property(t => t.CapitalMajorCity).HasColumnName("CapitalMajorCity");
        }
    }
}
