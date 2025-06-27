using System;

class Program
{
    static void Main(string[] args)
    {
        Manager manager = new Manager();

        while (true)
        {
            Console.WriteLine("\n=== MENU CHÍNH ===");
            Console.WriteLine("1. Đăng ký tài khoản Student");
            Console.WriteLine("2. Đăng nhập");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn: ");
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
                    return;
                default:
                    Console.WriteLine("❌ Không hợp lệ.");
                    break;
            }
        }
    }

    static void SubMenu(Manager manager)
    {
        while (true)
        {
            Console.WriteLine("\n=== MENU SAU ĐĂNG NHẬP ===");
            Console.WriteLine("1. Tạo tài khoản Teacher/Admin (Admin)");
            Console.WriteLine("2. Thêm khóa học (Admin)");
            Console.WriteLine("3. Xóa người dùng (Admin)");
            Console.WriteLine("4. Đăng xuất");
            Console.Write("Chọn: ");
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
                default:
                    Console.WriteLine("❌ Không hợp lệ.");
                    break;
            }
        }
    }
}
