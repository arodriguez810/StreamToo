using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Admin.Models.Mapping
{
    public class BaseLogMap : EntityTypeConfiguration<BaseLog>
    {
        public BaseLogMap()
        {
            // Primary Key
            this.HasKey(t => t.created);

            // Properties
            this.Property(t => t.entityIdS)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("BaseLog");
            this.Property(t => t.created).HasColumnName("created");
            this.Property(t => t.basedynamicList_entity).HasColumnName("basedynamicList_entity");
            this.Property(t => t.entityId).HasColumnName("entityId");
            this.Property(t => t.entityIdU).HasColumnName("entityIdU");
            this.Property(t => t.entityIdS).HasColumnName("entityIdS");
            this.Property(t => t.date).HasColumnName("date");
            this.Property(t => t.baseUser_user).HasColumnName("baseUser_user");
            this.Property(t => t.baseLogAction_action).HasColumnName("baseLogAction_action");
            this.Property(t => t.version).HasColumnName("version");

            // Relationships
            this.HasRequired(t => t.BaseDynamicList)
                .WithMany(t => t.BaseLogs)
                .HasForeignKey(d => d.basedynamicList_entity);
            this.HasRequired(t => t.BaseLogAction)
                .WithMany(t => t.BaseLogs)
                .HasForeignKey(d => d.baseLogAction_action);
            this.HasRequired(t => t.BaseUser)
                .WithMany(t => t.BaseLogs)
                .HasForeignKey(d => d.baseUser_user);

        }
    }
}
