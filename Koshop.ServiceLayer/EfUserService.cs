using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.ServiceLayer.Contracts;
using Koshop.DataLayer;
using Koshop.DomainClasses;
using Koshop.ViewModels;
using System.Linq.Expressions;

namespace Koshop.ServiceLayer
{
    public class EfUserService : IUserService,IDisposable
    {
        private UnitOfWork _unitOfWork;

        public EfUserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DataGridViewModel<User> GetBySearch(int page, int pageSize, string srchString)
        {
            var DataGridView = new DataGridViewModel<User> {
                Records = _unitOfWork.UserRepository.Get(s => s.Name.Contains(srchString) || s.UserName.Contains(srchString),
                s => s.OrderBy(o => o.moblie), "Role")
                .Skip((page -1) * pageSize).Take(pageSize).ToList(),

                TotalCount = _unitOfWork.UserRepository.Get(s => s.Name.Contains(srchString) || s.UserName.Contains(srchString),
                s => s.OrderBy(o => o.moblie), "Role").Count()
            };

            return DataGridView;
        }

        public User GetById(int? id)
        {
            return _unitOfWork.UserRepository.GetById(id);
        }

        public User GetUserByIdentity(string userName)
        {
            return _unitOfWork.UserRepository.Get(u => u.UserName == userName).FirstOrDefault();
        }

        public User GetUserByPassword(int userId, string password)
        {
            return _unitOfWork.UserRepository.Get(u => u.UserId == userId && u.Password == password).FirstOrDefault();
        }

        public void Add(User user)
        {
            user.AddedDate = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            _unitOfWork.UserRepository.Insert(user);
            _unitOfWork.Save();
        }

        public void Edit(User user)
        {
            user.ModifiedDate = DateTime.Now;
            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();
        }

        public void EditPassword(User user, string password)
        {
            user.Password = password;
            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();
        }

        public void Delete(User user)
        {
            _unitOfWork.UserRepository.Delete(user);
            _unitOfWork.Save();
        }

        public void Delete(int id)
        {
            _unitOfWork.UserRepository.Delete(id);
            _unitOfWork.Save();
        }

       
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public IEnumerable<User> Users()
        {
            return _unitOfWork.UserRepository.Get();
        }

        public IEnumerable<User> GetAllAdmin()
        {
            return _unitOfWork.UserRepository.Get(x => x.RoleId == 1,null,"Role");
        }

        public bool UniqueUserName(string userName, int userId)
        {
            return _unitOfWork.UserRepository.Get(s => s.UserName == userName && s.UserId != userId).Any();
        }

        public bool UniqueEmail(string email, int userId)
        {
            return _unitOfWork.UserRepository.Get(s => s.Email == email && s.UserId != userId).Any();
        }

        public bool UniqueMobile(string mobile, int userId)
        {
            return _unitOfWork.UserRepository.Get(s => s.moblie == mobile && s.UserId != userId).Any();
        }

        public User LogIn(string userName, string password)
        {
            return _unitOfWork.UserRepository.Get(x => (x.UserName == userName || x.moblie == userName || x.Email == userName) && x.Password == password).FirstOrDefault();
        }

        
        public bool UniqueUserMobile(string mobile, string userName)
        {
            return _unitOfWork.UserRepository.Get(s => s.moblie == mobile && s.UserName != userName).Any();
        }

        public bool UniqueUserEmail(string email, string userName)
        {
            return _unitOfWork.UserRepository.Get(s => s.Email == email && s.UserName != userName).Any();
        }

        public bool UniqueUserName(string userName)
        {
            return _unitOfWork.UserRepository.Get(s => s.UserName == userName ).Any();
        }

        public User GetByUserName(string userName)
        {
            return _unitOfWork.UserRepository.Get(x => x.UserName == userName || x.moblie == userName || x.Email == userName).FirstOrDefault();
        }

        public User GetByActiveCode(string activeCode)
        {
            return _unitOfWork.UserRepository.Get(x => x.ActiveCode == activeCode).FirstOrDefault();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _unitOfWork.UserRepository.Get().ToList();
        }
    }
}
