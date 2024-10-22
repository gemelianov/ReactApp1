import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const UploadComponent = () => {
  const [file, setFile] = useState(null);
  const [uploadStatus, setUploadStatus] = useState('');
  const [uploadedFiles, setUploadedFiles] = useState([]);

  useEffect(() => {
    const fetchUploadedFiles = async () => {
      const storedLogin = localStorage.getItem('token');
      if (storedLogin) {
        try {
          const response = await axios.get(`/api/Patterns/(login)?login=${storedLogin}`, storedLogin);
          setUploadedFiles(response.data);
          console.log(response.data)
        } catch (error) {
          console.error('Ошибка получения списка файлов:', error);
        }
      }
    };

    fetchUploadedFiles();
  }, []);

  const navigate = useNavigate();
  const handleFillDocument = (fileId) => {
    navigate(`/fill-document/${fileId}`);
  };

  const handleFileChange = (event) => {
    setFile(event.target.files[0]);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setUploadStatus('Загрузка...');

    const formData = new FormData();
    const storedLogin = localStorage.getItem('token');
    formData.append('Login', storedLogin);
    formData.append('File', file);

    try {
      const response = await axios.post('/api/PatternsUpload', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      });
      setUploadStatus('Файл успешно загружен.');
    } catch (error) {
      setUploadStatus('Ошибка загрузки.');
      console.error(error);
    }
  };

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="file">Файл:</label>
          <input type="file" id="file" accept=".docx" onChange={handleFileChange} />
        </div>
        <button type="submit">Загрузить</button>
        <div>{uploadStatus}</div>
      </form>

      <h2>Загруженные файлы:</h2>
      <ul>
        {uploadedFiles.map((file) => (
          <li key={file.id}>
            {file.path.split('\\').pop()} 
            <button onClick={() => handleFillDocument(file.id)}>Заполнить</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default UploadComponent;