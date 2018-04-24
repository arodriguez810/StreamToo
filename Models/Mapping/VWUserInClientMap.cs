using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWUserInClientMap : EntityTypeConfiguration<VWUserInClient>
    {
        public VWUserInClientMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.name, t.username, t.password, t.superUser });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.username)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.password)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.activeKey)
                .HasMaxLength(64);

            this.Property(t => t.imageUrl)
                .HasMaxLength(100);

            this.Property(t => t.lastName)
                .HasMaxLength(30);

            this.Property(t => t.lastName2)
                .HasMaxLength(30);

            this.Property(t => t.address)
                .HasMaxLength(300);

            this.Property(t => t.city)
                .HasMaxLength(30);

            this.Property(t => t.province)
                .HasMaxLength(30);

            this.Property(t => t.country)
                .HasMaxLength(50);

            this.Property(t => t.phone)
                .HasMaxLength(20);

            this.Property(t => t.phone2)
                .HasMaxLength(20);

            this.Property(t => t.email)
                .HasMaxLength(100);

            this.Property(t => t.lastIP)
                .HasMaxLength(20);

            this.Property(t => t.identification)
                .HasMaxLength(20);

            this.Property(t => t.cookie)
                .HasMaxLength(50);

            this.Property(t => t.tryAction)
                .HasMaxLength(100);

            this.Property(t => t.tryController)
                .HasMaxLength(100);

            this.Property(t => t.sector)
                .HasMaxLength(50);

            this.Property(t => t.motherFamilyName)
                .HasMaxLength(50);

            this.Property(t => t.favoriteColor)
                .HasMaxLength(50);

            this.Property(t => t.firstTrip)
                .HasMaxLength(50);

            this.Property(t => t.companyContactName)
                .HasMaxLength(50);

            this.Property(t => t.companyContactLastName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VWUserInClient");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.userStatusID).HasColumnName("userStatusID");
            this.Property(t => t.username).HasColumnName("username");
            this.Property(t => t.password).HasColumnName("password");
            this.Property(t => t.activeKey).HasColumnName("activeKey");
            this.Property(t => t.lastVisitAt).HasColumnName("lastVisitAt");
            this.Property(t => t.superUser).HasColumnName("superUser");
            this.Property(t => t.departmentID).HasColumnName("departmentID");
            this.Property(t => t.companyID).HasColumnName("companyID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.updateDate).HasColumnName("updateDate");
            this.Property(t => t.createUser).HasColumnName("createUser");
            this.Property(t => t.positionID).HasColumnName("positionID");
            this.Property(t => t.imageUrl).HasColumnName("imageUrl");
            this.Property(t => t.lastName).HasColumnName("lastName");
            this.Property(t => t.lastName2).HasColumnName("lastName2");
            this.Property(t => t.externalUser).HasColumnName("externalUser");
            this.Property(t => t.confirmationToken).HasColumnName("confirmationToken");
            this.Property(t => t.address).HasColumnName("address");
            this.Property(t => t.city).HasColumnName("city");
            this.Property(t => t.province).HasColumnName("province");
            this.Property(t => t.country).HasColumnName("country");
            this.Property(t => t.phone).HasColumnName("phone");
            this.Property(t => t.phone2).HasColumnName("phone2");
            this.Property(t => t.defaultActionID).HasColumnName("defaultActionID");
            this.Property(t => t.email).HasColumnName("email");
            this.Property(t => t.lastIP).HasColumnName("lastIP");
            this.Property(t => t.TipoDocumentoID).HasColumnName("TipoDocumentoID");
            this.Property(t => t.identification).HasColumnName("identification");
            this.Property(t => t.dateOfBirth).HasColumnName("dateOfBirth");
            this.Property(t => t.cookie).HasColumnName("cookie");
            this.Property(t => t.failedAttempts).HasColumnName("failedAttempts");
            this.Property(t => t.firstTime).HasColumnName("firstTime");
            this.Property(t => t.widgetsToShow).HasColumnName("widgetsToShow");
            this.Property(t => t.tryAction).HasColumnName("tryAction");
            this.Property(t => t.tryController).HasColumnName("tryController");
            this.Property(t => t.countryID).HasColumnName("countryID");
            this.Property(t => t.sector).HasColumnName("sector");
            this.Property(t => t.motherFamilyName).HasColumnName("motherFamilyName");
            this.Property(t => t.favoriteColor).HasColumnName("favoriteColor");
            this.Property(t => t.firstTrip).HasColumnName("firstTrip");
            this.Property(t => t.randomQuestionId).HasColumnName("randomQuestionId");
            this.Property(t => t.superAdminShowHiddenMenu).HasColumnName("superAdminShowHiddenMenu");
            this.Property(t => t.isCompany).HasColumnName("isCompany");
            this.Property(t => t.companyContactName).HasColumnName("companyContactName");
            this.Property(t => t.companyContactLastName).HasColumnName("companyContactLastName");
            this.Property(t => t.imageID).HasColumnName("imageID");
        }
    }
}
