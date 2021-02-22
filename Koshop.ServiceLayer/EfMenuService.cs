using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.ViewModels;
using Koshop.ServiceLayer.Contracts;
using Koshop.DataLayer;
using Koshop.DomainClasses;

namespace Koshop.ServiceLayer
{
    public class EfMenuService : IMenuService, IDisposable
    {
        private UnitOfWork _unitOfWork;

        public EfMenuService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public DataGridViewModel<Menu> GetByMenuGroup(int? menuGroupId)
        {
            var dataGridView = new DataGridViewModel<Menu>
            {
                Records = _unitOfWork.MenuRepository.Get(x=>x.MenuGroupId == menuGroupId
                ,x=>x.OrderBy(o=>o.MenuId),"MenuGroup").ToList()
            };

            return dataGridView;
        }

        public Menu GetById(int? id)
        {
            return _unitOfWork.MenuRepository.GetById(id);
        }

       

        public void Add(Menu menu)
        {
            if (menu.ParentId == 0)
            {
                menu.Depth = 0;
                menu.Path = "0";
            }
            else
            {
                var Menus = GetById(menu.ParentId );
                menu.Depth = Menus.Depth + 1;
                menu.Path = Menus.MenuId + "/" + Menus.Path;
            }
            var last = GetLastOrder( menu.ParentId , menu.MenuGroupId);
            if (last == null)
            {
                menu.DisplayOrder = 1;
            }
            else
            {
                menu.DisplayOrder = Convert.ToInt32(last.DisplayOrder) + 1;
            }
            _unitOfWork.MenuRepository.Insert(menu);
            _unitOfWork.Save();
        }

       
        public void Edit(Menu menu, int? pastDisOrder, int? pastParentId, int? pastGroupId)
        {
            //////if new parent is the same as past parent and past group
            if (pastParentId == menu.ParentId && pastGroupId == menu.MenuGroupId)
            {
                //if new Menu order is lower than this menu order
                if (menu.DisplayOrder < pastDisOrder)
                {
                    //Get list of menu between lower of past display odrer and upper of new display order
                    foreach (var item in _unitOfWork.MenuRepository.Get(x => x.ParentId == menu.ParentId && x.MenuGroupId == menu.MenuGroupId &&
                                          x.DisplayOrder >= menu.DisplayOrder && x.DisplayOrder < pastDisOrder, x => x.OrderBy(o => o.DisplayOrder)))
                    {
                        item.DisplayOrder += 1;
                        _unitOfWork.MenuRepository.Update(item);
                    }
                }
                //if new parent order is upper than this menu order
                else if (menu.DisplayOrder > pastDisOrder)
                {
                    //Get list of menu between upper of past display odrer and lower of new display order
                    foreach (var item in _unitOfWork.MenuRepository.Get(x => x.ParentId == menu.ParentId && x.MenuGroupId == menu.MenuGroupId &&
                                         x.DisplayOrder <= menu.DisplayOrder && x.DisplayOrder > pastDisOrder, x => x.OrderBy(o => o.DisplayOrder)))
                    {
                        item.DisplayOrder -= 1;
                        _unitOfWork.MenuRepository.Update(item);
                    }
                }
            }
            //if menu choose another group menu
            else
            {
                //ordering displalay order of past parent 
                foreach (var item in GetByParentGroupOrder(pastParentId, pastGroupId, pastDisOrder))
                {
                    item.DisplayOrder -= 1;
                    _unitOfWork.MenuRepository.Update(item);
                }

                //ordering the last child of new parent 
                var last = GetLastOrder(menu.ParentId, menu.MenuGroupId);
                if (last == null)
                {
                    menu.DisplayOrder = 1;
                }
                else
                {
                    menu.DisplayOrder = Convert.ToInt32(last.DisplayOrder) + 1;
                }
            }
            _unitOfWork.MenuRepository.Update(menu);

            //Update the children of changed Menu of this Menu group
            ChildEdit(menu);

            _unitOfWork.Save();
        }

        public void ChildEdit(Menu menu)
        {
            foreach (Menu child in _unitOfWork.MenuRepository.Get(x => x.ParentId == menu.MenuId))
            {
                child.Path = menu.MenuId + "/" + menu.Path;
                child.Depth = menu.Depth + 1;
                child.MenuGroupId = menu.MenuGroupId;

                _unitOfWork.MenuRepository.Update(child);

                ChildEdit(child);
            }
        }

        public void Delete(Menu menu)
        {
            foreach (var item in _unitOfWork.MenuRepository.Get(x => x.ParentId == menu.ParentId && x.MenuGroupId == menu.MenuGroupId && x.DisplayOrder > menu.DisplayOrder,x=>x.OrderBy(o => o.DisplayOrder)))
            {
                item.DisplayOrder -= 1;
                _unitOfWork.MenuRepository.Update(item);
            }

            ChildRemove(menu);
            _unitOfWork.MenuRepository.Delete(menu);
            _unitOfWork.Save();
        }

        public void ChildRemove(Menu menu)
        {
            foreach (Menu child in _unitOfWork.MenuRepository.Get(x => x.ParentId == menu.MenuId))
            {
                _unitOfWork.MenuRepository.Delete(child);
                ChildRemove(child);
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Menu GetLastOrder(int? parentId, int? menuGroupId)
        {
            return _unitOfWork.MenuRepository.Get(x => x.ParentId == parentId && x.MenuGroupId == menuGroupId,
                x => x.OrderByDescending(o => o.DisplayOrder)).FirstOrDefault();
        }

        public IList<Menu> GetByParentGroupOrder(int? parentId, int? menuGroupId,int? pastDisOrder)
        {
            return _unitOfWork.MenuRepository.Get(x => x.ParentId == parentId && x.MenuGroupId == menuGroupId 
            && x.DisplayOrder > pastDisOrder,x => x.OrderBy(o => o.DisplayOrder)).ToList();
        }

        public bool UniquePageName(string pageName, int? menuId)
        {
            return _unitOfWork.MenuRepository.Get(s => s.PageName == pageName && s.MenuId != menuId).Any();
        }

        public IList<Menu> GetByParentId(int? parentId)
        {
            return _unitOfWork.MenuRepository.Get(x => x.ParentId == parentId).ToList();
        }

        public IEnumerable<Menu> menus()
        {
            return _unitOfWork.MenuRepository.Get();
        }

        public Menu GetByPageName(string pageName)
        {
            return _unitOfWork.MenuRepository.Get(x => x.PageName == pageName,null, "ModulePage").FirstOrDefault();
        }
    }
}
