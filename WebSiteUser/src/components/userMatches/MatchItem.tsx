import React from "react";
import type { Match } from "../../models/Match";
import "../../App.css";

interface Props {
  match: Match;
}

export const MatchItem: React.FC<Props> = ({ match }) => {
  return (
    <div className="match-item">
      <div>{new Date(match.date).toLocaleDateString()}</div>
      <div className="match-item-result">{match.result}</div>
    </div>
  );
};
