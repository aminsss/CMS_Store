using Koshop.DomainClasses;
using System.Data.Entity.ModelConfiguration;

namespace Koshop.DataLayer.Mapping
{
    public class ContactModuleConfig : EntityTypeConfiguration<ContactModule>
    {
        public ContactModuleConfig()
        {
            this.HasRequired(t => t.Module)
                .WithRequiredPrincipal();
            //.Map(x => x.MapKey("ModuleId"));
        }
    }
}
