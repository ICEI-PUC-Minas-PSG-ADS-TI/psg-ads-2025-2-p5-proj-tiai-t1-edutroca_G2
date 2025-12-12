import type { ButtonHTMLAttributes, ReactNode } from "react";

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: "primary" | "secondary" | "danger";
  fullWidth?: boolean;
  children: ReactNode;
}

export function Button({
  variant = "primary",
  fullWidth = false,
  children,
  className = "",
  ...props
}: ButtonProps) {
  const baseStyles =
    "rounded-lg font-medium transition-smooth disabled:opacity-50 disabled:cursor-not-allowed px-4 py-2 cursor-pointer";

  const variants = {
    primary: "bg-primary text-white hover:bg-primary-dark",
    secondary: "bg-inactive text-white hover:bg-inactive-dark",
    danger: "bg-red-600 text-white hover:bg-red-700",
  };

  return (
    <button
      className={`
        ${baseStyles}
        ${variants[variant]}
        ${fullWidth ? "w-full" : ""}
        ${className}
      `}
      {...props}
    >
      {children}
    </button>
  );
}
