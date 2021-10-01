using System.Collections.Generic;
using SHBank.Entity;

namespace SHBank.Controller
{
    public interface ITransactionHistoryController
    {
       void GetList(string name);
    }
}