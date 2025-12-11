import type { SimpleConteudoResponse } from "@/types/conteudo.types";
import { ConteudoCard } from "@/components/conteudo/ConteudoCard";

interface ConteudoListProps {
  conteudos: SimpleConteudoResponse[];
}

export function ConteudoList({ conteudos }: ConteudoListProps) {
  if (conteudos.length === 0) {
    return (
      <div className="text-center py-12">
        <p className="text-secondary text-lg">
          Nenhum conteúdo encontrado com os filtros aplicados.
        </p>
      </div>
    );
  }

  return (
    <div className="space-y-4">
      {conteudos.map((conteudo) => (
        <ConteudoCard key={conteudo.id} conteudo={conteudo} />
      ))}
    </div>
  );
}
