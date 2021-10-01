using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MySqlHandle.Util;
using SHBank.Entity;

namespace SHBank.Model
{
    public class AdminModel: IAdminModel
    {
        private string _insertCommand = $"INSERT INTO admins (nameAccount, password_hash, salt, userName, phone, email, address) VALUES (@nameAccount, @password_hash, @salt, @userName, @phone, @email, @address)";
        private string _selectCommandByUsername = $"SELECT * FROM admins WHERE nameAccount = @nameAccount";
        private string _selectFindByUsername = $"SELECT * FROM accounts WHERE username = @username";
        private string _selectFindByUserPhone = $"SELECT * FROM accounts WHERE phone = @phone";
        private string _selectFindByUserNameAccount = $"SELECT * FROM accounts WHERE nameAccount = @nameAccount";
        private string _selectFindByUserAccountNumber= $"SELECT * FROM accounts WHERE account_number = @account_number";
        private string _insertUserAccount= $"INSERT INTO accounts (account_number,nameAccount ,username, password_hash, salt, address, phone) VALUES (@account_number, @nameAccount, @username, @password_hash, @salt, @address, @phone)";
        private string _updateUserStatus = $"UPDATE accounts SET status = @status WHERE nameAccount = @nameAccount";
        private string _updateAdminPassword = $"UPDATE admins SET password_hash = @password_hash, salt = @salt WHERE nameAccount = @nameAccount";
        private string _updateAdminInform= $"UPDATE admins SET username = @username, phone = @phone, email = @email, address = @address WHERE nameAccount = @nameAccount";
        private string _selectAllTransfer= $"SELECT * FROM transaction_history";

        private string _selectTransfer =
            $"SELECT * FROM transaction_history WHERE accountNumberTransfer= @accountNumberTransfer OR accountNumberReceive = @accountNumberReceive";
        

        public Admin Save(Admin admin)
        {
            using (var cnn = ConnectionHelper.GetInstance())
            {
                
                cnn.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(_insertCommand, cnn);
                mySqlCommand.Parameters.AddWithValue("@nameAccount", admin.NameAccount);
                mySqlCommand.Parameters.AddWithValue("@password_hash", admin.PasswordHash);
                mySqlCommand.Parameters.AddWithValue("@salt", admin.Salt);
                mySqlCommand.Parameters.AddWithValue("@username", admin.UserName);
                mySqlCommand.Parameters.AddWithValue("@phone", admin.Phone);
                mySqlCommand.Parameters.AddWithValue("@email", admin.Email);
                mySqlCommand.Parameters.AddWithValue("@address", admin.Address);
                mySqlCommand.Prepare();
                var result = mySqlCommand.ExecuteNonQuery();
                if (result > 0)
                {
                    return admin;
                }
                return null;
            }
        }

        public Account FindByUserName(string username)
        {
            MySqlConnection cnn = ConnectionHelper.GetInstance();
            cnn.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(_selectFindByUsername,cnn);
            mySqlCommand.Parameters.AddWithValue("@username", username);
            mySqlCommand.Prepare();
            var reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var account = new Account()
                {
                    NameAccount = reader.GetString("nameAccount"),
                    AccountNumber = reader.GetString("account_number"),
                    UserName = reader.GetString("username"),
                    Address = reader.GetString("address"),
                    Phone = reader.GetString("phone"),
                    Balance = reader.GetDouble("balance"),
                    Status = reader.GetInt32("status")
                };
                return account;
            }
            return null;
        }

        public Account FindByUserAccountNumber(string accountNumber)
        {
            MySqlConnection cnn = ConnectionHelper.GetInstance();
            cnn.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(_selectFindByUserAccountNumber,cnn);
            mySqlCommand.Parameters.AddWithValue("@account_number", accountNumber);
            mySqlCommand.Prepare();
            var reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var account = new Account()
                {
                    NameAccount = reader.GetString("nameAccount"),
                    AccountNumber = reader.GetString("account_number"),
                    UserName = reader.GetString("username"),
                    Address = reader.GetString("address"),
                    Phone = reader.GetString("phone"),
                    Balance = reader.GetDouble("balance"),
                    Status = reader.GetInt32("status")
                };
                return account;
            }
            return null;
        }

        public Account FindByUserPhone(string phone)
        {
            MySqlConnection cnn = ConnectionHelper.GetInstance();
            cnn.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(_selectFindByUserPhone,cnn);
            mySqlCommand.Parameters.AddWithValue("@phone", phone);
            mySqlCommand.Prepare();
            var reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var account = new Account()
                {
                    NameAccount = reader.GetString("nameAccount"),
                    AccountNumber = reader.GetString("account_number"),
                    UserName = reader.GetString("username"),
                    Address = reader.GetString("address"),
                    Phone = reader.GetString("phone"),
                    Balance = reader.GetDouble("balance"),
                    Status = reader.GetInt32("status")
                };
                return account;
            }
            return null;
        }
        
        public Account FindByUserNameAccount(string nameAccount)
        {
            MySqlConnection cnn = ConnectionHelper.GetInstance();
            cnn.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(_selectFindByUserNameAccount,cnn);
            mySqlCommand.Parameters.AddWithValue("@nameAccount", nameAccount);
            mySqlCommand.Prepare();
            var reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var account = new Account()
                {
                    NameAccount = reader.GetString("nameAccount"),
                    AccountNumber = reader.GetString("account_number"),
                    UserName = reader.GetString("username"),
                    Address = reader.GetString("address"),
                    Phone = reader.GetString("phone"),
                    Balance = reader.GetDouble("balance"),
                    Status = reader.GetInt32("status")
                };
                return account;
            }
            return null;
        }

        public Account CreateNewAccount(Account account)
        {
            using (var cnn = ConnectionHelper.GetInstance())
            {
                
                cnn.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(_insertUserAccount, cnn);
                mySqlCommand.Parameters.AddWithValue("@account_number", account.AccountNumber);
                mySqlCommand.Parameters.AddWithValue("@nameAccount", account.NameAccount);
                mySqlCommand.Parameters.AddWithValue("@username", account.UserName);
                mySqlCommand.Parameters.AddWithValue("@password_hash", account.PasswordHash);
                mySqlCommand.Parameters.AddWithValue("@salt", account.Salt);
                mySqlCommand.Parameters.AddWithValue("@address", account.Address);
                mySqlCommand.Parameters.AddWithValue("@phone", account.Phone);
                mySqlCommand.Prepare();
                var result = mySqlCommand.ExecuteNonQuery();
                if (result > 0)
                {
                    return account;
                }
                return null;
            }
        }

        public Account ChangeStatus(string nameAccount, Account account)
        {
            using ( MySqlConnection cnn = ConnectionHelper.GetInstance())
            {
                cnn.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(_updateUserStatus,cnn);
                mySqlCommand.Parameters.AddWithValue("@status", account.Status);
                mySqlCommand.Parameters.AddWithValue("@nameAccount", nameAccount);
                mySqlCommand.Prepare(); 
                mySqlCommand.ExecuteReader();
                return account;   
            }
        }

        public Admin ChangeInform(string nameAccount, Admin admin)
        {
            using (MySqlConnection cnn = ConnectionHelper.GetInstance())
            {
                cnn.Open();
                MySqlCommand command = new MySqlCommand(_updateAdminInform,cnn);
                command.Parameters.AddWithValue("@userName", admin.UserName);
                command.Parameters.AddWithValue("@phone", admin.Phone);
                command.Parameters.AddWithValue("@email", admin.Email);
                command.Parameters.AddWithValue("@address", admin.Address);
                command.Parameters.AddWithValue("@nameAccount", nameAccount);
                command.Prepare();
                command.ExecuteReader();
                return admin;
            }
        }

        public Admin ChangePassword(string nameAccount, Admin admin)
        {
            using (MySqlConnection cnn = ConnectionHelper.GetInstance())
            {
                cnn.Open();
                MySqlCommand command = new MySqlCommand(_updateAdminPassword,cnn);
                command.Parameters.AddWithValue("@password_hash", admin.PasswordHash);
                command.Parameters.AddWithValue("@salt", admin.Salt);
                command.Parameters.AddWithValue("@nameAccount", nameAccount);
                command.Prepare();
                command.ExecuteReader();
                return admin;
            }
        }

        public List<TransactionHistory> FindByList(string name)
        {
            using (MySqlConnection cnn = ConnectionHelper.GetInstance())
            {
                List<TransactionHistory> list = new List<TransactionHistory>();
                cnn.Open();
                MySqlCommand command = new MySqlCommand(_selectTransfer,cnn);
                command.Parameters.AddWithValue("@accountNumberTransfer", name);
                command.Parameters.AddWithValue("@accountNumberReceive", name);
                command.Prepare();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var trans = new TransactionHistory()
                    {
                        UserTransfer = reader.GetString("userTransfer"),
                        AccountNumberTransfer = reader.GetString("accountNumberTransfer"),
                        UserReceive = reader.GetString("userReceive"),
                        AccountNumberReceive = reader.GetString("accountNumberReceive"),
                        MoneyTransfer = reader.GetDouble("moneyTransfer"),
                        Content = reader.GetString("content"),
                        RollNumber = reader.GetInt32("STT")
                    };
                    list.Add(trans);
                }
                return list;
            }
        }

        public List<TransactionHistory> FindByAll()
        {
            using (MySqlConnection cnn = ConnectionHelper.GetInstance())
            {
                List<TransactionHistory> list = new List<TransactionHistory>();
                cnn.Open();
                MySqlCommand command = new MySqlCommand(_selectAllTransfer,cnn);
                command.Prepare();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var trans = new TransactionHistory()
                    {
                        UserTransfer = reader.GetString("userTransfer"),
                        AccountNumberTransfer = reader.GetString("accountNumberTransfer"),
                        UserReceive = reader.GetString("userReceive"),
                        AccountNumberReceive = reader.GetString("accountNumberReceive"),
                        MoneyTransfer = reader.GetDouble("moneyTransfer"),
                        Content = reader.GetString("content"),
                        RollNumber = reader.GetInt32("STT")
                    };
                    list.Add(trans);
                }
                return list;
            }
        }


        public Admin FindByUserNameAdmin(string username)
        {
            MySqlConnection cnn = ConnectionHelper.GetInstance();
            cnn.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(_selectCommandByUsername,cnn);
            mySqlCommand.Parameters.AddWithValue("@nameAccount", username);
            mySqlCommand.Prepare();
            var reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var admin = new Admin()
                {
                    NameAccount = reader.GetString("nameAccount"),
                    UserName = reader.GetString("username"),
                    PasswordHash = reader.GetString("password_hash"),
                    Salt = reader.GetString("salt"),
                    Address = reader.GetString("address"),
                    Phone = reader.GetString("phone"),
                    Email = reader.GetString("email")
                };
                return admin;
            }
            return null;
        }
        
    }
}