using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IOptionsService
    {
        IList<Options> GetOptions();
        void Edit(Options options);

        Options GetByName(string name);
    }
}
