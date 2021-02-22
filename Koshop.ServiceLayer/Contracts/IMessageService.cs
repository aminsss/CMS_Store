using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IMessageService
    {
        DataGridViewModel<Message> GetBySearch(int page, int pageSize, string searchString ,string identity);
        Message GetById(int? id);
        void Add(Message message);
        void Edit(Message message);
        void Delete(Message message);
        void Delete(int? id);
        IEnumerable<Message> GetMessages(string type,string searchString, string identity);
    }
}
