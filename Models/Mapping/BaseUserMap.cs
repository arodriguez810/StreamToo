using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseUserMap : EntityTypeConfiguration<BaseUser>
    {
        public BaseUserMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.username)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.password)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.imageUrl)
                .HasMaxLength(100);

            this.Property(t => t.lastName)
                .HasMaxLength(30);

            this.Property(t => t.address)
                .HasMaxLength(300);

            this.Property(t => t.phone)
                .HasMaxLength(20);

            this.Property(t => t.cookie)
                .HasMaxLength(50);

            this.Property(t => t.tryAction)
                .HasMaxLength(100);

            this.Property(t => t.tryController)
                .HasMaxLength(100);

            this.Property(t => t.token)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("BaseUser");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.userStatusID).HasColumnName("userStatusID");
            this.Property(t => t.username).HasColumnName("username");
            this.Property(t => t.password).HasColumnName("password");
            this.Property(t => t.superUser).HasColumnName("superUser");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.createUser).HasColumnName("createUser");
            this.Property(t => t.imageUrl).HasColumnName("imageUrl");
            this.Property(t => t.lastName).HasColumnName("lastName");
            this.Property(t => t.address).HasColumnName("address");
            this.Property(t => t.phone).HasColumnName("phone");
            this.Property(t => t.defaultActionID).HasColumnName("defaultActionID");
            this.Property(t => t.cookie).HasColumnName("cookie");
            this.Property(t => t.widgetsToShow).HasColumnName("widgetsToShow");
            this.Property(t => t.tryAction).HasColumnName("tryAction");
            this.Property(t => t.tryController).HasColumnName("tryController");
            this.Property(t => t.superAdminShowHiddenMenu).HasColumnName("superAdminShowHiddenMenu");
            this.Property(t => t.token).HasColumnName("token");
            this.Property(t => t.employeeType_Type).HasColumnName("employeeType_Type");
            this.Property(t => t.office_office).HasColumnName("office_office");
            this.Property(t => t.isAppUser).HasColumnName("isAppUser");

            // Relationships
            this.HasOptional(t => t.BaseAction)
                .WithMany(t => t.BaseUsers)
                .HasForeignKey(d => d.defaultActionID);
            this.HasOptional(t => t.BaseUserStatu)
                .WithMany(t => t.BaseUsers)
                .HasForeignKey(d => d.userStatusID);

        }
    }
}
