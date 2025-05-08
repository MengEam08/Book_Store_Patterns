using System.Data.SqlClient;

public sealed class DatabaseConnection
{
    private static DatabaseConnection _instance = null;
    private static readonly object _lock = new object();
    private SqlConnection _connection;

    private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\OOAD\S2 Project\MiniStore\BookStoreMG\MyBook.mdf;Integrated Security=True";

    private DatabaseConnection()
    {
        _connection = new SqlConnection(connectionString);
    }

    public static DatabaseConnection Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                    _instance = new DatabaseConnection();
                return _instance;
            }
        }
    }

    public SqlConnection Connection
    {
        get
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
                _connection.Open();
            return _connection;
        }
    }

    public void Close()
    {
        if (_connection.State == System.Data.ConnectionState.Open)
            _connection.Close();
    }
}
