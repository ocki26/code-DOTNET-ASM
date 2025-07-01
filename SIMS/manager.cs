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
        Console.Write("name: "); string name = Console.ReadLine();
        Console.Write("Email: "); string email = Console.ReadLine();
        Console.Write("pass word: "); string password = Console.ReadLine();

        if (users.Any(u => u.email == email))
        {
            Console.WriteLine("❌ Email already exists.");
            return;
        }

        Console.Write("Couser: "); string major = Console.ReadLine();
        User student = new Student(id, name, email, password, major);
        users.Add(student);
        SaveStudents();
        Console.WriteLine("✅ register complete!");
    }

    public void CreateUserByAdmin()
    {
        if (currentUser is not Admin)
        {
            Console.WriteLine("❌ Only admin can create this account.");
            return;
        }

        Console.Write("ID: "); string id = Console.ReadLine();
        Console.Write("name: "); string name = Console.ReadLine();
        Console.Write("Email: "); string email = Console.ReadLine();
        Console.Write("pass work: "); string password = Console.ReadLine();
        Console.Write("type account (admin/teacher): "); string role = Console.ReadLine().ToLower();

        if (users.Any(u => u.email == email))
        {
            Console.WriteLine("❌ Email already exists.");
            return;
        }

        User user = null;
        if (role == "admin")
        {
            user = new Admin(id, name, email, password);
        }
        else if (role == "teacher")
        {
            Console.Write("work: "); string work = Console.ReadLine();
            user = new Teacher(id, name, email, password, work);
        }
        else
        {
            Console.WriteLine("❌ Invalid.");
            return;
        }

        users.Add(user);
        if (user is Teacher) SaveTeachers();
        Console.WriteLine("✅ create account " + role + " success!");
    }

    public void AddCourse()
    {
        if (currentUser is not Admin)
        {
            Console.WriteLine("❌ Only admin can add courses.");
            return;
        }

        Console.Write("id courser: ");
        string id = Console.ReadLine();
        Console.Write("name courser: ");
        string name = Console.ReadLine();
        Console.Write("Suitable industry: ");
        string major = Console.ReadLine();

        courses.Add(new Course(id, name, major));
        SaveCourses();
        Console.WriteLine("✅ Course added.");
    }

    public void LoginUI()
    {
        Console.Write("Email: ");
        string email = Console.ReadLine();
        Console.Write("pass word: ");
        string password = Console.ReadLine();

        var user = users.FirstOrDefault(u => u.email == email && u.password == password);

        if (user != null)
        {
            currentUser = user;
            Console.WriteLine($"✅ hello {user.name} ({user.GetRole()})");

            if (user is Student student)
            {
                while (true)
                {
                    Console.WriteLine("=== STUDENT MENU ===");
                    Console.WriteLine("1. View relevant courses");
                    Console.WriteLine("2. log out");
                    Console.Write("choose: ");
                    string input = Console.ReadLine();

                    if (input == "1")
                        ShowCoursesForStudent(student);
                    else if (input == "2")
                    {
                        Logout();
                        break;
                    }
                    else Console.WriteLine("❌Invalid selection.");
                }
            }
            else if (user is Teacher)
            {
                while (true)
                {
                    Console.WriteLine("=== LECTURER MENU ===");
                    Console.WriteLine("1. View all courses");
                    Console.WriteLine("2. Log out");
                    Console.Write("choose: ");
                    string input = Console.ReadLine();

                    if (input == "1")
                        ShowAllCourses();
                    else if (input == "2")
                    {
                        Logout();
                        break;
                    }
                    else Console.WriteLine("❌ Invalid selection.");
                }
            }
        }
        else
        {
            Console.WriteLine("❌Incorrect login information.");
        }
    }

    public void ShowCoursesForStudent(Student student)
    {
        Console.WriteLine("📘 Courses relevant to the industry: " + student.Major);
        var matched = courses.Where(c => c.Major.Equals(student.Major, StringComparison.OrdinalIgnoreCase)).ToList();

        if (matched.Count == 0)
        {
            Console.WriteLine("❌No courses match.");
            return;
        }

        foreach (var course in matched)
        {
            Console.WriteLine($"{course.CourseId} - {course.CourseName}");
        }
    }

    public void ShowAllCourses()
    {
        Console.WriteLine("📚List of all courses:");
        foreach (var course in courses)
        {
            Console.WriteLine($"{course.CourseId} - {course.CourseName} (courser: {course.Major})");
        }
    }

    public void Logout()
    {
        currentUser = null;
        Console.WriteLine("🚪 log out.");
    }
    public void RemoveUser()
{
    if (currentUser is not Admin)
    {
        Console.WriteLine("❌ Only admin can delete account.");
        return;
    }

    ShowUsers();
    Console.Write("Enter the user ID to delete:");
    string id = Console.ReadLine();
    var user = users.FirstOrDefault(u => u.id == id);

    if (user != null && user != currentUser)
    {
        users.Remove(user);

        if (user is Student) SaveStudents();
        else if (user is Teacher) SaveTeachers();

        Console.WriteLine("✅User deleted.");
    }
    else
    {
        Console.WriteLine("❌ Could not find or delete itself.");
    }
}

public void editAccount()
{
    if (currentUser is not Admin)
    {
        Console.WriteLine("❌ Only admin can edit account.");
        return;
    }

    ShowUsers();
    Console.Write("Select the account ID to edit:");
    string id = Console.ReadLine();

    if (id == currentUser.id)
    {
        Console.WriteLine("❌ Can't edit myself.");
        return;
    }

    var user = users.FirstOrDefault(u => u.id == id);
    if (user == null)
    {
        Console.WriteLine("❌ User not found.");
        return;
    }

    Console.WriteLine("1. edit name");
    Console.WriteLine("2. edit  email");
    Console.WriteLine("3. edit password");
    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.Write("add new name: ");
            user.name = Console.ReadLine();
            break;
        case "2":
            Console.Write("add new email: ");
            string newEmail = Console.ReadLine();
            if (users.Any(u => u.email == newEmail))
            {
                Console.WriteLine("❌ Email already exists.");
                return;
            }
            user.email = newEmail;
            break;
        case "3":
            Console.Write("Enter new password: ");
            user.password = Console.ReadLine();
            break;
        default:
            Console.WriteLine("❌ Invalid selection.");
            return;
    }

    if (user is Student) SaveStudents();
    else if (user is Teacher) SaveTeachers();

    Console.WriteLine("✅ Account updated.");
}

public void ShowUsers()
{
    Console.WriteLine("\n📋 List of users:");
    foreach (var user in users)
    {
        Console.WriteLine($"{user.id} - {user.name} ({user.GetRole()}) - {user.email}");
    }
}

}
