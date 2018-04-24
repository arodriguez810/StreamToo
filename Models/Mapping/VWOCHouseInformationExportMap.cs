using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class VWOCHouseInformationExportMap : EntityTypeConfiguration<VWOCHouseInformationExport>
    {
        public VWOCHouseInformationExportMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.House_ID, t.Sticker_Number, t.Creation_Date, t.User, t.Comment });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.House_ID)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Sticker_Number)
                .IsRequired();

            this.Property(t => t.Location)
                .HasMaxLength(67);

            this.Property(t => t.User)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Comment)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("VWOCHouseInformationExport");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.House_ID).HasColumnName("House ID");
            this.Property(t => t.Sticker_Number).HasColumnName("Sticker Number");
            this.Property(t => t.Account_Numbers).HasColumnName("Account Numbers");
            this.Property(t => t.Barcode).HasColumnName("Barcode");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.Creation_Date).HasColumnName("Creation Date");
            this.Property(t => t.User).HasColumnName("User");
            this.Property(t => t.Comment).HasColumnName("Comment");
        }
    }
}
