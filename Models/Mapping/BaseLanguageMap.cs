using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseLanguageMap : EntityTypeConfiguration<BaseLanguage>
    {
        public BaseLanguageMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.languageCultureName)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.displayName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.cultureCode)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ISO639xValue)
                .HasMaxLength(50);

            this.Property(t => t.flagImage)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("BaseLanguage");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.languageCultureName).HasColumnName("languageCultureName");
            this.Property(t => t.displayName).HasColumnName("displayName");
            this.Property(t => t.cultureCode).HasColumnName("cultureCode");
            this.Property(t => t.ISO639xValue).HasColumnName("ISO639xValue");
            this.Property(t => t.flagImage).HasColumnName("flagImage");
        }
    }
}
