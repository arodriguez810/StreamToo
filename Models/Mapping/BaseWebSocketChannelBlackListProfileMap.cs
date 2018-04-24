using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseWebSocketChannelBlackListProfileMap : EntityTypeConfiguration<BaseWebSocketChannelBlackListProfile>
    {
        public BaseWebSocketChannelBlackListProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("BaseWebSocketChannelBlackListProfiles");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.profileId).HasColumnName("profileId");
            this.Property(t => t.webSocketChannelId).HasColumnName("webSocketChannelId");

            // Relationships
            this.HasRequired(t => t.BaseProfile)
                .WithMany(t => t.BaseWebSocketChannelBlackListProfiles)
                .HasForeignKey(d => d.profileId);
            this.HasRequired(t => t.BaseWebSocketChannel)
                .WithMany(t => t.BaseWebSocketChannelBlackListProfiles)
                .HasForeignKey(d => d.webSocketChannelId);

        }
    }
}
