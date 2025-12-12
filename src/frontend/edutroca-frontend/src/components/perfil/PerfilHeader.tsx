import type { CompleteUsuarioResponse } from "@/types/usuario.types";
import { ENivel } from "@/types/usuario.types";
import { Avatar } from "@/components/common/Avatar";
import { Card } from "@/components/common/Card";

interface PerfilHeaderProps {
  usuario: CompleteUsuarioResponse;
}

const nivelLabels: Record<ENivel, string> = {
  [ENivel.Usuario]: "Usuário",
  [ENivel.Criador]: "Criador",
  [ENivel.CriadorPleno]: "Criador Pleno",
  [ENivel.CriadorCertificado]: "Criador Certificado",
};

export function PerfilHeader({ usuario }: PerfilHeaderProps) {
  return (
    <Card className="mb-6">
      <div className="flex flex-col items-center text-center gap-4">
        <Avatar src={usuario.caminhoImagem} alt={usuario.nome} size="lg" />

        <span className="px-3 py-1 bg-primary text-white rounded-full text-sm font-medium">
          {nivelLabels[usuario.nivel]}
        </span>

        <h1 className="text-3xl font-bold text-black">{usuario.nome}</h1>

        {usuario.bio && (
          <p className="text-secondary max-w-2xl">{usuario.bio}</p>
        )}

        {usuario.categoriasDeInteresse &&
          usuario.categoriasDeInteresse.length > 0 && (
            <div className="w-full">
              <h3 className="text-sm font-semibold text-black mb-2">
                Interesses:
              </h3>
              <div className="flex flex-wrap gap-2 justify-center">
                {usuario.categoriasDeInteresse.map((categoria) => (
                  <span
                    key={categoria.id}
                    className="px-3 py-1 bg-background text-black rounded-lg text-sm"
                  >
                    {categoria.nome}
                  </span>
                ))}
              </div>
            </div>
          )}
      </div>
    </Card>
  );
}
