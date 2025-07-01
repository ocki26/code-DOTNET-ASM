using System;

class Program
{
    static void Main(string[] args)
    {
        Manager manager = new Manager();

        while (true)
        {
            Console.WriteLine("\n=== MAIN MENU ===");
            Console.WriteLine("1. Register Student Account");
            Console.WriteLine("2. Login");
            Console.WriteLine("0. Exit");
            Console.Write("Choose: ");
            string mainChoice = Console.ReadLine();

            switch (mainChoice)
            {
                case "1":
                    manager.RegisterStudent();
                    break;
                case "2":
                    manager.LoginUI();
                    SubMenu(manager);
                    break;
                case "0":
                    Console.WriteLine("👋 Goodbye!");
                    return;
                default:
                    Console.WriteLine("❌ Invalid option.");
                    break;
            }
        }
    }

    static void SubMenu(Manager manager)
    {
        while (true)
        {
            Console.WriteLine("\n=== POST-LOGIN MENU ===");
            Console.WriteLine("1. Create Teacher/Admin Account (Admin Only)");
            Console.WriteLine("2. Add Course (Admin Only)");
            Console.WriteLine("3. Remove User (Admin Only)");
            Console.WriteLine("4. Logout");
            Console.WriteLine("5. Edit Account (Admin Only)");
            Console.WriteLine("6. Show All Users (Admin Only)");
            Console.Write("Choose: ");
            string subChoice = Console.ReadLine();

            switch (subChoice)
            {
                case "1":
                    manager.CreateUserByAdmin();
                    break;
                case "2":
                    manager.AddCourse();
                    break;
                case "3":
                    manager.RemoveUser();
                    break;
                case "4":
                    manager.Logout();
                    return;
                case "5":
                    manager.editAccount();
                    break;
                case "6":
                    manager.ShowUsers();
                    break;
                default:
                    Console.WriteLine("❌ Invalid option.");
                    break;
            }
        }
    }
}
