using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseAccountMap : EntityTypeConfiguration<BaseAccount>
    {
        public BaseAccountMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.username)
                .HasMaxLength(15);

            this.Property(t => t.phone)
                .HasMaxLength(20);

            this.Property(t => t.email)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.password)
                .IsRequired();

            this.Property(t => t.fullName)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("BaseAccount");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.username).HasColumnName("username");
            this.Property(t => t.phone).HasColumnName("phone");
            this.Property(t => t.email).HasColumnName("email");
            this.Property(t => t.registered).HasColumnName("registered");
            this.Property(t => t.creationDate).HasColumnName("creationDate");
            this.Property(t => t.password).HasColumnName("password");
            this.Property(t => t.token).HasColumnName("token");
            this.Property(t => t.fullName).HasColumnName("fullName");
        }
    }
}
