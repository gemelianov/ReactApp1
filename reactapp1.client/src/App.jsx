import React, { useState, useEffect } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';
import UploadFile from './components/UploadFile';
import FillDocument from './components/FillDocument';

function App() {
  const [loggedIn, setLoggedIn] = useState(false);
  const [login, setLogin] = useState('');

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      setLoggedIn(true);
      setLogin(token);
    } else {
      setLoggedIn(false);
      setLogin('');
    }
  }, []);

  return (
    <BrowserRouter>
      <div>
        {loggedIn && (
          <p>Логин: {login}</p>
        )}
        <Routes>
          <Route path="/" element={<Login setLoggedIn={setLoggedIn} />} />
          <Route path="/register" element={<Register />} />
          <Route path="/upload" element={<UploadFile />} />
          <Route path="/fill-document/:fileId" element={<FillDocument />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;

