public class User
{
  private string Id;
  public string id
  {
    get { return Id; }
    set { Id = value; }
  }
  private string Name;
  public string name
  {
    get { return Name; }
    set { Name = value; }
  }
  private string Email;
  public string email
  {
    get { return Email; }
    set { Email = value; }
  }
  private string Passsword;
  public string password
  {
    get { return Passsword; }
    set { Passsword = value; }
  }
  public virtual string GetRole() => "User";
  public User(string id, string name, string email, string password)
  {
    this.id = id;
    this.name = name;
    this.email = email;
    this.password = password;
  }
    
}
