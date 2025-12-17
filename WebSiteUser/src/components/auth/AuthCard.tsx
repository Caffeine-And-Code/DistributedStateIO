import React from "react";
import type { User } from "../../models/User";
import { LoginForm } from "./LoginForm";
import { StartMatchButton } from "./StartMatchButton";
import "../../App.css";

interface Props {
  user?: User | null;
  onLogin: (username: string, password: string) => void;
  onRegister: (username: string, password: string) => void;
  onStartMatch?: () => void;
}

export const AuthCard: React.FC<Props> = ({
  user,
  onLogin,
  onRegister,
  onStartMatch,
}) => {
  return (
    <div className="auth-card">
      <h1 className="auth-card-title">State.io</h1>

      {user ? (
        <div className="auth-card-logged">
          <p>Benvenuto, {user.username}</p>
          <StartMatchButton onStartMatch={onStartMatch || (() => {})} />
        </div>
      ) : (
        <LoginForm onSubmit={onLogin} onRegister={onRegister} />
      )}
    </div>
  );
};
