import React from "react";

export const Card: React.FC<React.PropsWithChildren<{ className?: string }>> = ({ children, className }) => {
  return (
    <div className={className} style={{ padding: 12, border: '1px solid #eee', borderRadius: 8 }}>
      {children}
    </div>
  );
};