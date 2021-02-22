using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.ServiceLayer.Contracts;
using Koshop.DomainClasses;
using Koshop.ViewModels;
using Koshop.DataLayer;

namespace Koshop.ServiceLayer
{
    public class EfNewsCommentService : INewsCommentService
    {
        private UnitOfWork _unitOfWork;

        public EfNewsCommentService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool IsUserAllowed(string ip)
        {
            //Not Allow if user sent a comment in last 5 minutes
            DateTime last5Minute = DateTime.Now.AddMinutes(-5);
            var comment = _unitOfWork.NewsCommentRepository.Get(x => x.IP == ip).OrderByDescending(x => x.AddedDate).FirstOrDefault();
            if (comment != null)
            {
                //if comment sent less than 5 minutes ago
                if (comment.AddedDate < last5Minute)
                    return false;
                else return true;
            }
            else return false;
        }


        public void Add(NewsComment newsComment)
        {
            _unitOfWork.NewsCommentRepository.Insert(newsComment);
            _unitOfWork.Save();
        }

        public void ChangeStatus(int id,bool isActive)
        {
            var newsComment = _unitOfWork.NewsCommentRepository.GetById(id);
            newsComment.IsActive = isActive;
            newsComment.ModifiedDate = DateTime.Now;
            _unitOfWork.NewsCommentRepository.Update(newsComment);
            _unitOfWork.Save();
        }

        public void Delete(int id)
        {
            NewsComment newsComment = _unitOfWork.NewsCommentRepository.GetById(id);
            _unitOfWork.NewsCommentRepository.Delete(newsComment);
            _unitOfWork.Save();
        }

        public IEnumerable<NewsComment> GetAll(bool? isActive)
        {
            return _unitOfWork.NewsCommentRepository.Get(x => x.IsActive == isActive,
                x => x.OrderBy(y => y.AddedDate), "News");


        }

       
    }
}
