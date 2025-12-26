import React from "react";
import type { User } from "../../models/User";
import { LoginForm } from "./LoginForm";
import { StartMatchButton } from "./StartMatchButton";
import "../../App.css";

interface Props {
  user?: User | null;
  message?: string | null;
  onLogin: (username: string, password: string) => void;
  onRegister: (username: string, password: string) => void;
  onStartMatch: () => void;
  onLogout: () => void;
}

export const AuthCard: React.FC<Props> = ({
  user,
  message,
  onLogin,
  onRegister,
  onStartMatch,
  onLogout,
}) => {
  return (
    <div className="auth-card">
      <h1 className="auth-card-title">State.io</h1>

      {user ? (
        <div className="auth-card-logged">
          <p>Benvenuto, {user.username}</p>
          <StartMatchButton onStartMatch={onStartMatch} />
          <button onClick={onLogout} style={{ marginTop: 10 }}>
            Logout
          </button>
        </div>
      ) : (
        <LoginForm onSubmit={onLogin} onRegister={onRegister} />
      )}

      {message && <p className="auth-error">{message}</p>}
    </div>
  );
};
