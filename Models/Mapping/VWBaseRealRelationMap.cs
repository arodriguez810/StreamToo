using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWBaseRealRelationMap : EntityTypeConfiguration<VWBaseRealRelation>
    {
        public VWBaseRealRelationMap()
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

            this.Property(t => t.realname)
                .HasMaxLength(4000);

            // Table & Column Mappings
            this.ToTable("VWBaseRealRelation");
            this.Property(t => t.K_Table).HasColumnName("K_Table");
            this.Property(t => t.FK_Column).HasColumnName("FK_Column");
            this.Property(t => t.PK_Table).HasColumnName("PK_Table");
            this.Property(t => t.PK_Column).HasColumnName("PK_Column");
            this.Property(t => t.Constraint_Name).HasColumnName("Constraint_Name");
            this.Property(t => t.realname).HasColumnName("realname");
        }
    }
}
