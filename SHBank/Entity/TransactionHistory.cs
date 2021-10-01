using System;

namespace SHBank.Entity
{
    public class TransactionHistory
    {
        public string UserTransfer { get; set; }
        public string AccountNumberTransfer { get; set; }
        public string UserReceive { get; set; }
        public string AccountNumberReceive { get; set; }
        public double MoneyTransfer { get; set; }
        public string Content { get; set; }

        public int RollNumber { get; set; }
        public override string ToString()
        {
            return String.Format("USerTransfer: {0} - AccountTransfer: {1} - UserReceive: {2} - AccountReceive: {3} - MoneyTrans: {4} - Content: {5}",UserTransfer,AccountNumberTransfer,UserReceive,AccountNumberReceive,MoneyTransfer,Content);
        }
    }
}