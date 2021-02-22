using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer.Contracts
{
    public interface INewsCommentService
    {
        //NewsComments
        void Add(NewsComment newsComment);
        bool IsUserAllowed(string ip);

        //admin
        IEnumerable<NewsComment> GetAll(bool? isActive);
        void ChangeStatus(int id, bool isActive);
        void Delete(int id);
    }
}
