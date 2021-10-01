using System;
using SHBank.Controller;

namespace SHBank.View
{
    public class Menu
    {
        public void GenerateMenu()
        {
            var adminView = new AdminView();
            var userView = new UserView();
            IAdminController adminController = new AdminController();
            IUserController userController = new UserController();
            while (true)
            {
                Console.WriteLine("—— Ngan hang Spring Hero Bank ——");
                Console.WriteLine("1. Register Admin. ");
                Console.WriteLine("2. Register User. ");
                Console.WriteLine("3. Login Admin. ");
                Console.WriteLine("4. Login User");
                Console.WriteLine("5. Exit Program. ");
                Console.WriteLine("-------------------------");
                Console.WriteLine("Please enter your choice (1-3): ");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        adminController.Register();
                        break;
                    case 2:
                        userController.Register();
                        break;
                    case 3:
                           adminView.GenerateAdmin();
                        break; 
                    case 4:
                          userView.GenerateUser();
                        break;
                    case 5:
                        Console.WriteLine("Exit Program !!");
                        break;
                    default:
                        Console.WriteLine("Please choice again.");
                        break;
                }

                if (choice == 5)
                {
                    break;
                }
            }
        }
    }
}