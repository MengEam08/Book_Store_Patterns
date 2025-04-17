using System;

public interface IObserver
{
    void UpdateDashboard();
}

public class Dashboard : IObserver
{
    public void UpdateDashboard()
    {
        // Refresh total products, total sales, total customers
        Console.WriteLine("Dashboard has been updated.");
    }
}
