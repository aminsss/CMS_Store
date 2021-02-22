using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IMultiPictureModuleService
    {
        MultiPictureModule GetByModuleId(int? moduleId);
        void Edit(MultiPictureModule htmlModule);

        //MultiModuleItems
        void CreateItems(MultiPictureItems multiPictureItems);

        void EditItems(MultiPictureItems multiPictureItems);

        IList<MultiPictureItems> GetMultiPictureItems(int id);

        MultiPictureItems GetItemsById(int id);

        void DeleteItems(MultiPictureItems multiPictureItems);
    }
}
