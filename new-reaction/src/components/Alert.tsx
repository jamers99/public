import { ReactNode } from "react";

interface Props {
  children: ReactNode;
  onDismiss: () => void;
}

const Alert = ({ children, onDismiss }: Props) => {
  if (!children) return null;
  return (
    <div
      className="alert alert-warning alert-dismissible fade show"
      role="alert"
    >
      {children}
      <button
        type="button"
        className="btn-close"
        data-bs-dismiss="alert"
        aria-label="Close"
        onClick={onDismiss}
      ></button>
    </div>
  );
};

export default Alert;
