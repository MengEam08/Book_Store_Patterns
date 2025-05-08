public abstract class UserRole
{
    public abstract string Role { get; }
}

public class Admin : UserRole
{
    public override string Role => "Admin";
}

public class Employee : UserRole
{
    public override string Role => "Employee";
}

public static class UserFactory
{
    public static UserRole CreateUser(string role)
    {
        switch (role.Trim().ToLower())
        {
            case "admin":
                return new Admin();
            case "employee":
                return new Employee();
            default:
                return new Employee(); // default fallback
        }
    }
}

