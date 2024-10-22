import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate, Link } from 'react-router-dom';

const Login = ({ setLoggedIn }) => {
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();

    try {
      const response = await axios.post('/api/Auth', { login, password });
      localStorage.setItem('token', response.data.login);
      setLoggedIn(true);
      setLogin(login);
      alert("Вы успешно зашли!")
      navigate('/upload');
    } catch (error) {
      alert("Неверное имя пользователя или пароль!")
      console.error(error);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <h2>Вход</h2>
      <div>
        <label htmlFor="login">Логин:</label>
        <input
          type="text"
          id="login"
          value={login}
          onChange={(e) => setLogin(e.target.value)}
        />
      </div>
      <div>
        <label htmlFor="password">Пароль:</label>
        <input
          type="password"
          id="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      </div>
      <button type="submit">Войти</button>
      <p>Нет аккаунта? <Link to="/register">Зарегистрируйтесь</Link></p>
    </form>
  );
};

export default Login;

