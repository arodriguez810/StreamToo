using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseGuestConstraintMap : EntityTypeConfiguration<BaseGuestConstraint>
    {
        public BaseGuestConstraintMap()
        {
            // Primary Key
            this.HasKey(t => t.CONSTRAINT_NAME);

            // Properties
            this.Property(t => t.CONSTRAINT_CATALOG)
                .HasMaxLength(128);

            this.Property(t => t.CONSTRAINT_SCHEMA)
                .HasMaxLength(128);

            this.Property(t => t.CONSTRAINT_NAME)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.CHECK_CLAUSE)
                .HasMaxLength(4000);

            // Table & Column Mappings
            this.ToTable("BaseGuestConstraints");
            this.Property(t => t.CONSTRAINT_CATALOG).HasColumnName("CONSTRAINT_CATALOG");
            this.Property(t => t.CONSTRAINT_SCHEMA).HasColumnName("CONSTRAINT_SCHEMA");
            this.Property(t => t.CONSTRAINT_NAME).HasColumnName("CONSTRAINT_NAME");
            this.Property(t => t.CHECK_CLAUSE).HasColumnName("CHECK_CLAUSE");
        }
    }
}
