import React from "react";

interface Props {
  onStartMatch: () => void;
}

export const StartMatchButton: React.FC<Props> = ({ onStartMatch }) => {
  return (
    <button style={{ padding: '8px 16px', fontSize: 16 }} onClick={onStartMatch}>
      Inizia partita
    </button>
  );
};