using System;
using System.Collections.Generic;
using System.Globalization;
using System.Transactions;
using SHBank.Entity;
using SHBank.Model;

namespace SHBank.Controller
{
    public class TransactionHistoryController: ITransactionHistoryController

    {
        public void GetList(string name)
        {
            ITransactionHistoryModel model = new TransactionHistoryModel();
            List<TransactionHistory> list = model.FindByNameAccount(name);
            foreach (var listTran in list)
            {
                Console.WriteLine(listTran.ToString());
            }
        }
    }
}