using Npgsql;
using ReactApp1.Server.Models;
using ReactApp1.Server.Services.Interfaces;
using System.Data;

namespace ReactApp1.Server.Services
{
    public class PatternsService : IPatternsService
    {
        private ConnectionDB _connectionDB = new();

        public DocumentModel Create(DocumentModel model)
        {
            try
            {
                // Автоопределение ID документа
                string findLastId = "SELECT * FROM public.\"Patterns\"\r\n\t ORDER BY pattern_id DESC LIMIT 1";
                DataTable IdDt = DataBaseCommand(findLastId);
                if (IdDt.Rows.Count == 0)
                    model.Id = 0;
                else
                    model.Id = GetModel(IdDt).Id + 1;
                // Добавление модели документа в базу данных
                string command = $"INSERT INTO public.\"Patterns\"(pattern_id, pattern_path, pattern_user_login) VALUES ({model.Id}, '{model.Path}', '{model.UserLogin}');";
                DataTable responseDT = DataBaseCommand(command);
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteById(int id)
        {
            string command = $"DELETE FROM public.\"Patterns\"\r\n\t WHERE pattern_id={id};";
            try
            {
                DataBaseCommand(command);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<DocumentModel> GetAll(string login)
        {
            string command = $"SELECT * FROM public.\"Patterns\"\r\n\t WHERE pattern_user_login = '{login}'";
            try
            {
                DataTable responseDT = DataBaseCommand(command);
                return GetList(responseDT);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DocumentModel GetById(int id)
        {
            string command = $"SELECT * FROM public.\"Patterns\"\r\n\t WHERE pattern_id = {id}";
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

        private List<DocumentModel> GetList(DataTable dt)
        {
            List<DocumentModel> list = new List<DocumentModel>();
            foreach (DataRow row in dt.Rows)
            {
                DocumentModel model = new DocumentModel();
                model.Id = Convert.ToInt32(row["pattern_id"]);
                model.Path = Convert.ToString(row["pattern_path"]);
                model.UserLogin = Convert.ToString(row["pattern_user_login"]);
                list.Add(model);
            }
            return list;
        }

        private DocumentModel GetModel(DataTable dt)
        {
            DocumentModel model = new DocumentModel();
            DataRow row = dt.Rows[0];
            model.Id = Convert.ToInt32(row["pattern_id"]);
            model.Path = Convert.ToString(row["pattern_path"]);
            model.UserLogin = Convert.ToString(row["pattern_user_login"]);

            return model;
        }
    }
}
