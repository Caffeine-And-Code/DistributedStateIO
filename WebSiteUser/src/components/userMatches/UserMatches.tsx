import React from "react";
import type { Match } from "../../models/Match";
import { MatchItem } from "./MatchItem";

interface Props {
  matches: Match[];
  isLogged: boolean;
}

export const UserMatches: React.FC<Props> = ({ matches, isLogged }) => {
  if (!isLogged) {
    return (
      <div className="leaderboard">
        <h3>Last matches</h3>
        <p>Login to see your last matches</p>
      </div>
    );
  }

  if (matches.length === 0) {
    return (
      <div className="leaderboard">
        <h3>Last matches</h3>
        <p>No matches</p>
      </div>
    );
  }

  return (
    <div className="leaderboard">
      <h3>Last matches</h3>
      {matches.map((m) => (
        <MatchItem key={m.id} match={m} />
      ))}
    </div>
  );
};
