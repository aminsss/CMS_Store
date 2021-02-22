using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IModuleService
    {
        DataGridViewModel<Module> GetBySearch(string searchString);
        void Add(Module module);
        void Edit(Module module,int? pastPosition,int? pastDisOrder);
        void Delete(int id);
        Module GetById(int? id);
        Module GetLastByPosition(int? id);
        IList<Module> GetByPositionId(int? id);


        //position
        IEnumerable<Position> Positions();

    }
}
