import React from "react";
import type { Match } from "../../models/Match";
import { MatchItem } from "./MatchItem";

interface Props {
  matches: Match[];
  userId: number;
}

export const UserMatches: React.FC<Props> = ({ matches, userId }) => {

  if (matches.length === 0) {
    return (
    <div className="leaderboard">
      <h3>Last matches</h3>
      <div>
        <p>No matches</p>
      </div>
    </div>
  );
  }

  return (
    <div className="leaderboard">
      <h3>Last matches</h3>
      <div>
        
        {matches.map((m) => (
          <MatchItem key={m.id} match={m} userId={userId} />
        ))}
      </div>
    </div>
  );
};