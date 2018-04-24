using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseWebSocketChannelBlackListUserMap : EntityTypeConfiguration<BaseWebSocketChannelBlackListUser>
    {
        public BaseWebSocketChannelBlackListUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("BaseWebSocketChannelBlackListUsers");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.userID).HasColumnName("userID");
            this.Property(t => t.webSocketChannelid).HasColumnName("webSocketChannelid");

            // Relationships
            this.HasRequired(t => t.BaseUser)
                .WithMany(t => t.BaseWebSocketChannelBlackListUsers)
                .HasForeignKey(d => d.userID);
            this.HasRequired(t => t.BaseWebSocketChannel)
                .WithMany(t => t.BaseWebSocketChannelBlackListUsers)
                .HasForeignKey(d => d.webSocketChannelid);

        }
    }
}
