using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ReactApp1.Server.Models;
using ReactApp1.Server.Services.Interfaces;
using System.Data;
using System.Text.RegularExpressions;

namespace ReactApp1.Server.Services
{
    public class DocumentFiller : IDocumentFiller
    {
        private ConnectionDB _connectionDB = new();
        public async Task<byte[]> FillDocument(int fileId, Dictionary<string, string> tagValues)
        {
            DocumentModel model = new DocumentModel();
            model = GetById(fileId);
            string documentPath = model.Path;
            // Проверяем, существует ли файл
            if (!File.Exists(documentPath))
            {
                throw new FileNotFoundException("Документ не найден", documentPath);
            }

            string newFileName = Path.GetFileNameWithoutExtension(documentPath) + "_filled" + Path.GetExtension(documentPath);
            string newFilePath = Path.Combine("Data\\FilledDocs", newFileName);

            // Клонируем документ
            File.Copy(documentPath, newFilePath);

            using (WordprocessingDocument doc =
                    WordprocessingDocument.Open(newFilePath, true))
            {
                var body = doc.MainDocumentPart.Document.Body;
                var paras = body.Elements<Paragraph>();

                foreach (var para in paras)
                {
                    foreach (var run in para.Elements<Run>())
                    {
                        foreach (var text in run.Elements<Text>())
                        {
                            foreach (var tag in tagValues)
                            {
                                if (text.Text.Contains(tag.Key))
                                {
                                    text.Text = text.Text.Replace(tag.Key, tag.Value);
                                }
                            }
                        }
                    }
                }
            }
            DocumentModel documentModel = new DocumentModel();
            documentModel.Path = newFilePath;
            documentModel.UserLogin = model.UserLogin;
            documentModel.Id = 0;
            Create(documentModel);

            // Возвращаем файл для скачивания
            var fileBytes = System.IO.File.ReadAllBytes(newFilePath);
            return fileBytes;
        }

        private DocumentModel GetById(int id)
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

        private void Create(DocumentModel model)
        {
            try
            {
                // Автоопределение ID документа
                string findLastId = "SELECT * FROM public.\"FilledDocuments\"\r\n\t ORDER BY document_id DESC LIMIT 1";
                DataTable IdDt = DataBaseCommand(findLastId);
                if (IdDt.Rows.Count == 0)
                    model.Id = 0;
                else
                    model.Id = GetModelDoc(IdDt).Id + 1;
                // Добавление документа в базу данных
                string command = $"INSERT INTO public.\"FilledDocuments\"(document_id, document_path, document_user_login) VALUES ({model.Id}, '{model.Path}', '{model.UserLogin}');";
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

        private DocumentModel GetModelDoc(DataTable dt)
        {
            DocumentModel model = new DocumentModel();
            DataRow row = dt.Rows[0];
            model.Id = Convert.ToInt32(row["document_id"]);
            model.Path = Convert.ToString(row["document_path"]);
            model.UserLogin = Convert.ToString(row["document_user_login"]);

            return model;
        }
    }
}
