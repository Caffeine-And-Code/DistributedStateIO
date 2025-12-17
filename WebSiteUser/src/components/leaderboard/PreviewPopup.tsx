import React from "react";
import type { Match } from "../../models/Match";
import "../../App.css";

interface Props {
  matches: Match[];
  visible: boolean;
  anchorRef?: React.RefObject<HTMLElement | null>;
}

export const PreviewPopup: React.FC<Props> = ({ matches, visible }) => {
  if (!visible) return null;

  return (
    <div role="tooltip" className="preview-popup">
      <div className="preview-popup-title">Last matches</div>
      <ul className="preview-popup-list">
        {matches.map((m) => (
          <li key={m.id}>
            <span>{new Date(m.date).toLocaleDateString()}</span>
            <strong>{m.result}</strong>
          </li>
        ))}
      </ul>
    </div>
  );
};
