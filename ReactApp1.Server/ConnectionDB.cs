namespace ReactApp1.Server
{
    public class ConnectionDB
    {
        public string connectionString;

        public ConnectionDB()
        {
            connectionString = "Server=localhost;Port=5432;Database=AutoFillDB; User Id = postgres; Password=postgres;";
        }
    }
}
