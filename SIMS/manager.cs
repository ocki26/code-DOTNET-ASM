using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Manager
{
    public bool IsLoggedIn => currentUser != null && currentUser is Admin;

    private List<User> users;
    private List<Course> courses;
    private User currentUser;

    private readonly string studentFile = "Data/students.csv";
    private readonly string teacherFile = "Data/teachers.csv";
    private readonly string courseFile = "Data/courses.csv";

    public Manager()
    {
        users = new List<User>();
        courses = new List<Course>();
        LoadDataFromFiles();

        // Admin mặc định
        if (!users.Any(u => u is Admin))
        {
            users.Add(new Admin("admin01", "Admin", "admin@gmail.com", "admin"));
        }
    }

    private void LoadDataFromFiles()
    {
        var studentRecords = CsvService.LoadFromCsv<StudentRecord>(studentFile);
        foreach (var s in studentRecords)
        {
            users.Add(new Student(s.Id, s.Name, s.Email, s.Password, s.Major));
        }

        var teacherRecords = CsvService.LoadFromCsv<TeacherRecord>(teacherFile);
        foreach (var t in teacherRecords)
        {
            users.Add(new Teacher(t.Id, t.Name, t.Email, t.Password, t.Work));
        }

        courses = CsvService.LoadFromCsv<Course>(courseFile);
    }

    private void SaveStudents()
    {
        var students = users.OfType<Student>().Select(s => new StudentRecord
        {
            Id = s.id,
            Name = s.name,
            Email = s.email,
            Password = s.password,
            Major = s.Major
        }).ToList();

        CsvService.SaveToCsv(students, studentFile);
    }

    private void SaveTeachers()
    {
        var teachers = users.OfType<Teacher>().Select(t => new TeacherRecord
        {
            Id = t.id,
            Name = t.name,
            Email = t.email,
            Password = t.password,
            Work = t.Work
        }).ToList();

        CsvService.SaveToCsv(teachers, teacherFile);
    }

    private void SaveCourses()
    {
        CsvService.SaveToCsv(courses, courseFile);
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
        SaveStudents();
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
        if (user is Teacher) SaveTeachers();
        Console.WriteLine("✅ Tạo tài khoản " + role + " thành công!");
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
        SaveCourses();
        Console.WriteLine("✅ Đã thêm khóa học.");
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
                    Console.WriteLine("=== MENU SINH VIÊN ===");
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
                    Console.WriteLine("=== MENU GIẢNG VIÊN ===");
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

    public void ShowCoursesForStudent(Student student)
    {
        Console.WriteLine("📘 Các khóa học phù hợp với ngành: " + student.Major);
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
        Console.WriteLine("📚 Danh sách tất cả khóa học:");
        foreach (var course in courses)
        {
            Console.WriteLine($"{course.CourseId} - {course.CourseName} (Ngành: {course.Major})");
        }
    }

    public void Logout()
    {
        currentUser = null;
        Console.WriteLine("🚪 Đã đăng xuất.");
    }
    public void RemoveUser()
{
    if (currentUser is not Admin)
    {
        Console.WriteLine("❌ Chỉ admin được xóa tài khoản.");
        return;
    }

    ShowUsers();
    Console.Write("Nhập ID người dùng cần xóa: ");
    string id = Console.ReadLine();
    var user = users.FirstOrDefault(u => u.id == id);

    if (user != null && user != currentUser)
    {
        users.Remove(user);

        if (user is Student) SaveStudents();
        else if (user is Teacher) SaveTeachers();

        Console.WriteLine("✅ Đã xóa người dùng.");
    }
    else
    {
        Console.WriteLine("❌ Không tìm thấy hoặc không thể xóa chính mình.");
    }
}

public void editAccount()
{
    if (currentUser is not Admin)
    {
        Console.WriteLine("❌ Chỉ admin được chỉnh sửa tài khoản.");
        return;
    }

    ShowUsers();
    Console.Write("Chọn ID tài khoản cần sửa: ");
    string id = Console.ReadLine();

    if (id == currentUser.id)
    {
        Console.WriteLine("❌ Không thể chỉnh sửa chính mình.");
        return;
    }

    var user = users.FirstOrDefault(u => u.id == id);
    if (user == null)
    {
        Console.WriteLine("❌ Không tìm thấy người dùng.");
        return;
    }

    Console.WriteLine("1. Sửa tên");
    Console.WriteLine("2. Sửa email");
    Console.WriteLine("3. Sửa mật khẩu");
    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.Write("Nhập tên mới: ");
            user.name = Console.ReadLine();
            break;
        case "2":
            Console.Write("Nhập email mới: ");
            string newEmail = Console.ReadLine();
            if (users.Any(u => u.email == newEmail))
            {
                Console.WriteLine("❌ Email đã tồn tại.");
                return;
            }
            user.email = newEmail;
            break;
        case "3":
            Console.Write("Nhập mật khẩu mới: ");
            user.password = Console.ReadLine();
            break;
        default:
            Console.WriteLine("❌ Lựa chọn không hợp lệ.");
            return;
    }

    if (user is Student) SaveStudents();
    else if (user is Teacher) SaveTeachers();

    Console.WriteLine("✅ Đã cập nhật tài khoản.");
}

public void ShowUsers()
{
    Console.WriteLine("\n📋 Danh sách người dùng:");
    foreach (var user in users)
    {
        Console.WriteLine($"{user.id} - {user.name} ({user.GetRole()}) - {user.email}");
    }
}

}
