using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.ViewModels;
using Koshop.DataLayer;
using Koshop.ServiceLayer.Contracts;
using Koshop.DomainClasses;

namespace Koshop.ServiceLayer
{
    public class EfMultiPictureModuleService : IMultiPictureModuleService
    {
        private UnitOfWork _unitOfWork;

        public EfMultiPictureModuleService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Edit(MultiPictureModule multiPictureModule)
        {
            _unitOfWork.MultiPictureModuleRepository.Update(multiPictureModule);
            _unitOfWork.Save();
        }

        public MultiPictureModule GetByModuleId(int? moduleId)
        {
            return _unitOfWork.MultiPictureModuleRepository.Get(x => x.ModuleId == moduleId).FirstOrDefault();
        }


        //MultiictureModuleItems
        public void CreateItems(MultiPictureItems multiPictureItems)
        {
            multiPictureItems.Image = "no-photo.jpg";
            _unitOfWork.MultiPictureItemsRepository.Insert(multiPictureItems);
            _unitOfWork.Save();
        }

        public void EditItems(MultiPictureItems multiPictureItems)
        {
            _unitOfWork.MultiPictureItemsRepository.Update(multiPictureItems);
            _unitOfWork.Save();
        }

        public IList<MultiPictureItems> GetMultiPictureItems(int id)
        {
            return _unitOfWork.MultiPictureItemsRepository.Get(x => x.ModuleId == id).ToList();
        }

        public MultiPictureItems GetItemsById(int id)
        {
            return _unitOfWork.MultiPictureItemsRepository.Get(x => x.MultiPictureItemsId == id).FirstOrDefault();
        }

        public void DeleteItems(MultiPictureItems multiPictureItems)
        {
            _unitOfWork.MultiPictureItemsRepository.Delete(multiPictureItems);
            _unitOfWork.Save();
        }
    }
}
