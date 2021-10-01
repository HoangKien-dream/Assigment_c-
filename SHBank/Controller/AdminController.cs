using System;
using System.Collections.Generic;
using SHBank.Entity;
using SHBank.Model;
using SHBank.Util;

namespace SHBank.Controller
{
    public class AdminController: IAdminController
    {
        private IAdminModel _model;
        private IAccountModel _modelUser;
        public AdminController()
        {
            _modelUser = new AccountModel();
            _model = new AdminModel();
        }

        public void ShowUser()
        {
            List<Account> list = _modelUser.FindByAll();
            foreach (var account in list)
            {
                Console.WriteLine(account.ToString());
            }
            Console.ReadLine();
        }

        public Admin Register()
        {   //1. Đăng kí tài khoản.
            var isValid = false;
            Admin admin = null;
            do
            {
                admin = GetAccountInformation();
                //2. Validate dữ liệu.
                var errors = admin.CheckValid();
                if (CheckExistUserName(admin.UserName))
                {
                    errors.Add("username_duplicate","Duplicate username, please choose another username !");
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
            //Check unique username.
            _model.FindByUserNameAdmin(admin.NameAccount);

            Console.WriteLine(admin.ToString());
            //3 Mã hóa.
            //3.1 Tạo muối.
            admin.EncryptPassword();
            //4 Save vào database.
            var result = _model.Save(admin);
            if (result != null)
            {
                Console.WriteLine("Register Success !!");
                return result;
            }
            return null;
        }

        public Admin Login()
        {
            bool isValid;
            Admin admin = null;
            do
            {
                admin = GetLoginInformation();
                var errors =admin.CheckValidLogin();
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
            
            Admin existingAccount = _model.FindByUserNameAdmin(admin.NameAccount);
            var hash1 =  HashUtil.GenerateSaltedSHA1(admin.Password, existingAccount.Salt);
            if (existingAccount != null && HashUtil.ComparePasswordHash(admin.Password, existingAccount.Salt,existingAccount.PasswordHash))
            {
                Console.WriteLine("Login Success !!");
                return existingAccount;

            }
            else
            {
                Console.WriteLine("Not Found !!!!");   
            }
            return null;
        }

        public Account FindByUserName()
        {
            Console.WriteLine("Enter username account: ");
            string username = Console.ReadLine();
            var account = _model.FindByUserName(username);
            if (account!=null)
            {
                Console.WriteLine("Found Account have user name: "+account.UserName);
                Console.WriteLine(account.ToString());
                Console.ReadLine();
                return account;
            }

            Console.WriteLine("Not Found!!");
            Console.ReadLine();
            return null;
        }

        public Account FindByUserAccountNumber()
        {
            Console.WriteLine("Enter  userAccountNumber : ");
            string accountNumber = Console.ReadLine();
            var account = _model.FindByUserAccountNumber(accountNumber);
            if (account!=null)
            {
                Console.WriteLine("Found Account have userAccountNumber: "+account.AccountNumber);
                Console.WriteLine(account.ToString());
                Console.ReadLine();
                return account;
            }

            Console.WriteLine("Not Found!!");
            Console.ReadLine();
            return null;
        }

        public Account FindByUserPhone()
        {
            Console.WriteLine("Enter  userPhone : ");
            string phone = Console.ReadLine();
            var account = _model.FindByUserPhone(phone);
            if (account!=null)
            {
                Console.WriteLine("Found Account have userPhone: "+account.Phone);
                Console.WriteLine(account.ToString());
                Console.ReadLine();
                return account;
            }

            Console.WriteLine("Not Found!!");
            Console.ReadLine();
            return null;
        }

        public Account CreateNewAccount()
        {
            var account = new Account();
            Console.WriteLine("Enter new nameAccount: ");
            account.NameAccount = Console.ReadLine();
            if (_model.FindByUserNameAccount(account.NameAccount)==null)
            {
                account.GenerateAccountNumber();
                //Lỗi trùng tài khoản không thuộc về người dùng.
                while (CheckExistAccountNumber(account.AccountNumber))
                {
                    // trùng thì generate lại.
                    account.GenerateAccountNumber();
                }
                Console.WriteLine("Enter password: ");
                account.Password = Console.ReadLine();
                Console.WriteLine("Enter confirm password: ");
                account.PasswordConfirm = Console.ReadLine();
                if (account.Password.Equals(account.PasswordConfirm))
                {
                    Console.WriteLine("Enter username: ");
                    account.UserName = Console.ReadLine();
                    Console.WriteLine("Enter phone: ");
                    account.Phone = Console.ReadLine();
                    Console.WriteLine("Enter address: ");
                    account.Address = Console.ReadLine();
                    account.EncryptPassword();
                    _model.CreateNewAccount(account);
                    return account;
                }
                else
                {
                    Console.WriteLine("Try again !!!");
                }
            }
            else
            {
                Console.WriteLine("Try again !!!");
            }

            return null;
        }
        
        private bool CheckExistAccountNumber(string accountAccountNumber)
        {
            return _modelUser.FindByAccountNumber(accountAccountNumber) != null;
        }

        public bool BlockAndOpenAccount()
        {
            Console.WriteLine("Enter username account: ");
            string username = Console.ReadLine();
            var account = _model.FindByUserNameAccount(username);
            if (account!=null)
            {
                Console.WriteLine("Found Account have user name: "+account.UserName);
                Console.WriteLine("Do u want open account or block account(1.Open, -1.Block, 2.Cancel)");
                Console.WriteLine("Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        account.Status = 1;
                        _model.ChangeStatus(account.NameAccount, account);
                        Console.WriteLine("Account is Open");
                        Console.ReadLine();
                        break;
                    case -1:
                        account.Status = -1;
                        _model.ChangeStatus(account.NameAccount, account);
                        Console.WriteLine("Account is Blocked");
                        Console.ReadLine();
                        break;
                    case 2:
                        Console.WriteLine("Cancel Action!!!");
                        Console.ReadLine();
                        break;
                    default:
                        Console.WriteLine("Try again!!!");
                        Console.ReadLine();
                        break;
                }
                return true;
            }

            Console.WriteLine("Not Found!!");
            Console.ReadLine();
            return false;
        }

        public Admin ChangeInform(Admin admin)
        {
            Console.WriteLine("Enter your username: ");
            admin.UserName = Console.ReadLine();
            Console.WriteLine("Enter your phone: ");
            admin.Phone = Console.ReadLine();
            Console.WriteLine("Enter your email: ");
            admin.Email = Console.ReadLine();
            Console.WriteLine("Enter your address: ");
            admin.Address = Console.ReadLine();
            _model.ChangeInform(admin.NameAccount,admin);
            Console.WriteLine("Change Inform success !!!");
            Console.ReadLine();
            return admin;
        }

        public void GetList()
        {
            Console.WriteLine("Enter AccountNumber: ");
            string accountNumber = Console.ReadLine();
            var  result = _modelUser.FindByAccountNumber(accountNumber);
            if (result !=null)
            {
                List<TransactionHistory> list = _model.FindByList(result.AccountNumber);
                foreach (var listTrans in list)
                {
                    Console.WriteLine(listTrans.ToString());
                }
                Console.ReadLine();
            }
        }

        public void FindAll()
        {
            List<TransactionHistory> list = _model.FindByAll();
            foreach (var listTrans in list)
            {
                Console.WriteLine(listTrans.ToString());
            }
            Console.ReadLine();
        }

        public Admin ChangePassword(Admin admin)
        {
            Console.WriteLine("Enter your password: ");
            string oldPassword = Console.ReadLine();
            var hash1 =  HashUtil.GenerateSaltedSHA1(oldPassword, admin.Salt);
            if (hash1.Equals(admin.PasswordHash))
            {
                Console.WriteLine("Enter your new password: ");
                admin.Password = Console.ReadLine();
                Console.WriteLine("Enter your confirm new password: ");
                admin.PasswordConfirm = Console.ReadLine();
                if (admin.Password.Equals(admin.PasswordConfirm))
                {
                    admin.EncryptPassword();
                    _model.ChangePassword(admin.NameAccount,admin);
                    Console.WriteLine("Change Password Success !!!");
                    Console.ReadLine();
                    return admin;
                }
                else
                {
                    Console.WriteLine("Password is not matched");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Try again !!");
                Console.ReadLine();
            }
            return null;
        }

        private Admin GetLoginInformation()
        {
            var admin = new Admin();
            Console.WriteLine("Please enter name account: ");
            admin.NameAccount = Console.ReadLine();
            Console.WriteLine("Please enter password: ");
            admin.Password = Console.ReadLine();
            return admin;
        }

        private bool CheckExistUserName(string userName)
        {
            return _model.FindByUserNameAdmin(userName) != null;
        }

        private Admin GetAccountInformation()
        {
            var admin = new Admin();
            Console.WriteLine("Please enter name account: ");
            admin.NameAccount = Console.ReadLine();
            Console.WriteLine("Please enter password: ");
            admin.Password = Console.ReadLine();
            Console.WriteLine("Please confirm password: ");
            admin.PasswordConfirm = Console.ReadLine();
            Console.WriteLine("Please enter username: ");
            admin.UserName = Console.ReadLine();
            Console.WriteLine("Please enter phone: ");
            admin.Phone = Console.ReadLine();
            Console.WriteLine("Please enter email: ");
            admin.Email = Console.ReadLine();
            Console.WriteLine("Please enter address: ");
            admin.Address = Console.ReadLine();
            return admin;
        }
    }
}