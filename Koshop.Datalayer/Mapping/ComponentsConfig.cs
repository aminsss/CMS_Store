using Koshop.DomainClasses;
using System.Data.Entity.ModelConfiguration;

namespace Koshop.DataLayer.Mapping
{
    public class ComponentConfig : EntityTypeConfiguration<Component>
    {
        public ComponentConfig()
        {
            //Property
            Property(t => t.ActionName).HasMaxLength(30);
            Property(t => t.AdminAction).HasMaxLength(30);
            Property(t => t.AdminController).HasMaxLength(30);
            Property(t => t.ComponentName).HasMaxLength(30);
            Property(t => t.ControllerName).HasMaxLength(30);
            Property(t => t.Descroption).HasMaxLength(500);


        }
    }
}
