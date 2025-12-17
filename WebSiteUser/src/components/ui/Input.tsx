import React from "react";

interface Props extends React.InputHTMLAttributes<HTMLInputElement> {}

export const Input: React.FC<Props> = (props) => {
  return <input {...props} style={{ padding: 8, borderRadius: 6, border: '1px solid #ccc' }} />;
};