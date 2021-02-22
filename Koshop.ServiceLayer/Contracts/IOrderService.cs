using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IOrderService
    {
        DataGridViewModel<Order> GetBySearch(int page, int pageSize, string searchString);
        Order GetById(int? id);
        void Add(Order order);
        void Edit(Order order);
        void Delete(Order order);
        void Delete(int? id);
        IEnumerable<Order> Orders();

        //orderDetail
        IList<OrderDetail> GetOrderDetail(int? id);
        DataGridViewModel<OrderDetail> GetAllDetail(int page, int pageSize, string searchString);
    }
}
