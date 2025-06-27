public class Course
{
    public string CourseId { get; set; }
    public string CourseName { get; set; }
    public string Major { get; set; } // ngành học phù hợp

    public Course(string courseId, string courseName, string major)
    {
        CourseId = courseId;
        CourseName = courseName;
        Major = major;
    }
}