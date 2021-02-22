using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ViewModels;
using Koshop.ServiceLayer.Contracts;
using Koshop.DataLayer;


namespace Koshop.ServiceLayer
{
    public class EfModuleService : IModuleService
    {
        private UnitOfWork _unitOfWork;
        private IMenuService _menuService;

        public EfModuleService(UnitOfWork unitOfWork,IMenuService menuService)
        {
            _unitOfWork = unitOfWork;
            _menuService = menuService;
        }

        public DataGridViewModel<Module> GetBySearch(string searchString)
        {
            var dataGridView = new DataGridViewModel<Module>
            {
                Records = _unitOfWork.ModuleRepository.Get(x => x.ModuleTitle.Contains(searchString)
                , x => x.OrderBy(o => o.DisplayOrder).OrderBy(O=>O.PositionId), "Component,Position").ToList()
            };

            return dataGridView;
        }

        public void Add(Module module)
        {
            module.AddedDate = DateTime.Now;
            module.ModifiedDate = DateTime.Now;

            //method for asign order in create
            var last = GetLastByPosition(module.PositionId);
            if (last == null)
                module.DisplayOrder = 1;
            else
                module.DisplayOrder = (int)last.DisplayOrder + 1;

            _unitOfWork.ModuleRepository.Insert(module);
            _unitOfWork.Save();
        }

        
        public void Edit(Module module, int? pastPosition, int? pastDisOrder)
        {
            module.ModifiedDate = DateTime.Now;

            // ordering => if new order is lower than this module
            if (pastPosition == module.PositionId && module.DisplayOrder < pastDisOrder)
            {
                foreach (var item in _unitOfWork.ModuleRepository.Get(x => x.PositionId == module.PositionId && x.DisplayOrder >= module.DisplayOrder && x.DisplayOrder < pastDisOrder, x => x.OrderBy(o => o.DisplayOrder)))
                {
                    item.DisplayOrder += 1;
                    _unitOfWork.ModuleRepository.Update(item);
                }
            }
            //if new order is higher than this module
            else if (pastPosition == module.PositionId && module.DisplayOrder > pastDisOrder)
            {
                foreach (var item in _unitOfWork.ModuleRepository.Get(x => x.PositionId == module.PositionId && x.DisplayOrder <= module.DisplayOrder && x.DisplayOrder > pastDisOrder, x => x.OrderBy(o => o.DisplayOrder)))
                {
                    item.DisplayOrder -= 1;
                    _unitOfWork.ModuleRepository.Update(item);
                }
            }
            //if menu choose another position
            else if (pastPosition != module.PositionId)
            {
                //ordering displalay order of past position 
                foreach (var item in _unitOfWork.ModuleRepository.Get(x => x.PositionId == pastPosition && x.DisplayOrder > pastDisOrder, x => x.OrderBy(o => o.DisplayOrder)))
                {
                    item.DisplayOrder -= 1;
                    _unitOfWork.ModuleRepository.Update(item);
                }

                //making the last child of new position 
                var last = GetLastByPosition(module.PositionId);
                if (last == null)
                    module.DisplayOrder = 1;
                else
                    module.DisplayOrder = (int)last.DisplayOrder + 1;
            }

            _unitOfWork.ModuleRepository.Update(module);
            _unitOfWork.Save();
        }

        public Module GetById(int? id)
        {
            return _unitOfWork.ModuleRepository.GetById(id);
        }

        public void Delete(int id)
        {
            Module module = GetById(id);
            //editing order of modules with bigger displayOrder in current Position
            foreach (var item in _unitOfWork.ModuleRepository.Get(x => x.PositionId == module.PositionId && x.DisplayOrder > module.DisplayOrder, x => x.OrderBy(o => o.DisplayOrder)))
            {
                item.DisplayOrder -= 1;
                _unitOfWork.ModuleRepository.Update(item);
            }
            _unitOfWork.ModuleRepository.Delete(module);
            _unitOfWork.Save();
        }

        public Module GetLastByPosition(int? positionId)
        {
            return _unitOfWork.ModuleRepository.Get(x => x.PositionId == positionId, 
                x=>x.OrderByDescending(o=>o.DisplayOrder)).FirstOrDefault();
        }



        //Position
        public IEnumerable<Position> Positions()
        {
            return _unitOfWork.PositionRepository.Get();
        }

        public IList<Module> GetByPositionId(int? id)
        {
            return _unitOfWork.ModuleRepository.Get(x => x.PositionId == id).ToList();
        }
    }
}
