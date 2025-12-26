import React, { useEffect, useState } from "react";
import { ThreeColumnLayout } from "../components/layout/ThreeColumnLayout";
import { Leaderboard } from "../components/leaderboard/Leaderboard";
import { AuthCard } from "../components/auth/AuthCard";
import { UserMatches } from "../components/userMatches/UserMatches";
import type { LeaderboardEntry } from "../models/LeaderboardEntry";
import type { User } from "../models/User";
import type { Match } from "../models/Match";
import {
  getLeaderboard,
  login,
  register,
  getMyMatches,
  getMe
} from "../api/webService";

export const Dashboard: React.FC = () => {
  const [leaderboardEntries, setLeaderboardEntries] = useState<LeaderboardEntry[]>([]);
  const [user, setUser] = useState<User | null>(null);
  const [authMsg, setAuthMsg] = useState<string | null>(null);
  const [matches, setMatches] = useState<Match[]>([]);
  const [token, setToken] = useState<string | null>(
    localStorage.getItem("access_token")
  );

  useEffect(() => {
    if (!token) return;

    getMe(token)
      .then(setUser)
      .catch(() => {
        localStorage.removeItem("access_token");
        setToken(null);
      });
  }, []);

  useEffect(() => {
    const fetchLeaderboard = async () => {
      try {
        const data = await getLeaderboard(10, 5);

        const transformed: LeaderboardEntry[] = data.map(
          (item: any, index: number) => ({
            position: index + 1,
            userId: item.userId,
            username: item.username, 
            score: item.points,
            recentMatches: item.lastMatches.map((m: any) => ({
              id: m.matchId,
              date: m.endDate,
              result: m.isWinner ? "V" : "P",
            })),
          })
        );

        setLeaderboardEntries(transformed);
      } catch (err) {
        console.error("Error loading leaderboard.", err);
      }
    };

    fetchLeaderboard();
  }, []);

  useEffect(() => {
    if (!token) {
      setMatches([]);
      return;
    }

    const fetchMatches = async () => {
      try {
        const data = await getMyMatches(token);

        const transformed: Match[] = data.map((m: any) => ({
          id: m.matchId,
          date: m.endDate,
          result: m.isWinner ? "V" : "P",
        }));

        setMatches(transformed);
      } catch {
        setMatches([]);
      }
    };

    fetchMatches();
  }, [token]);

  const handleLogin = async (username: string, password: string) => {
    try {
      setAuthMsg(null);

      const token = await login(username, password);
      localStorage.setItem("access_token", token);
      setToken(token);

      const me = await getMe(token);
      setUser(me);
    } catch (err: any) {
      setAuthMsg(err?.response?.data?.message || "Incorrect username or password.");
    }
  };

  const handleRegister = async (username: string, password: string) => {
    try {
      setAuthMsg(null);
      await register(username, password);
      setAuthMsg("User successfully created. You can now log in.");
    } catch (err: any) {
      setAuthMsg(err?.response?.data?.message || "Error during registration.");
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("access_token");
    setToken(null);
    setUser(null);
    setMatches([]);
    setAuthMsg(null);
  };

  return (
    <ThreeColumnLayout
      left={<Leaderboard entries={leaderboardEntries} />}
      center={
        <AuthCard
          user={user}
          message={authMsg}
          onLogin={handleLogin}
          onRegister={handleRegister}
          onLogout={handleLogout}
          onStartMatch={() => console.log("Start match")}
        />
      }
      right={<UserMatches matches={matches} isLogged={!!token} />}
    />
  );
};
