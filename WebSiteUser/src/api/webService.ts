import axios from "axios";

const BASE_URL = "http://localhost:5005/api";

const api = axios.create({
  baseURL: BASE_URL,
});

export const login = async (username: string, password: string) => {
  const resp = await api.post("/login", { username, password });
  return resp.data.token;
};

export const register = async (username: string, password: string) => {
  const resp = await api.post("/register", { username, password });
  return resp.data;
};

export const getMe = async (token: string) => {
  const resp = await api.get("/me", { headers: { Authorization: `Bearer ${token}` } });
  return resp.data;
};

export const getLeaderboard = async (topN = 10, lastM = 5) => {
  const resp = await api.get(`/leaderboard?topN=${topN}&lastM=${lastM}`);
  return resp.data;
};

export const getUserMatches = async (userId: number, lastN = 5) => {
  const resp = await api.get(`/users/${userId}/matches?lastN=${lastN}`);
  return resp.data;
};

export const getUserById = async (userId: number) => {
    const resp = await api.get(`/users/${userId}`);
  return resp.data;
}
