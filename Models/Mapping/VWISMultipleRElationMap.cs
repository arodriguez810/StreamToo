using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWISMultipleRElationMap : EntityTypeConfiguration<VWISMultipleRElation>
    {
        public VWISMultipleRElationMap()
        {
            // Primary Key
            this.HasKey(t => t.Constraint_Name);

            // Properties
            this.Property(t => t.K_Table)
                .HasMaxLength(128);

            this.Property(t => t.FK_Column)
                .HasMaxLength(128);

            this.Property(t => t.PK_Table)
                .HasMaxLength(128);

            this.Property(t => t.PK_Column)
                .HasMaxLength(128);

            this.Property(t => t.Constraint_Name)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("VWISMultipleRElations");
            this.Property(t => t.K_Table).HasColumnName("K_Table");
            this.Property(t => t.FK_Column).HasColumnName("FK_Column");
            this.Property(t => t.PK_Table).HasColumnName("PK_Table");
            this.Property(t => t.PK_Column).HasColumnName("PK_Column");
            this.Property(t => t.Constraint_Name).HasColumnName("Constraint_Name");
        }
    }
}
