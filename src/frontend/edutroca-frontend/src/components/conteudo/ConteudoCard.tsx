import { Link } from "react-router-dom";
import { ThumbsUp, ThumbsDown, MessageCircle, Eye } from "lucide-react";
import type { SimpleConteudoResponse } from "@/types/conteudo.types";
import { EConteudoTipo } from "@/types/conteudo.types";
import { Avatar } from "@/components/common/Avatar";

interface ConteudoCardProps {
  conteudo: SimpleConteudoResponse;
}

export function ConteudoCard({ conteudo }: ConteudoCardProps) {
  const formatDate = (date: string) => {
    return new Date(date).toLocaleDateString("pt-BR", {
      day: "2-digit",
      month: "short",
      year: "numeric",
    });
  };

  return (
    <Link
      to={`/app/conteudo/${conteudo.id}`}
      className="block bg-white rounded-lg shadow hover:shadow-lg transition-smooth overflow-hidden"
    >
      {conteudo.caminhoImagem && (
        <img
          src={import.meta.env.VITE_FILES_URL + conteudo.caminhoImagem}
          alt={conteudo.titulo}
          className="w-full h-48 object-cover"
        />
      )}

      <div className="p-4">
        <div className="flex items-center gap-2 mb-2">
          <Avatar
            src={conteudo.autor.caminhoImagem}
            alt={conteudo.autor.nome}
            size="sm"
          />
          <div className="text-sm">
            <p className="font-medium">{conteudo.autor.nome}</p>
            <p className="text-secondary">
              {formatDate(conteudo.dataPublicacao)}
            </p>
          </div>
        </div>

        <h3 className="font-semibold text-lg mb-1">{conteudo.titulo}</h3>
        <p className="text-secondary text-sm mb-3 line-clamp-2">
          {conteudo.descricao}
        </p>

        <div className="flex items-center gap-4 text-sm text-secondary">
          <span className="flex items-center gap-1">
            <Eye size={16} />
            {conteudo.visualizacoes}
          </span>
          <span className="flex items-center gap-1">
            <ThumbsUp size={16} />
            {conteudo.likes}
          </span>
          <span className="flex items-center gap-1">
            <ThumbsDown size={16} />
            {conteudo.dislikes}
          </span>
          <span className="flex items-center gap-1">
            <MessageCircle size={16} />
            {conteudo.totalComentarios}
          </span>
          <span className="ml-auto px-2 py-1 bg-background rounded text-xs">
            {conteudo.tipo === EConteudoTipo.Video ? "Vídeo" : "Pergunta"}
          </span>
        </div>

        <div className="mt-2">
          <span className="inline-block px-2 py-1 bg-primary/10 text-primary rounded text-xs">
            {conteudo.categoria.nome}
          </span>
        </div>
      </div>
    </Link>
  );
}
