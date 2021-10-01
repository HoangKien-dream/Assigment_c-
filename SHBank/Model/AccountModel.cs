using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MySqlHandle.Util;
using SHBank.Entity;

namespace SHBank.Model
{
    public class AccountModel: IAccountModel
    {
        private string _insertCommand = $"INSERT INTO accounts (account_number,nameAccount ,username, password_hash, salt, address, phone) VALUES (@account_number, @nameAccount, @username, @password_hash, @salt, @address, @phone)";
        private string _selectCommandByAccountNumber = $"SELECT * FROM accounts WHERE account_number = @account_number";
        private string _selectCommandByUsername = $"SELECT * FROM accounts WHERE nameAccount = @nameAccount";
        private string _updateCommandByUsername = $"UPDATE accounts SET address = @address, phone = @phone WHERE nameAccount = @nameAccount";
        private string _depositCommandByAccountNumber = $"UPDATE accounts SET balance = @balance WHERE nameAccount = @nameAccount";
        private string _updatePasswordByNameAccount = $"UPDATE accounts SET password_hash = @password_hash, salt = @salt WHERE nameAccount = @nameAccount";
        private string _selectUser = $"SELECT * FROM accounts";




        public List<Account> FindByAll()
        {
            List<Account> list = new List<Account>();
            MySqlConnection cnn = ConnectionHelper.GetInstance();
            cnn.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(_selectUser,cnn);
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
                list.Add(account);
            }
            return list;
        }

        public Account Save(Account account)
        {
            using (var cnn = ConnectionHelper.GetInstance())
            {
                
                cnn.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(_insertCommand, cnn);
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

        public Account FindByAccountNumber(string accountNumber)
        {
            MySqlConnection cnn = ConnectionHelper.GetInstance();
            cnn.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(_selectCommandByAccountNumber,cnn);
            mySqlCommand.Parameters.AddWithValue("@account_number", accountNumber);
            mySqlCommand.Prepare();
            var reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var account = new Account()
                {
                      AccountNumber = reader.GetString("account_number"),
                      NameAccount = reader.GetString("nameAccount"),
                      UserName = reader.GetString("username"),
                      PasswordHash = reader.GetString("password_hash"),
                      Salt = reader.GetString("salt")
                };
                return account;
            }

            return null;
        }

        public Account FindByUserName(string username)
        {
            MySqlConnection cnn = ConnectionHelper.GetInstance();
            cnn.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(_selectCommandByUsername,cnn);
            mySqlCommand.Parameters.AddWithValue("@nameAccount", username);
            mySqlCommand.Prepare();
            var reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var account = new Account()
                {
                    AccountNumber = reader.GetString("account_number"),
                    NameAccount = reader.GetString("nameAccount"),
                    UserName = reader.GetString("username"),
                    PasswordHash = reader.GetString("password_hash"),
                    Salt = reader.GetString("salt"),
                    Balance = reader.GetDouble("balance"),
                    Phone = reader.GetString("phone")
                };
                return account;
            }

            return null;
        }

 
        
        public Account Update(string nameAccount, Account account)
        {
            using ( MySqlConnection cnn = ConnectionHelper.GetInstance())
            {
                cnn.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(_updateCommandByUsername,cnn);
                mySqlCommand.Parameters.AddWithValue("@address", account.Address);
                mySqlCommand.Parameters.AddWithValue("@phone", account.Phone);
                mySqlCommand.Parameters.AddWithValue("@nameAccount", nameAccount);
                mySqlCommand.Prepare(); 
                mySqlCommand.ExecuteReader();
                return account;   
            }
        }

        public bool Funds(string nameAccount, double updateBalance)
        {
            MySqlConnection cnn = ConnectionHelper.GetInstance();
            cnn.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(_depositCommandByAccountNumber,cnn);
            mySqlCommand.Parameters.AddWithValue("@balance", updateBalance);
            mySqlCommand.Parameters.AddWithValue("@nameAccount", nameAccount);
            mySqlCommand.Prepare(); 
            mySqlCommand.ExecuteReader();
            return true;
        }

        public Account ChangePass(string nameAccount, Account account)
        {
            MySqlConnection cnn = ConnectionHelper.GetInstance();
            cnn.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(_updatePasswordByNameAccount, cnn);
            mySqlCommand.Parameters.AddWithValue("@password_hash", account.PasswordHash);
            mySqlCommand.Parameters.AddWithValue("@salt", account.Salt);
            mySqlCommand.Parameters.AddWithValue("@nameAccount", account.NameAccount);
            mySqlCommand.Prepare();
            mySqlCommand.ExecuteReader();
            return account;
        }
    }
}