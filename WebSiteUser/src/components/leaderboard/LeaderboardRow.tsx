import React, { useState, useRef } from "react";
import type { LeaderboardEntry } from "../../models/LeaderboardEntry";
import { PreviewPopup } from "./PreviewPopup";
import "../../App.css"; 

interface Props {
  entry: LeaderboardEntry;
}

export const LeaderboardRow: React.FC<Props> = ({ entry }) => {
  const [hover, setHover] = useState(false);
  const ref = useRef<HTMLDivElement | null>(null);

  return (
    <div
      ref={ref}
      tabIndex={0}
      className="leaderboard-row"
      onMouseEnter={() => setHover(true)}
      onMouseLeave={() => setHover(false)}
      onFocus={() => setHover(true)}
      onBlur={() => setHover(false)}
    >
      <div className="leaderboard-row-left">
        <span className="leaderboard-row-username">#{entry.position}</span>
        <span>{entry.username}</span>
      </div>

      <div>{entry.score}</div>

      <PreviewPopup matches={entry.recentMatches} visible={hover} anchorRef={ref} />
    </div>
  );
};
