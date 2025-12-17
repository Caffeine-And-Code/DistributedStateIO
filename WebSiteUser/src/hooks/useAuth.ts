import { useState } from "react";
import type { User } from "../models/User";

export function useAuth() {
  const [user, setUser] = useState<User | null>(() => {
    try {
      const raw = localStorage.getItem('user');
      return raw ? (JSON.parse(raw) as User) : null;
    } catch { return null; }
  });

  function login(username: string, _password: string) {
    const u: User = { id: Math.floor(Math.random() * 10000), username };
    localStorage.setItem('user', JSON.stringify(u));
    setUser(u);
  }

  function register(username: string, _password: string) {
    login(username, _password);
  }

  function logout() {
    localStorage.removeItem('user');
    setUser(null);
  }

  return { user, login, register, logout } as const;
}