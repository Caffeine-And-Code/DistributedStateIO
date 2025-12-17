import React, { useEffect, useState } from "react";
import { ThreeColumnLayout } from "../components/layout/ThreeColumnLayout";
import { Leaderboard } from "../components/leaderboard/Leaderboard";
import { AuthCard } from "../components/auth/AuthCard";
import { UserMatches } from "../components/userMatches/UserMatches";
import type { LeaderboardEntry } from "../models/LeaderboardEntry";
import type { Match } from "../models/Match";
import { getLeaderboard, getUserById } from "../api/webService";

export const Dashboard: React.FC = () => {
  const [leaderboardEntries, setLeaderboardEntries] = useState<LeaderboardEntry[]>([]);
    
  useEffect(() => {
  const fetchLeaderboard = async () => {
    try {
      const data = await getLeaderboard(10, 5); 

      // Trasformiamo ogni elemento della leaderboard
      const transformed: LeaderboardEntry[] = await Promise.all(
        data.map(async (item: any, index: number) => {
          let username = "NaN";

          try {
            const user = await getUserById(item.userId);
            if (user && user.username) {
              username = user.username;
            }
            else {
              username = "NaN"
            }
          } catch (err) {
            console.warn(`Non è stato possibile recuperare l'username per userId ${item.userId}`, err);
          }

          return {
            position: index + 1,
            userId: item.userId,
            username,
            score: item.points,
            recentMatches: item.lastMatches.map((m: any) => ({
              id: m.matchId,
              date: m.endDate,
              result: m.isWinner ? "V" : "P"
            }))
          };
        })
      );

      setLeaderboardEntries(transformed);
    } catch (err) {
      console.error("Errore caricando leaderboard", err);
    }
  };

  fetchLeaderboard();
}, []);

    const mockUserMatches: Match[] = [
    {
        id: 10,
        date: new Date().toISOString(),
        result: "V",
    },
    {
        id: 11,
        date: new Date(Date.now() - 86400000).toISOString(),
        result: "P",
    },
    ];

  const userMatches = mockUserMatches;
  const userId = 1; 

  return (
    <ThreeColumnLayout
        left={<Leaderboard entries={leaderboardEntries} />}
        center={<AuthCard onLogin={() => {}} onRegister={() => {}} />}
        right={<UserMatches matches={userMatches} userId={userId} />}
    />

  );
};