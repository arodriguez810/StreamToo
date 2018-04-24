using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseWebSocketChannelMap : EntityTypeConfiguration<BaseWebSocketChannel>
    {
        public BaseWebSocketChannelMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ParametersName)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("BaseWebSocketChannels");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.IsGlobal).HasColumnName("IsGlobal");
            this.Property(t => t.ServerCode).HasColumnName("ServerCode");
            this.Property(t => t.ParametersName).HasColumnName("ParametersName");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.DataEmitExample).HasColumnName("DataEmitExample");
            this.Property(t => t.ClientCode).HasColumnName("ClientCode");
            this.Property(t => t.NoDateLimit).HasColumnName("NoDateLimit");
            this.Property(t => t.NoTimeLimit).HasColumnName("NoTimeLimit");
        }
    }
}
