public class Admin : User
{
    public Admin(string id, string name, string email, string password)
        : base(id, name, email, password)
    {
    }

    public override string GetRole() => "Admin";
}