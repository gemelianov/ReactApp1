using Npgsql;
using ReactApp1.Server.Models;
using ReactApp1.Server.Services.Interfaces;
using System.Data;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ReactApp1.Server.Services
{
    public class TagsService : ITagsService
    {
        private ConnectionDB _connectionDB = new();
        public List<TagModel> FindTags(int documentId)
        {
            DocumentModel documentModel = new DocumentModel();
            documentModel = GetById(documentId);
            string documentPath = documentModel.Path;

            // Проверяем, существует ли файл
            if (!File.Exists(documentPath))
            {
                throw new FileNotFoundException("Документ не найден", documentPath);
            }

            var tags = new List<TagModel>();
            // Загружаем документ и извлекаем теги
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documentPath, false))
            {
                var body = wordDoc.MainDocumentPart.Document.Body;
                var text = body.InnerText;

                // Находим теги в тексте
                var regex = new Regex(@"<([^>]*)>");
                var matches = regex.Matches(text);

                foreach (Match match in matches)
                {
                    tags.Add(new TagModel { Name = match.Groups[1].Value});
                }
            }

            return tags;
        }

        private DocumentModel GetById(int id)
        {
            string command = $"SELECT * FROM public.\"Patterns\"\r\n\t WHERE pattern_id = {id}";
            try
            {
                System.Data.DataTable responseDT = DataBaseCommand(command);
                return GetModel(responseDT);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private System.Data.DataTable DataBaseCommand(string request)
        {
            NpgsqlConnection sqlConnection = new NpgsqlConnection(_connectionDB.connectionString);
            sqlConnection.Open();

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = sqlConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = request;

            NpgsqlDataReader reader = command.ExecuteReader();
            System.Data.DataTable dt = new System.Data.DataTable();
            if (reader.HasRows)
            {
                dt.Load(reader);
            }

            return dt;
        }

        private DocumentModel GetModel(System.Data.DataTable dt)
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
