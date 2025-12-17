import React, { useState } from "react";
import "../../App.css";

interface Props {
  onSubmit: (username: string, password: string) => void;
  onRegister: (username: string, password: string) => void;
}

export const LoginForm: React.FC<Props> = ({ onSubmit, onRegister }) => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  return (
    <form
      className="login-form"
      onSubmit={(e) => {
        e.preventDefault();
        onSubmit(username, password);
      }}
    >
      <div className="login-form-group">
        <label>Username</label>
        <input
          className="login-form-input"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />
      </div>

      <div className="login-form-group">
        <label>Password</label>
        <input
          type="password"
          className="login-form-input"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      </div>

      <div className="login-form-buttons">
        <button type="submit">Login</button>
        <button type="button" onClick={() => onRegister(username, password)}>
          Register
        </button>
      </div>
    </form>
  );
};
