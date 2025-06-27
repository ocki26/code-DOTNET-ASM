using System;
using System.Collections.Generic;
using System.Linq;

public class Manager
{
    private List<User> users;
    private List<Course> courses;
    private User currentUser;

    public Manager()
    {
        users = new List<User>();
        courses = new List<Course>();

        // Admin mặc định
        users.Add(new Admin("admin01", "Admin", "admin@gmail.com", "admin"));

        // Tài khoản Student mẫu
        users.Add(new Student("st01", "Lan", "lan@student.com", "123", "CNTT"));
        users.Add(new Student("st02", "Minh", "minh@student.com", "123", "Kinh tế"));

        // Tài khoản Teacher mẫu
        users.Add(new Teacher("tc01", "Thầy Tùng", "tung@teacher.com", "123", "Lập trình"));
        users.Add(new Teacher("tc02", "Cô Hoa", "hoa@teacher.com", "123", "Marketing"));

        // Khóa học mẫu
        courses.Add(new Course("c001", "Lập trình C#", "CNTT"));
        courses.Add(new Course("c002", "Cơ sở dữ liệu", "CNTT"));
        courses.Add(new Course("c003", "Kinh tế vi mô", "Kinh tế"));
        courses.Add(new Course("c004", "Marketing căn bản", "Kinh tế"));
    }

    public void SeedStudent(string id, string name, string email, string password, string major)
    {
        users.Add(new Student(id, name, email, password, major));
    }

    public void SeedTeacher(string id, string name, string email, string password, string work)
    {
        users.Add(new Teacher(id, name, email, password, work));
    }

    public void SeedCourse(string id, string name, string major)
    {
        courses.Add(new Course(id, name, major));
    }

    public void RegisterStudent()
    {
        Console.Write("ID: "); string id = Console.ReadLine();
        Console.Write("Tên: "); string name = Console.ReadLine();
        Console.Write("Email: "); string email = Console.ReadLine();
        Console.Write("Mật khẩu: "); string password = Console.ReadLine();

        if (users.Any(u => u.email == email))
        {
            Console.WriteLine("❌ Email đã tồn tại.");
            return;
        }

        Console.Write("Ngành học: "); string major = Console.ReadLine();
        User student = new Student(id, name, email, password, major);
        users.Add(student);
        Console.WriteLine("✅ Đăng ký tài khoản sinh viên thành công!");
    }

    public void CreateUserByAdmin()
    {
        if (currentUser is not Admin)
        {
            Console.WriteLine("❌ Chỉ admin mới có quyền tạo tài khoản này.");
            return;
        }

        Console.Write("ID: "); string id = Console.ReadLine();
        Console.Write("Tên: "); string name = Console.ReadLine();
        Console.Write("Email: "); string email = Console.ReadLine();
        Console.Write("Mật khẩu: "); string password = Console.ReadLine();
        Console.Write("Loại tài khoản (admin/teacher): "); string role = Console.ReadLine().ToLower();

        if (users.Any(u => u.email == email))
        {
            Console.WriteLine("❌ Email đã tồn tại.");
            return;
        }

        User user = null;
        if (role == "admin")
        {
            user = new Admin(id, name, email, password);
        }
        else if (role == "teacher")
        {
            Console.Write("Công việc: "); string work = Console.ReadLine();
            user = new Teacher(id, name, email, password, work);
        }
        else
        {
            Console.WriteLine("❌ Không hợp lệ.");
            return;
        }

        users.Add(user);
        Console.WriteLine("✅ Tạo tài khoản " + role + " thành công!");
    }

    public void LoginUI()
    {
        Console.Write("Email: ");
        string email = Console.ReadLine();
        Console.Write("Mật khẩu: ");
        string password = Console.ReadLine();

        var user = users.FirstOrDefault(u => u.email == email && u.password == password);

        if (user != null)
        {
            currentUser = user;
            Console.WriteLine($"✅ Xin chào {user.name} ({user.GetRole()})");

            if (user is Student student)
            {
                while (true)
                {
                    Console.WriteLine("\n=== MENU SINH VIÊN ===");
                    Console.WriteLine("1. Xem các khóa học phù hợp");
                    Console.WriteLine("2. Đăng xuất");
                    Console.Write("Chọn: ");
                    string input = Console.ReadLine();

                    if (input == "1")
                        ShowCoursesForStudent(student);
                    else if (input == "2")
                    {
                        Logout();
                        break;
                    }
                    else Console.WriteLine("❌ Lựa chọn không hợp lệ.");
                }
            }
            else if (user is Teacher)
            {
                while (true)
                {
                    Console.WriteLine("\n=== MENU GIẢNG VIÊN ===");
                    Console.WriteLine("1. Xem tất cả các khóa học");
                    Console.WriteLine("2. Đăng xuất");
                    Console.Write("Chọn: ");
                    string input = Console.ReadLine();

                    if (input == "1")
                        ShowAllCourses();
                    else if (input == "2")
                    {
                        Logout();
                        break;
                    }
                    else Console.WriteLine("❌ Lựa chọn không hợp lệ.");
                }
            }
        }
        else
        {
            Console.WriteLine("❌ Sai thông tin đăng nhập.");
        }
    }

    public void AddCourse()
    {
        if (currentUser is not Admin)
        {
            Console.WriteLine("❌ Chỉ admin mới được thêm khóa học.");
            return;
        }

        Console.Write("Mã khóa học: ");
        string id = Console.ReadLine();
        Console.Write("Tên khóa học: ");
        string name = Console.ReadLine();
        Console.Write("Ngành phù hợp: ");
        string major = Console.ReadLine();

        courses.Add(new Course(id, name, major));
        Console.WriteLine("✅ Đã thêm khóa học.");
    }

    public void ShowCoursesForStudent(Student student)
    {
        Console.WriteLine("\n📘 Các khóa học phù hợp với ngành: " + student.Major);
        var matched = courses.Where(c => c.Major.Equals(student.Major, StringComparison.OrdinalIgnoreCase)).ToList();

        if (matched.Count == 0)
        {
            Console.WriteLine("❌ Không có khóa học nào phù hợp.");
            return;
        }

        foreach (var course in matched)
        {
            Console.WriteLine($"{course.CourseId} - {course.CourseName}");
        }
    }

    public void ShowAllCourses()
    {
        Console.WriteLine("\n📚 Danh sách tất cả khóa học:");
        foreach (var course in courses)
        {
            Console.WriteLine($"{course.CourseId} - {course.CourseName} (Ngành: {course.Major})");
        }
    }

    public void RemoveUser()
    {
        if (currentUser is not Admin)
        {
            Console.WriteLine("❌ Chỉ admin được xóa tài khoản.");
            return;
        }

        Console.Write("Nhập ID người dùng cần xóa: ");
        string id = Console.ReadLine();
        var user = users.FirstOrDefault(u => u.id == id);

        if (user != null && user != currentUser)
        {
            users.Remove(user);
            Console.WriteLine("✅ Đã xóa người dùng.");
        }
        else
        {
            Console.WriteLine("❌ Không tìm thấy hoặc không thể xóa chính mình.");
        }
    }

    public void Logout()
    {
        currentUser = null;
        Console.WriteLine("🚪 Đã đăng xuất.");
    }
}
