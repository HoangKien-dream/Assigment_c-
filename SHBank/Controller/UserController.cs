using System;
using System.Collections.Generic;
using System.Transactions;
using SHBank.Entity;
using SHBank.Model;
using SHBank.Util;

namespace SHBank.Controller
{
    public class UserController: IUserController
    {
        private IAccountModel _model;
        private ITransactionHistoryModel _transactionHistory;

        public UserController()
        {
            _model = new AccountModel();
            _transactionHistory = new TransactionHistoryModel();
        }
        public Account Register()
        {   //1. Đăng kí tài khoản.
            var isValid = false;
            Account account = null;
            do
            {
                account = GetAccountInformation();
                //2. Validate dữ liệu.
                var errors = account.CheckValid();
                if (CheckExistUserName(account.NameAccount))
                {
                    errors.Add("username_duplicate","Duplicate NameAccount, please choose another NameAccount !");
                }
                isValid = errors.Count == 0;
                if (!isValid)
                {
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error.Value);
                    }

                    Console.WriteLine("Please  reenter account information !");
                }
            } while (!isValid);
            account.GenerateAccountNumber();
            //Lỗi trùng tài khoản không thuộc về người dùng.
            while (CheckExistAccountNumber(account.AccountNumber))
            {
                // trùng thì generate lại.
                account.GenerateAccountNumber();
            }

            var existingAccount = _model.FindByAccountNumber(account.AccountNumber);
            //Check unique username.
            _model.FindByUserName(account.NameAccount);
            //3 Mã hóa.
            //3.1 Tạo muối.
            account.EncryptPassword();
            //4 Save vào database.
            var result = _model.Save(account);
            if (result != null)
            {
                Console.WriteLine("Register Success !!");
                return result;
            }
            return null;
        }

        public Account Login()
        {
            bool isValid;
            Account account = null;
            do
            {
                account = GetLoginInformation();
                var errors =account.CheckValidLogin();
                isValid = errors.Count == 0;
                if (!isValid)
                {
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error.Value);
                    }

                    Console.WriteLine("Please  reenter login information !");
                }
            } while (!isValid);
            
            Account existingAccount = _model.FindByUserName(account.NameAccount);
            var hash1 =  HashUtil.GenerateSaltedSHA1(account.Password, existingAccount.Salt);
            if (existingAccount != null && HashUtil.ComparePasswordHash(account.Password, existingAccount.Salt,existingAccount.PasswordHash))
            {
                Console.WriteLine("Login Success !!");
                Console.ReadLine();
                return existingAccount;

            }
            else
            {
                Console.WriteLine("Not Found !!!!");   
            }
            return null;
        }

        public Account Update(Account account)
        {
            // Console.WriteLine("Enter your account: ");
            // string nameAccount = Console.ReadLine();
            // if (_model.FindByUserName(nameAccount) != null)
            // {
                Console.WriteLine("Please enter your address: ");
                account.Address = Console.ReadLine();
                Console.WriteLine("Please enter your phone: ");
                account.Phone = Console.ReadLine();
                _model.Update(account.NameAccount, account);
                Console.WriteLine("Update success!!");
                Console.ReadLine();
                return account;
            // }

            Console.WriteLine("Not Found !!!");
            return null;
        }

        public bool Deposit(Account account)
        {
                Console.WriteLine("------Your AccountNumber: "+account.AccountNumber);
                Console.WriteLine("Enter Deposit Funds: "); 
                double money = Convert.ToDouble(Console.ReadLine());
                // set lại trường balance trong account
                if (money > 0 && account.Balance > money)
                {
                    account.Balance += money;
                    _model.Funds(account.NameAccount, account.Balance);
                    Console.WriteLine("Enter your content: ");
                    string content = Console.ReadLine();
                    Console.WriteLine("Deposit success!!!");
                    Console.ReadLine();
                    // lưu lịch sử giao dịch
                    var transactionHistory = new TransactionHistory
                    {
                        UserTransfer = "Spring Hero Bank",
                        AccountNumberTransfer = "0",
                        UserReceive = account.NameAccount,
                        AccountNumberReceive = account.AccountNumber,
                        MoneyTransfer = money,
                        Content = content
                    };
                    var transaction= new TransactionHistoryModel();
                    transaction.Save(transactionHistory);
                    return true;
                }

                Console.WriteLine("Balance not enough to deposit");
                Console.ReadLine();
                return false;
        }

        public bool WithDraw(Account account)
        {
            Console.WriteLine("Enter your money withdraw: ");
            double money = Convert.ToDouble(Console.ReadLine());
            if (money>0 && account.Balance > money)
            {
                account.Balance -= money;
                Console.WriteLine("Enter your content: ");
                string content = Console.ReadLine();
                _model.Funds(account.NameAccount, account.Balance);
                var transactionHistory = new TransactionHistory
                {
                    UserTransfer = "ATM Spring Hero Bank",
                    AccountNumberTransfer = "0",
                    UserReceive = account.NameAccount,
                    AccountNumberReceive = account.AccountNumber,
                    MoneyTransfer = money,
                    Content = content
                };
                _transactionHistory.Save(transactionHistory);
                Console.WriteLine("Withdraw success !!");
                Console.ReadLine();
                return true;
            }

            Console.WriteLine("Balance not enough to withdraw");
            Console.ReadLine();
            return false;
        }

        public bool Transfer(Account userTransfer)
        {
            Console.WriteLine("Enter your nameAccount you want transfer: ");
            string name = Console.ReadLine();
            Account userReceive = _model.FindByUserName(name);
            if ( userReceive!= null)
            {
                Console.WriteLine("Enter your money transfer: ");
                double money = Convert.ToDouble(Console.ReadLine());
                if (money>0 && userTransfer.Balance>money)
                {
                    double balance1 = userTransfer.Balance - money;
                    _model.Funds(userTransfer.NameAccount, balance1);
                    double balance2 = userReceive.Balance + money;
                    _model.Funds(userReceive.NameAccount, balance2);
                    Console.WriteLine("Enter your content: ");
                    string content = Console.ReadLine();
                    Console.WriteLine("Withdraw success !!");
                    Console.ReadLine();
                    var transactionHistory = new TransactionHistory
                    {
                        UserTransfer = userTransfer.NameAccount,
                        AccountNumberTransfer =userTransfer.AccountNumber,
                        UserReceive = userReceive.NameAccount,
                        AccountNumberReceive = userReceive.AccountNumber,
                        MoneyTransfer = money,
                        Content = content
                    };
                    _transactionHistory.Save(transactionHistory);
                    return true;
                }
                else
                {
                    Console.WriteLine("Balance not enough to transfer");
                    Console.ReadLine();
                }
            }
            Console.WriteLine("Try again.");
            Console.ReadLine();
            return false;
        }

        public Account Information(Account account)
        {
            Console.WriteLine("---- Information Account ----");
            Console.WriteLine("Number Account: "+account.AccountNumber);
            Console.WriteLine("User name: "+account.NameAccount);
            Console.WriteLine("Phone: "+account.Phone);
            Console.WriteLine("Balance: "+account.Balance);
            Console.ReadLine();
            return account;
        }
        

        public Account ChangePassword(Account account)
        {
            var oldPassword = _model.FindByUserName(account.NameAccount);
            Console.WriteLine("Enter your password: ");
            string password = Console.ReadLine();
            var hashPassword =  HashUtil.GenerateSaltedSHA1(password, account.Salt);
            if (hashPassword.Equals(account.PasswordHash))
            {
                Console.WriteLine("Enter new password: ");
                account.Password = Console.ReadLine();
                Console.WriteLine("Enter new confirm password: ");
                account.PasswordConfirm = Console.ReadLine();
                if (account.Password.Equals(account.PasswordConfirm))
                {
                    account.EncryptPassword();
                    var result =_model.ChangePass(account.NameAccount, account);
                    return result;
                }
                else
                {
                    Console.WriteLine("Password not matched !!!");
                }
            }
            else
            {
                Console.WriteLine("Password not matched !!!");
            }
            return null;
        }


        private Account GetLoginInformation()
        {
            var account = new Account();
            Console.WriteLine("Please enter nameAccount: ");
            account.NameAccount = Console.ReadLine();
            Console.WriteLine("Please enter password: ");
            account.Password = Console.ReadLine();
            return account;
        }

        private bool CheckExistUserName(string account)
        {
            return _model.FindByUserName(account) != null;
        }

        private bool CheckExistAccountNumber(string accountAccountNumber)
        {
            return _model.FindByAccountNumber(accountAccountNumber) != null;
        }

        private Account GetAccountInformation()
        {
            var account = new Account();
            Console.WriteLine("Please enter name account: ");
            account.NameAccount = Console.ReadLine();
            Console.WriteLine("Please enter username: ");
            account.UserName = Console.ReadLine();
            Console.WriteLine("Please enter password: ");
            account.Password = Console.ReadLine();
            Console.WriteLine("Please confirm password: ");
            account.PasswordConfirm = Console.ReadLine();
            Console.WriteLine("Please enter your address: ");
            account.Address = Console.ReadLine();
            Console.WriteLine("Please enter your phone: ");
            account.Phone = Console.ReadLine();
            return account;
        }
    }
}