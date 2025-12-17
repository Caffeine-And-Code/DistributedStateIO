import React from "react";
import type { LeaderboardEntry } from "../../models/LeaderboardEntry";
import { LeaderboardRow } from "./LeaderboardRow";

interface Props {
  entries: LeaderboardEntry[];
}

export const Leaderboard: React.FC<Props> = ({ entries }) => {
  return (
    <div className="leaderboard">
      <h3>Leaderboard</h3>
      <div>
        {entries.map((e) => (
          <LeaderboardRow key={e.userId} entry={e} />
        ))}
      </div>
    </div>
  );
};
