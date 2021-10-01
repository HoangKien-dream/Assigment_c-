using System;
using SHBank.Controller;

namespace SHBank.View
{
    public class AdminView
    {
         public void GenerateAdmin()
         {
             IAdminController adminController = new AdminController();
             var admin = adminController.Login();
             Console.ReadLine();
             if (admin!=null)
             {
              while (true)
              {
                  Console.WriteLine("—— Ngan Hang Spring Hero BAnk ——");
                  Console.WriteLine("Welcome Back Admin " +admin.UserName );
                  Console.WriteLine("1. List User. ");
                  Console.WriteLine("2. List Transaction History ");
                  Console.WriteLine("3. Searching User By Name. ");
                  Console.WriteLine("4. Searching User By Account Number. ");
                  Console.WriteLine("5. Searching User By Phone. ");
                  Console.WriteLine("6. Add New User. ");
                  Console.WriteLine("7. Block And Open User Account. ");
                  Console.WriteLine("8. Searching TransactionHistory By Account Number. ");
                  Console.WriteLine("9. Change Information User. ");
                  Console.WriteLine("10. Change Password User. ");
                  Console.WriteLine("11. Exit Program. ");
                  Console.WriteLine("--------------------------");
                  Console.WriteLine("Please enter your choice (1-11): ");
                  int choice = Convert.ToInt32(Console.ReadLine());
                  switch (choice)
                  {
                      case 1:
                          adminController.ShowUser();
                          break;
                      case 2:
                          adminController.FindAll();
                          break;
                      case 3:
                          adminController.FindByUserName();
                          break;
                      case 4:
                          adminController.FindByUserAccountNumber();
                          break;
                      case 5:
                          adminController.FindByUserPhone();
                          break;
                      case 6:
                          adminController.CreateNewAccount();
                          break;
                      case 7:
                          adminController.BlockAndOpenAccount();
                          break;
                      case 8:
                          adminController.GetList();
                          break;
                      case 9:
                          adminController.ChangeInform(admin);
                          break;
                      case 10:
                          adminController.ChangePassword(admin);
                         break;
                      case 11:
                          Console.WriteLine("Exit Program!!!");
                          break;
                      default:
                          Console.WriteLine("Please choice again!!!!");
                          break;
                  }

                  if (choice == 11)
                  {
                      break;
                  }
              }   
             }
         }
    }
}