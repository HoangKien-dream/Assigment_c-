using System;
using System.Runtime.InteropServices;
using SHBank.Controller;
using SHBank.Entity;

namespace SHBank.View
{
    public class UserView
    {
        public void GenerateUser()
        {
            ITransactionHistoryController transactionHistoryController = new TransactionHistoryController();
            IUserController userController = new UserController();
            var account = userController.Login();
            if (account != null)
            { 
                while (true)
                {
                    Console.WriteLine("—— Ngan Hang Spring Hero Bank ——");
                    Console.WriteLine("Welcome Back "+ account.UserName);
                    Console.WriteLine("1. Deposit Funds. ");
                    Console.WriteLine("2. Withdraw Cash. ");
                    Console.WriteLine("3. Tranfer Funds. ");
                    Console.WriteLine("4. Check Balance. ");
                    Console.WriteLine("5. Change Information. ");
                    Console.WriteLine("6. Change Password. ");
                    Console.WriteLine("7. Check Transaction History. ");
                    Console.WriteLine("8. Exit Program. ");
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Please enter your choice (1-8): ");
                    int choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            userController.Deposit(account);
                            break;
                        case 2:
                            userController.WithDraw(account);
                            break;
                        case 3:
                            userController.Transfer(account);
                            break;
                        case 4:
                            userController.Information(account);
                            break;
                        case 5:
                            userController.Update(account);
                            break;
                        case 6:
                            userController.ChangePassword(account);
                            break;
                        case 7:
                            transactionHistoryController.GetList(account.NameAccount);
                            break;
                        case 8:
                            Console.WriteLine("Exit Program!!");
                            break;
                        default:
                            Console.WriteLine("Please choice again!!!!");
                            break;
                    }

                    if (choice == 8)
                    {
                        break;
                    }  
                }
            }
        }
    }
}