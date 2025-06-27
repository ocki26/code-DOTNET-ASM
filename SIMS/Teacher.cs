public class Teacher : User
{
  public string Work { get; set; } // công việc (môn dạy, phòng ban,...)

  public override string GetRole() => "Teacher";
  public Teacher(string id, string name, string email, string password, string work)
      : base(id, name, email, password)
  {
    this.Work = work;
  }
}
