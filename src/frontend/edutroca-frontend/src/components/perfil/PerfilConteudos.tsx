import { useState } from "react";
import type { SimpleConteudoResponse } from "@/types/conteudo.types";
import { EConteudoTipo } from "@/types/conteudo.types";
import { ConteudoCard } from "@/components/conteudo/ConteudoCard";
import { ConteudoEditModal } from "@/components/conteudo/ConteudoEditModal";
import { useDeleteVideo, useDeletePergunta } from "@/hooks/useConteudos";
import { useAuth } from "@/hooks/useAuth";
import { Spinner } from "@/components/common/Spinner";
import { Button } from "@/components/common/Button";
import { Pencil, Trash2 } from "lucide-react";

interface PerfilConteudosProps {
  conteudos: SimpleConteudoResponse[];
  isLoading: boolean;
  tipoAtual: EConteudoTipo;
  isOwnProfile?: boolean;
}

export function PerfilConteudos({
  conteudos,
  isLoading,
  tipoAtual,
  isOwnProfile,
}: PerfilConteudosProps) {
  const [editingConteudo, setEditingConteudo] =
    useState<SimpleConteudoResponse | null>(null);
  const [deletingId, setDeletingId] = useState<string | null>(null);
  const deleteVideo = useDeleteVideo();
  const deletePergunta = useDeletePergunta();
  const { currentUser } = useAuth();

  const handleDelete = async (conteudo: SimpleConteudoResponse) => {
    if (!confirm(`Tem certeza que deseja excluir "${conteudo.titulo}"?`)) {
      return;
    }

    try {
      setDeletingId(conteudo.id);
      if (conteudo.tipo === EConteudoTipo.Video) {
        await deleteVideo.mutateAsync(conteudo.id);
      } else {
        await deletePergunta.mutateAsync(conteudo.id);
      }
    } catch (error) {
      alert("Erro ao excluir conteúdo. Tente novamente.");
    } finally {
      setDeletingId(null);
    }
  };

  if (isLoading) {
    return (
      <div className="flex justify-center py-12">
        <Spinner />
      </div>
    );
  }

  if (conteudos.length === 0) {
    return (
      <div className="text-center py-12">
        <p className="text-secondary">
          Nenhum {tipoAtual === EConteudoTipo.Video ? "vídeo" : "pergunta"}{" "}
          publicado ainda.
        </p>
      </div>
    );
  }

  return (
    <>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {conteudos.map((conteudo) => (
          <div key={conteudo.id} className="relative">
            <ConteudoCard conteudo={conteudo} />

            {isOwnProfile && currentUser?.id === conteudo.autor.id && (
              <div className="absolute top-2 right-2 flex gap-2">
                <button
                  onClick={() => setEditingConteudo(conteudo)}
                  className="p-2 bg-white rounded-lg shadow-md hover:bg-background transition-smooth"
                  title="Editar"
                  disabled={deletingId === conteudo.id}
                >
                  <Pencil size={16} className="text-black" />
                </button>
                <button
                  onClick={() => handleDelete(conteudo)}
                  className="p-2 bg-white rounded-lg shadow-md hover:bg-red-50 transition-smooth"
                  title="Excluir"
                  disabled={deletingId === conteudo.id}
                >
                  {deletingId === conteudo.id ? (
                    <Spinner size="sm" />
                  ) : (
                    <Trash2 size={16} className="text-primary" />
                  )}
                </button>
              </div>
            )}
          </div>
        ))}
      </div>

      {editingConteudo && (
        <ConteudoEditModal
          conteudo={editingConteudo}
          onClose={() => setEditingConteudo(null)}
        />
      )}
    </>
  );
}
