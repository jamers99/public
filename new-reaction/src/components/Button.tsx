interface Props {
  children: string;
  style?: "primary" | "success" | "cancel";
  onClick?: () => void;
}

export const Button = ({ children, style = "primary", onClick }: Props) => {
  return (
    <button type="button" onClick={onClick} className={"btn btn-" + style}>
      {children}
    </button>
  );
};
