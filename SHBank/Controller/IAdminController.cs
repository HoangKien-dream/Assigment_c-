using System.Collections.Generic;
using SHBank.Entity;

namespace SHBank.Controller
{
    public interface IAdminController
    {
        void ShowUser();
        Admin Register();
        Admin Login();
        Account FindByUserName();
        Account FindByUserAccountNumber();
        Account FindByUserPhone();
        Account CreateNewAccount();
        bool BlockAndOpenAccount();
        Admin ChangeInform(Admin admin);
        void GetList();
        void FindAll();
        Admin ChangePassword(Admin admin);
    }
}