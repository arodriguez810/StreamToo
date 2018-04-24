using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class RDEventMap : EntityTypeConfiguration<RDEvent>
    {
        public RDEventMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.description)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("RDEvent");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.active).HasColumnName("active");
            this.Property(t => t.creationDate).HasColumnName("creationDate");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.channel_channel).HasColumnName("channel_channel");
            this.Property(t => t.dateFrom).HasColumnName("dateFrom");
            this.Property(t => t.dateTo).HasColumnName("dateTo");
            this.Property(t => t.timeFrom).HasColumnName("timeFrom");
            this.Property(t => t.timeTo).HasColumnName("timeTo");
            this.Property(t => t.isAllDay).HasColumnName("isAllDay");

            // Relationships
            this.HasOptional(t => t.RDChannel)
                .WithMany(t => t.RDEvents)
                .HasForeignKey(d => d.channel_channel);

        }
    }
}
