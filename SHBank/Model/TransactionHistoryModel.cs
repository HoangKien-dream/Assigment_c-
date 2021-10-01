using System.Collections.Generic;
using System.Transactions;
using MySql.Data.MySqlClient;
using MySqlHandle.Util;
using SHBank.Entity;

namespace SHBank.Model
{
    public class TransactionHistoryModel: ITransactionHistoryModel
    { 
        private string _insertCommand = $"INSERT INTO transaction_history (userTransfer, accountNumberTransfer, userReceive,  accountNumberReceive, moneyTransfer, content) VALUES (@userTransfer, @accountNumberTransfer, @userReceive,  @accountNumberReceive, @moneyTransfer, @content)";
        private string _selectCommandByUsername = $"SELECT * FROM transaction_history WHERE userTransfer = @userTransfer OR userReceive = @userReceive" ;
        public TransactionHistory Save(TransactionHistory transactionHistory)
        {
            using (var cnn = ConnectionHelper.GetInstance())
            {
                
                cnn.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(_insertCommand, cnn);
                mySqlCommand.Parameters.AddWithValue("@userTransfer", transactionHistory.UserTransfer);
                mySqlCommand.Parameters.AddWithValue("@accountNumberTransfer", transactionHistory.AccountNumberTransfer);
                mySqlCommand.Parameters.AddWithValue("@userReceive", transactionHistory.UserReceive);
                mySqlCommand.Parameters.AddWithValue("@accountNumberReceive", transactionHistory.AccountNumberReceive);
                mySqlCommand.Parameters.AddWithValue("@moneyTransfer", transactionHistory.MoneyTransfer);
                mySqlCommand.Parameters.AddWithValue("@content", transactionHistory.Content);
                mySqlCommand.Prepare();
                var result = mySqlCommand.ExecuteNonQuery();
                if (result > 0)
                {
                    return transactionHistory;
                }
                return null;
            }
        }

        public List<TransactionHistory> FindByNameAccount(string name)
        {
            using (var cnn = ConnectionHelper.GetInstance())
            {
                List<TransactionHistory> list = new List<TransactionHistory>();
                cnn.Open();
                MySqlCommand command = new MySqlCommand(_selectCommandByUsername, cnn);
                command.Parameters.AddWithValue("@userTransfer", name);
                command.Parameters.AddWithValue("@userReceive", name);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var transaction = new TransactionHistory()
                    {
                        UserTransfer = reader.GetString("userTransfer"),
                        AccountNumberTransfer = reader.GetString("accountNumberTransfer"),
                        UserReceive = reader.GetString("userReceive"),
                        AccountNumberReceive = reader.GetString("accountNumberReceive"),
                        Content = reader.GetString("content"),
                        RollNumber = reader.GetInt32("STT")
                    };
                    list.Add(transaction);
                }
                return list;
            }
        }
    }
}
