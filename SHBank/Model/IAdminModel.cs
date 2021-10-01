using System.Collections.Generic;
using SHBank.Entity;

namespace SHBank.Model
{
    public interface IAdminModel
    {
        
        Admin Save(Admin admin);
        Admin FindByUserNameAdmin(string username);
        Account FindByUserName(string username);
        Account FindByUserAccountNumber(string accountNumber);
        Account FindByUserPhone(string phone);
        Account FindByUserNameAccount(string nameAccount);
        Account CreateNewAccount(Account account);
        Account ChangeStatus(string nameAccount, Account account);
        Admin ChangeInform(string nameAccount, Admin admin);
        Admin ChangePassword(string nameAccount, Admin admin);

        List<TransactionHistory> FindByList(string accountNumber);
        List<TransactionHistory> FindByAll();
    }
}