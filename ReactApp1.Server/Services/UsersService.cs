using Npgsql;
using ReactApp1.Server.Models;
using ReactApp1.Server.Services.Interfaces;
using System.Data;

namespace ReactApp1.Server.Services
{
    public class UsersService : IUsersService
    {
        private ConnectionDB _connectionDB = new();

        public UserModel Create(UserModel model)
        {
            try
            {
                // Добавление пользователя в базу данных
                string command = $"INSERT INTO public.\"Users\"(user_password, user_login) VALUES ('{model.Password}', '{model.Login}');";
                DataTable responseDT = DataBaseCommand(command);
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteByLogin(string login)
        {
            string command = $"DELETE FROM public.\"Users\"\r\n\t WHERE user_login='{login}';";
            try
            {
                DataBaseCommand(command);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public UserModel GetByLogin(string login)
        {
            string command = $"SELECT * FROM public.\"Users\"\r\n\t WHERE user_login='{login}';";
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

        public UserModel Update(UserModel model, string newPass)
        {
            string command = $"UPDATE public.\"Users\"\r\n\t SET user_password='{newPass}' \r\n\t WHERE user_login='{model.Login}' AND user_password='{model.Password}';";
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

