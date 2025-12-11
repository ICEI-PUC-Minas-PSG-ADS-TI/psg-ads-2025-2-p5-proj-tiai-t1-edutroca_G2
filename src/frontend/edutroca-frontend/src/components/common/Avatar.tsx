interface AvatarProps {
  src?: string | null;
  alt: string;
  size?: "sm" | "md" | "lg";
  className?: string;
}

export function Avatar({ src, alt, size = "md", className = "" }: AvatarProps) {
  let caminhoImage = null;
  if (src) {
    if (src.startsWith("data")) {
      caminhoImage = src;
    } else {
      caminhoImage = import.meta.env.VITE_FILES_URL + src;
    }
  }
  const sizes = {
    sm: "w-8 h-8 text-xs",
    md: "w-10 h-10 text-sm",
    lg: "w-24 h-24 text-2xl",
  };
  const getInitials = (name: string) => {
    return name
      .split(" ")
      .map((n) => n[0])
      .join("")
      .toUpperCase()
      .slice(0, 2);
  };

  return (
    <div
      className={`
        ${sizes[size]} 
        rounded-full overflow-hidden 
        bg-inactive flex items-center justify-center
        ${className}
      `}
    >
      {caminhoImage ? (
        <img
          src={caminhoImage}
          alt={alt}
          className="w-full h-full object-cover"
        />
      ) : (
        <span className="text-white font-semibold">{getInitials(alt)}</span>
      )}
    </div>
  );
}
