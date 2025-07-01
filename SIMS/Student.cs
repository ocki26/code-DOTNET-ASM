public class Student : User
{
  public string Major { get; set; } 


  public override string GetRole() => "Student";
  public Student(string id, string name, string email, string password, string major)
      : base(id, name, email, password)
  {
    this.Major = major;
  }
}
