using System.Collections.Generic;
using SHBank.Entity;

namespace SHBank.Model
{
    public interface IAccountModel
    {
        List<Account> FindByAll();
        Account Save(Account account);
        Account FindByAccountNumber(string accountNumber);
        Account FindByUserName(string username);
        Account Update(string nameAccount, Account account);
        bool Funds(string nameAccount, double balance);
        Account ChangePass(string nameAccount, Account account);
    }
}