using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.DataLayer;
using Koshop.ViewModels;
using Koshop.ServiceLayer.Contracts;

namespace Koshop.ServiceLayer
{
    public class EfMessageService : IMessageService, IDisposable
    {
        private UnitOfWork _unitOfWork;

        public EfMessageService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DataGridViewModel<Message> GetBySearch(int page, int pageSize, string searchString,string identity)
        {
            var dataGridView = new DataGridViewModel<Message>
            {
                Records = _unitOfWork.MessageRepository.Get(x => (x.UsersFrom.UserName == identity)
                && (x.UsersFrom.UserName.Contains(searchString) || x.Subject.Contains(searchString)),
                x => x.OrderBy(o => o.MessageId), "UsersFrom").Skip((page - 1) * pageSize).Take(pageSize).ToList(),

                TotalCount = _unitOfWork.MessageRepository.Get(x => (x.UsersFrom.UserName == identity)
                && (x.UsersFrom.UserName.Contains(searchString) || x.Subject.Contains(searchString)),
                x => x.OrderBy(o => o.MessageId), "UsersFrom").Count(),

            };

            return dataGridView;
        }
        public void Add(Message message)
        {
            message.AddedDate = DateTime.Now;
            message.ModifiedDate = DateTime.Now;
            _unitOfWork.MessageRepository.Insert(message);
            _unitOfWork.Save();
        }

        public void Delete(Message message)
        {
            _unitOfWork.MessageRepository.Delete(message);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public void Edit(Message message)
        {
            message.ISRead = true;
            message.ModifiedDate = DateTime.Now;
            _unitOfWork.MessageRepository.Update(message);
            _unitOfWork.Save();
        }

        public Message GetById(int? id)
        {
            return _unitOfWork.MessageRepository.GetById(id);
        }

        public void Delete(int? id)
        {
            _unitOfWork.MessageRepository.Delete(id);
            _unitOfWork.Save();
        }

        public IEnumerable<Message> GetMessages(string type, string searchString, string identity)
        {
            return _unitOfWork.MessageRepository.Get(x =>x.Type == type && (x.UsersTo.UserName == identity || x.UsersFrom.UserName == identity)
                && (x.UsersTo.Name.Contains(searchString) || x.UsersFrom.Name.Contains(searchString) || x.Subject.Contains(searchString)));
        }
    }
}
