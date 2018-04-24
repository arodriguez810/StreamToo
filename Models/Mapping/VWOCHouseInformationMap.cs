using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWOCHouseInformationMap : EntityTypeConfiguration<VWOCHouseInformation>
    {
        public VWOCHouseInformationMap()
        {
            // Primary Key
            this.HasKey(t => new { t.id, t.houseId, t.stickerNumber, t.baseUser_User, t.comment, t.creationDate });

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.houseId)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.stickerNumber)
                .IsRequired();

            this.Property(t => t.location)
                .HasMaxLength(67);

            this.Property(t => t.baseUser_User)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.comment)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("VWOCHouseInformation");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.houseId).HasColumnName("houseId");
            this.Property(t => t.accountNumbers).HasColumnName("accountNumbers");
            this.Property(t => t.stickerNumber).HasColumnName("stickerNumber");
            this.Property(t => t.location).HasColumnName("location");
            this.Property(t => t.baseUser_User).HasColumnName("baseUser_User");
            this.Property(t => t.comment).HasColumnName("comment");
            this.Property(t => t.barCode).HasColumnName("barCode");
            this.Property(t => t.creationDate).HasColumnName("creationDate");
        }
    }
}
