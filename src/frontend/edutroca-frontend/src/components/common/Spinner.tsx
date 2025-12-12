interface SpinnerProps {
  size?: "sm" | "md" | "lg";
  className?: string;
}

export function Spinner({ size = "md", className = "" }: SpinnerProps) {
  const sizes = {
    sm: "w-4 h-4 border-2",
    md: "w-8 h-8 border-4",
    lg: "w-12 h-12 border-[5px]",
  };

  return (
    <div
      className={`
        inline-block 
        animate-spin 
        rounded-full 
        border-solid 
        
        /* O segredo do visual: */
        border-gray-200     /* 1. Cor do círculo completo (fundo suave) */
        border-t-primary    /* 2. Cor apenas do topo que vai girar */
        
        ${sizes[size]}
        ${className}
      `}
      role="status"
      aria-label="Carregando"
    >
      <span className="sr-only">Carregando...</span>
    </div>
  );
}
