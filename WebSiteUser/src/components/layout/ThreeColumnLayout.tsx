import React from "react";
import "../../App.css"; 
interface Props {
  left?: React.ReactNode;
  center: React.ReactNode;
  right?: React.ReactNode;
}

export const ThreeColumnLayout: React.FC<Props> = ({ left, center, right }) => {
  return (
    <div className="three-column-layout">
      <div className="three-column-grid">
        <aside className="left-col">{left}</aside>
        <main>{center}</main>
        <aside className="right-col">{right}</aside>
      </div>
    </div>
  );
};
