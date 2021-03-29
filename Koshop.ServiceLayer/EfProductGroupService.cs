using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DataLayer;
using Koshop.DomainClasses;
using Koshop.ServiceLayer.Contracts;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer
{
    public class EfProductGroupService : IProductGroupService,IDisposable
    {
        private UnitOfWork _unitOfWork;

        public EfProductGroupService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DataGridViewModel<ProductGroup> GetBySearch(int? page, int? pageSize, string searchString)
        {
            var dataGridView = new DataGridViewModel<ProductGroup>
            {
                Records = _unitOfWork.ProductGroupRepository.Get(s => s.GroupTitle.Contains(searchString) || s.AliasName.Contains(searchString),
                s => s.OrderBy(x => x.ProductGroupId)).ToList(),
            };

            return dataGridView;
        }

        public void Add(ProductGroup productGroup)
        {
            productGroup.AliasName = productGroup.AliasName.Replace(" ", "");
            _unitOfWork.ProductGroupRepository.Insert(productGroup);
            _unitOfWork.Save();
        }

        public void Delete(ProductGroup productGroup)
        {
            //Remove Child Of selected Group
            ChildRemove(productGroup);
            //remove the selected group
            _unitOfWork.ProductGroupRepository.Delete(productGroup);
            _unitOfWork.Save();
        }

        public void ChildRemove(ProductGroup productGroup)
        {
            foreach (ProductGroup child in  _unitOfWork.ProductGroupRepository.Get(x => x.ParentId == productGroup.ProductGroupId))
            {
                _unitOfWork.ProductGroupRepository.Delete(child);
                ChildRemove(child);
            }
        }

        public void Delete(int? id)
        {
            _unitOfWork.ProductGroupRepository.Delete(id);
            _unitOfWork.Save();
        }

        public void Edit(ProductGroup productGroup)
        {
            //edit the children of selected Group
            ChildEdit(productGroup);
            //edit the selected Group
            productGroup.AliasName = productGroup.AliasName.Replace(" ", "");
            _unitOfWork.ProductGroupRepository.Update(productGroup);
            _unitOfWork.Save();
        }

        public void ChildEdit(ProductGroup productGroup)
        {
            foreach (ProductGroup child in  _unitOfWork.ProductGroupRepository.Get(x => x.ParentId == productGroup.ProductGroupId))
            {
                child.Path = productGroup.ProductGroupId + "/" + productGroup.Path;
                child.Depth = productGroup.Depth + 1;
                _unitOfWork.ProductGroupRepository.Update(child);

                ChildEdit(child);
            }
        }

        public ProductGroup GetById(int? id)
        {
            return _unitOfWork.ProductGroupRepository.GetById(id);
        }

        public IList<ProductGroup> GetByDepth(int? depth)
        {
            return _unitOfWork.ProductGroupRepository.Get(x => x.Depth > depth, o => o.OrderBy(s => s.Depth)).ToList();
        }

        public IEnumerable<ProductGroup> ProductGroups()
        {
            return _unitOfWork.ProductGroupRepository.Get();
        }

        public IList<ProductGroup> GetByType(string type)
        {
            return _unitOfWork.ProductGroupRepository.Get(x => x.type == type).ToList();
        }

        public bool UniqueAlias(string aliasName, int? productGroupId)
        {
            return _unitOfWork.ProductGroupRepository.Get(s => s.AliasName == aliasName && s.ProductGroupId != productGroupId).Any();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public IList<ProductGroup> GetByDesDepthOrder()
        {
            return _unitOfWork.ProductGroupRepository.Get(null, x => x.OrderByDescending(o => o.Depth)).ToList();
        }
    }
}
