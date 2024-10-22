using ReactApp1.Server.Models;
using Npgsql;
using ReactApp1.Server.Services.Interfaces;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace ReactApp1.Server.Services
{
    public class PatternsUploaderService: IPatternsUploader
    {
        private ConnectionDB _connectionDB = new();
        public async Task UploadFileAsync([FromForm] PatternUploadModel upmodel)
        {
            // Сохранение файла на сервере
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data\\Patterns");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var filePath = Path.Combine(uploadsFolder, $"{upmodel.Login}_{upmodel.File.FileName}");

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await upmodel.File.CopyToAsync(fileStream);
            }

            DocumentModel model = new DocumentModel();
            model.Path = filePath;
            model.UserLogin = upmodel.Login;
            try
            {
                // Автоопределение ID документа
                string exist = $"SELECT * FROM public.\"Patterns\"\r\n\t WHERE pattern_path = '{model.Path}'";
                DataTable dt = DataBaseCommand(exist);
                if (dt.Rows.Count > 0)
                {
                    model.Id = GetModel(dt).Id;
                }
                else
                {
                    string findLastId = "SELECT * FROM public.\"Patterns\"\r\n\t ORDER BY pattern_id DESC LIMIT 1";
                    DataTable IdDt = DataBaseCommand(findLastId);
                    if (IdDt.Rows.Count == 0)
                        model.Id = 0;
                    else
                        model.Id = GetModel(IdDt).Id + 1;
                }
                
                // Добавление документа в базу данных
                string command = $"INSERT INTO public.\"Patterns\"(pattern_id, pattern_path, pattern_user_login) VALUES ({model.Id}, '{model.Path}', '{model.UserLogin}');";
                DataTable responseDT = DataBaseCommand(command);
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
