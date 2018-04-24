using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseMessageMap : EntityTypeConfiguration<BaseMessage>
    {
        public BaseMessageMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.code)
                .IsRequired()
                .HasMaxLength(300);

            this.Property(t => t.message)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("BaseMessage");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.code).HasColumnName("code");
            this.Property(t => t.message).HasColumnName("message");
            this.Property(t => t.messageCategory).HasColumnName("messageCategory");

            // Relationships
            this.HasRequired(t => t.BaseMessageCategory)
                .WithMany(t => t.BaseMessages)
                .HasForeignKey(d => d.messageCategory);

        }
    }
}
