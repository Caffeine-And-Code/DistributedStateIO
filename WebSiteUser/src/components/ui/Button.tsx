import React from "react";

interface Props extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary';
}

export const Button: React.FC<Props> = ({ variant = 'primary', children, ...rest }) => {
  const base = { padding: '8px 12px', borderRadius: 6 } as React.CSSProperties;
  const style = variant === 'primary' ? { ...base, background: '#111', color: '#fff' } : base;
  return (
    <button style={style} {...rest}>
      {children}
    </button>
  );
};