import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';

const FillDocument = () => {
  const { fileId } = useParams();
  const navigate = useNavigate();
  const [tags, setTags] = useState([]);
  const [tagValues, setTagValues] = useState({});
  const [errorMessage, setErrorMessage] = useState(null);
  const [isFilling, setIsFilling] = useState(false);
  const [downloadUrl, setDownloadUrl] = useState(null); // Добавьте состояние для URL-адреса загрузки

  useEffect(() => {
    const fetchTags = async () => {
      try {
        const response = await axios.get(`/api/Tags/${fileId}`);
        setTags(response.data);
        setTagValues(response.data.reduce((acc, tag) => ({ ...acc, [tag.name]: '' }), {}));
      } catch (error) {
        setErrorMessage("Произошла ошибка при получении тэгов.");
      }
    };

    if (fileId) {
      fetchTags();
    }
  }, [fileId]);

  const handleTagInputChange = (event) => {
    const name = event.target.name;
    const value = event.target.value;
    setTagValues({ ...tagValues, [name]: value });
  };

  const handleFillDocument = async () => {
    setIsFilling(true);
    try {
      const response = await axios.post(`/api/DocumentFiller/${fileId}/fill`, tagValues, {
        responseType: 'blob', 
      });

      // После успешного заполнения
      // Сохраняем URL-адрес для загрузки
      setDownloadUrl(window.URL.createObjectURL(response.data)); 
      setIsFilling(false);
    } catch (error) {
      setErrorMessage("Произошла ошибка при заполнении документа.");
      setIsFilling(false);
    }
  };

  const handleDownload = () => {
    if (downloadUrl) {
      const link = document.createElement('a');
      link.href = downloadUrl;
      link.setAttribute('download', `${fileId}_filled.docx`);
      document.body.appendChild(link);
      link.click();
      link.remove();
      setDownloadUrl(null); // Сбросьте URL-адрес после загрузки
    }
  };

  return (
    <div>
      <h1>Заполнение документа: {fileId}</h1>
      <h2>Найденные тэги:</h2>
      {errorMessage && <p>{errorMessage}</p>}
      <ul>
        {tags.map((tag) => (
          <li key={tag.name}>
            <label htmlFor={tag.name}>{tag.name}:</label>
            <input
              type="text"
              id={tag.name}
              name={tag.name}
              value={tagValues[tag.name]}
              onChange={handleTagInputChange}
            />
          </li>
        ))}
      </ul>
      <button onClick={handleFillDocument} disabled={!Object.values(tagValues).every(Boolean) || isFilling}>
        {isFilling ? 'Заполнение...' : 'Заполнить документ'}
      </button>
      {downloadUrl && ( // Отобразите ссылку, если downloadUrl не null
        <button onClick={handleDownload}>Скачать документ</button>
      )}
    </div>
  );
};

export default FillDocument;

