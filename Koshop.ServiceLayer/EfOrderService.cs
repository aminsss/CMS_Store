using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.ServiceLayer.Contracts;
using Koshop.DataLayer;
using Koshop.ViewModels;
using Koshop.DomainClasses;

namespace Koshop.ServiceLayer
{
    
    public class EfOrderService : IOrderService, IDisposable
    {
        private UnitOfWork _unitOfWork;

        public EfOrderService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public DataGridViewModel<Order> GetBySearch(int page, int pageSize, string searchString)
        {
            var dataGridView = new DataGridViewModel<Order>()
            {
                Records = _unitOfWork.OrderRepository.Get(x=>x.OrderId.ToString().Contains(searchString),
                x=>x.OrderBy(o=>o.OrderId),"User").Skip((page-1)*pageSize).Take(pageSize).ToList(),

                TotalCount = _unitOfWork.OrderRepository.Get(x => x.OrderId.ToString().Contains(searchString),
                x => x.OrderBy(o => o.OrderId), "User").Count()
            };

            return dataGridView;
        }

        public Order GetById(int? id)
        {
            return _unitOfWork.OrderRepository.GetById(id);
        }

       
        public void Add(Order order)
        {
            _unitOfWork.OrderRepository.Insert(order);
            _unitOfWork.Save();
        }

        public void Delete(Order order)
        {
            _unitOfWork.OrderRepository.Delete(order);
            _unitOfWork.Save();
        }

        public void Delete(int? id)
        {
            _unitOfWork.OrderRepository.Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public void Edit(Order order)
        {
            _unitOfWork.OrderRepository.Update(order);
            _unitOfWork.Save();
        }

        public IList<OrderDetail> GetOrderDetail(int? id)
        {
            return _unitOfWork.OrderDetailRepository.Get(x => x.OrderId == id,null, "Product,Order").ToList();
        }

        public DataGridViewModel<OrderDetail> GetAllDetail(int page, int pageSize, string searchString)
        {
            var dataGridView = new DataGridViewModel<OrderDetail>()
            {
                Records = _unitOfWork.OrderDetailRepository.Get(x => x.OrderId.ToString().Contains(searchString),
                x => x.OrderBy(o => o.OrderId), "Order,Product").Skip((page - 1) * pageSize).Take(pageSize).ToList(),

                TotalCount = _unitOfWork.OrderRepository.Get(x => x.OrderId.ToString().Contains(searchString),
                x => x.OrderBy(o => o.OrderId), "User").Count()
            };

            return dataGridView;
        }

        public IEnumerable<Order> Orders()
        {
            return _unitOfWork.OrderRepository.Get();
        }
    }
}
