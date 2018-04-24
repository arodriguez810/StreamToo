using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWBaseActionControllerMap : EntityTypeConfiguration<VWBaseActionController>
    {
        public VWBaseActionControllerMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.name)
                .HasMaxLength(50);

            this.Property(t => t.controller)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VWBaseActionController");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.controller).HasColumnName("controller");
        }
    }
}
