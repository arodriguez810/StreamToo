using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWBaseallsystemMap : EntityTypeConfiguration<VWBaseallsystem>
    {
        public VWBaseallsystemMap()
        {
            // Primary Key
            this.HasKey(t => new { t.object_id, t.name, t.create_date });

            // Properties
            this.Property(t => t.object_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(129);

            // Table & Column Mappings
            this.ToTable("VWBaseallsystem");
            this.Property(t => t.object_id).HasColumnName("object_id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.create_date).HasColumnName("create_date");
        }
    }
}
