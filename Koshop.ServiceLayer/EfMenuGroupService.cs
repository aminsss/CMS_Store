using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DataLayer;
using Koshop.ServiceLayer.Contracts;
using Koshop.ViewModels;
using Koshop.DomainClasses;

namespace Koshop.ServiceLayer
{
    public class EfMenuGroupService : IMenuGroupService, IDisposable
    {
        private UnitOfWork _unitOfWork;

        public EfMenuGroupService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DataGridViewModel<MenuGroup> GetBySearch(int? page, int? pageSize, string searchString)
        {
            var dataGridView = new DataGridViewModel<MenuGroup>
            {
                Records = _unitOfWork.MenuGroupRepository.Get(x => x.MenuTitile.Contains(searchString),
                x => x.OrderBy(o => o.MenuGroupId)).ToList(),
            };

            return dataGridView;
        }

        public void Add(MenuGroup menuGroup)
        {
            _unitOfWork.MenuGroupRepository.Insert(menuGroup);
            _unitOfWork.Save();
        }

        public void Delete(MenuGroup menuGroup)
        {
            _unitOfWork.MenuGroupRepository.Delete(menuGroup);
            _unitOfWork.Save();
        }

        public void Delete(int? id)
        {
            _unitOfWork.MenuGroupRepository.Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Edit(MenuGroup menuGroup)
        {
            _unitOfWork.MenuGroupRepository.Update(menuGroup);
            _unitOfWork.Save();
        }

        public MenuGroup GetById(int? id)
        {
            return _unitOfWork.MenuGroupRepository.GetById(id);
        }

        public IEnumerable<MenuGroup> MenuGroup()
        {
            return _unitOfWork.MenuGroupRepository.Get();
        }
    }
}
