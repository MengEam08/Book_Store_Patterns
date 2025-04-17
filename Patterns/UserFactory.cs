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

public class UserFactory
{
    public static UserRole CreateUser(string role)
    {
        if (role == "Admin") return new Admin();
        return new Employee();
    }
}
