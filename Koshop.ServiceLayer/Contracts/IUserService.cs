using Koshop.DomainClasses;
using System;
using System.Collections.Generic;
using Koshop.ViewModels;
using System.Threading.Tasks;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IUserService
    {
        DataGridViewModel<User> GetBySearch(int page, int pageSize,string srchString);
        User GetUserByIdentity(string userName);
        User GetUserByPassword(int userId, string password);
        User GetById(int? id);
        void Add(User user);
        void Edit(User user);
        void EditPassword(User user,string password);
        void Delete(User user);
        void Delete(int id);
        bool UniqueUserName(string userName, int userId);
        bool UniqueEmail(string email, int userId);
        bool UniqueMobile(string mobile, int userId);
        IEnumerable<User> Users();
        IEnumerable<User> GetAllAdmin();
        IEnumerable<User> GetAllUsers();

        //user panel
        User LogIn(string userName, string password);
        bool UniqueUserMobile(string mobile, string userName);
        bool UniqueUserEmail(string email, string userName);
        bool UniqueUserName(string userName);
        User GetByUserName(string userName);
        User GetByActiveCode(string activeCode);
    }
}
