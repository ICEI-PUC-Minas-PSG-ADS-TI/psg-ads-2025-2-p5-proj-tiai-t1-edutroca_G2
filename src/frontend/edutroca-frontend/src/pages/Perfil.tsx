import { useState } from "react";
import { useParams } from "react-router-dom";
import { useAuth } from "@/hooks/useAuth";
import { useUsuario } from "@/hooks/useUsuario";
import { useConteudos } from "@/hooks/useConteudos";
import { PerfilHeader } from "@/components/perfil/PerfilHeader";
import { PerfilConteudos } from "@/components/perfil/PerfilConteudos";
import { PerfilEdit } from "@/components/perfil/PerfilEdit";
import { Spinner } from "@/components/common/Spinner";
import { Card } from "@/components/common/Card";
import { Button } from "@/components/common/Button";
import { EConteudoTipo } from "@/types/conteudo.types";

export function Perfil() {
  const { id } = useParams<{ id: string }>();
  const { currentUser } = useAuth();
  const [isEditing, setIsEditing] = useState(false);
  const [tipoConteudo, setTipoConteudo] = useState<EConteudoTipo>(
    EConteudoTipo.Video
  );

  const { data: usuario, isLoading: isLoadingUser } = useUsuario(id!);
  const { data: conteudos, isLoading: isLoadingConteudos } = useConteudos({
    autorId: id,
    tipo: tipoConteudo,
    pageSize: 20,
  });

  const isOwnProfile = currentUser?.id === id;

  if (isLoadingUser) {
    return (
      <div className="flex justify-center items-center min-h-[400px]">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!usuario) {
    return (
      <Card>
        <h2 className="text-xl font-bold mb-4">Usuário não encontrado</h2>
        <p className="text-secondary">
          O perfil que você está procurando não existe.
        </p>
      </Card>
    );
  }

  if (isEditing) {
    return (
      <div className="max-w-4xl mx-auto">
        <PerfilEdit
          usuario={usuario}
          onCancel={() => setIsEditing(false)}
          onSuccess={() => setIsEditing(false)}
        />
      </div>
    );
  }

  return (
    <div className="max-w-4xl mx-auto">
      <PerfilHeader usuario={usuario} />

      {isOwnProfile && (
        <div className="mb-6">
          <Button onClick={() => setIsEditing(true)} fullWidth>
            Editar Perfil
          </Button>
        </div>
      )}

      <Card>
        <div className="mb-6">
          <h2 className="text-2xl font-bold text-black mb-4">Publicações</h2>

          <div className="flex gap-2">
            <Button
              variant={
                tipoConteudo === EConteudoTipo.Video ? "primary" : "secondary"
              }
              onClick={() => setTipoConteudo(EConteudoTipo.Video)}
            >
              Vídeos
            </Button>
            <Button
              variant={
                tipoConteudo === EConteudoTipo.Pergunta
                  ? "primary"
                  : "secondary"
              }
              onClick={() => setTipoConteudo(EConteudoTipo.Pergunta)}
            >
              Perguntas
            </Button>
          </div>
        </div>

        <PerfilConteudos
          conteudos={conteudos?.items || []}
          isLoading={isLoadingConteudos}
          tipoAtual={tipoConteudo}
          isOwnProfile={isOwnProfile}
        />
      </Card>
    </div>
  );
}
