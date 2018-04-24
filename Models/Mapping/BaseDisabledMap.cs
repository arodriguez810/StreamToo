using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseDisabledMap : EntityTypeConfiguration<BaseDisabled>
    {
        public BaseDisabledMap()
        {
            // Primary Key
            this.HasKey(t => new { t.type, t.id, t.userID, t.date });

            // Properties
            this.Property(t => t.type)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.userID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("BaseDisabled");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.userID).HasColumnName("userID");
            this.Property(t => t.date).HasColumnName("date");
        }
    }
}
