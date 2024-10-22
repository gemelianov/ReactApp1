using Npgsql;
using ReactApp1.Server.Models;
using ReactApp1.Server.Services.Interfaces;
using System.Data;

namespace ReactApp1.Server.Services
{
    public class AuthService : IAuthService
    {
        private ConnectionDB _connectionDB = new();

        public UserModel Login(UserModel model)
        {
            string command = $"SELECT * FROM public.\"Users\"\r\n\t WHERE user_login='{model.Login}' AND user_password='{model.Password}';";
            try
            {
                DataTable responseDT = DataBaseCommand(command);
                return GetModel(responseDT);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private DataTable DataBaseCommand(string request)
        {
            NpgsqlConnection sqlConnection = new NpgsqlConnection(_connectionDB.connectionString);
            sqlConnection.Open();

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = sqlConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = request;

            NpgsqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            if (reader.HasRows)
            {
                dt.Load(reader);
            }

            return dt;
        }

        private UserModel GetModel(DataTable dt)
        {
            UserModel model = new UserModel();
            DataRow row = dt.Rows[0];
            model.Login = Convert.ToString(row["user_login"]);
            model.Password = Convert.ToString(row["user_password"]);

            return model;
        }
    }
}

