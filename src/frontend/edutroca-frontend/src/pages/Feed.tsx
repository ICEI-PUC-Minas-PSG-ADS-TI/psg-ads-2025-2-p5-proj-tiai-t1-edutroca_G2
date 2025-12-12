import { useState } from "react";
import { useSearchParams } from "react-router-dom";
import { useConteudos } from "@/hooks/useConteudos";
import type { ConteudoFilters } from "@/types/conteudo.types";
import { EConteudoTipo, EConteudoOrderBy } from "@/types/conteudo.types";
import { ConteudoForm } from "@/components/conteudo/ConteudoForm";
import { ConteudoList } from "@/components/conteudo/ConteudoList";
import { FeedFilters } from "@/components/feed/FeedFilters";
import { Button } from "@/components/common/Button";
import { Spinner } from "@/components/common/Spinner";

export function Feed() {
  const [searchParams] = useSearchParams();
  const [page, setPage] = useState(1);

  const categoriaIdFromUrl = searchParams.get("categoria");
  const tipoFromUrl = searchParams.get("tipo") as EConteudoTipo | null;
  const [tipoConteudo, setTipoConteudo] = useState<EConteudoTipo>(
    tipoFromUrl || EConteudoTipo.Video
  );

  const [filters, setFilters] = useState<ConteudoFilters>({
    tipo: tipoConteudo,
    categoriasIds: categoriaIdFromUrl ? [categoriaIdFromUrl] : undefined,
    orderBy: EConteudoOrderBy.MaisRecente,
    pageNumber: page,
    pageSize: 10,
  });

  const { data, isLoading } = useConteudos(filters);

  const handleTipoChange = (novoTipo: EConteudoTipo) => {
    setTipoConteudo(novoTipo);
    setFilters((prev) => ({ ...prev, tipo: novoTipo, pageNumber: 1 }));
    setPage(1);
  };

  const handleFiltersChange = (newFilters: Partial<ConteudoFilters>) => {
    setFilters((prev) => ({ ...prev, ...newFilters, pageNumber: 1 }));
    setPage(1);
  };

  const handlePageChange = (newPage: number) => {
    setPage(newPage);
    setFilters((prev) => ({ ...prev, pageNumber: newPage }));
    window.scrollTo({ top: 0, behavior: "smooth" });
  };

  return (
    <div className="flex gap-6">
      <aside className="w-80 shrink-0 hidden lg:block">
        <FeedFilters filters={filters} onFiltersChange={handleFiltersChange} />
      </aside>

      <main className="flex-1 max-w-4xl">
        <div className="flex gap-2 mb-6">
          <Button
            variant={
              tipoConteudo === EConteudoTipo.Video ? "primary" : "secondary"
            }
            onClick={() => handleTipoChange(EConteudoTipo.Video)}
          >
            Vídeos
          </Button>
          <Button
            variant={
              tipoConteudo === EConteudoTipo.Pergunta ? "primary" : "secondary"
            }
            onClick={() => handleTipoChange(EConteudoTipo.Pergunta)}
          >
            Perguntas
          </Button>
        </div>

        <ConteudoForm tipo={tipoConteudo} />

        {isLoading ? (
          <div className="flex justify-center py-12">
            <Spinner size="lg" />
          </div>
        ) : (
          <>
            <ConteudoList conteudos={data?.items || []} />

            {data && data.totalPages > 1 && (
              <div className="flex justify-center items-center gap-4 mt-8">
                <Button
                  variant="secondary"
                  disabled={!data.hasPreviousPage}
                  onClick={() => handlePageChange(page - 1)}
                >
                  Anterior
                </Button>
                <span className="text-secondary">
                  Página {page} de {data.totalPages}
                </span>
                <Button
                  variant="secondary"
                  disabled={!data.hasNextPage}
                  onClick={() => handlePageChange(page + 1)}
                >
                  Próxima
                </Button>
              </div>
            )}
          </>
        )}
      </main>
    </div>
  );
}
