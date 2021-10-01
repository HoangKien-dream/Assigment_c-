using System;
using System.Collections.Generic;
using System.Transactions;
using SHBank.Entity;

namespace SHBank.Controller
{
    public interface IUserController
    {
        Account Register();
        Account Login();

        Account Update( Account account);
       bool Deposit(Account account);
       bool WithDraw(Account account);
       bool Transfer(Account account);

       Account Information(Account account);
       
       Account ChangePassword(Account account);
    }
}