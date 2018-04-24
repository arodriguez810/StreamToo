using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class RDClientChannelMap : EntityTypeConfiguration<RDClientChannel>
    {
        public RDClientChannelMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            // Table & Column Mappings
            this.ToTable("RDClientChannel");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.channel_channel).HasColumnName("channel_channel");
            this.Property(t => t.visitCount).HasColumnName("visitCount");
            this.Property(t => t.lastVisit).HasColumnName("lastVisit");
            this.Property(t => t.client_client).HasColumnName("client_client");
            this.Property(t => t.isFavorite).HasColumnName("isFavorite");
            this.Property(t => t.active).HasColumnName("active");
            this.Property(t => t.creationDate).HasColumnName("creationDate");

            // Relationships
            this.HasRequired(t => t.RDChannel)
                .WithMany(t => t.RDClientChannels)
                .HasForeignKey(d => d.channel_channel);
            this.HasRequired(t => t.RDClient)
                .WithMany(t => t.RDClientChannels)
                .HasForeignKey(d => d.client_client);

        }
    }
}
