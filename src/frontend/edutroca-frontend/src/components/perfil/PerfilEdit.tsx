import { useState, useRef, useEffect } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import type { CompleteUsuarioResponse } from "@/types/usuario.types";
import {
  useUpdateUsuario,
  useUpdateEmail,
  useSetInterests,
} from "@/hooks/useUsuario";
import { useCategorias } from "@/hooks/useCategorias";
import { Card } from "@/components/common/Card";
import { Button } from "@/components/common/Button";
import { Input } from "@/components/common/Input";
import { Avatar } from "@/components/common/Avatar";
import { Spinner } from "@/components/common/Spinner";
import { X } from "lucide-react";

interface PerfilEditProps {
  usuario: CompleteUsuarioResponse;
  onCancel: () => void;
  onSuccess: () => void;
}

const perfilSchema = z.object({
  nome: z.string().min(3, "Nome deve ter no mínimo 3 caracteres"),
  bio: z.string().optional(),
});

type PerfilFormData = z.infer<typeof perfilSchema>;

export function PerfilEdit({ usuario, onCancel, onSuccess }: PerfilEditProps) {
  const [previewImage, setPreviewImage] = useState<string | null>(
    usuario.caminhoImagem
  );
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [shouldRemoveImage, setShouldRemoveImage] = useState(false);
  const [showEmailModal, setShowEmailModal] = useState(false);
  const [apiError, setApiError] = useState("");

  const updateUsuario = useUpdateUsuario();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<PerfilFormData>({
    resolver: zodResolver(perfilSchema),
    defaultValues: {
      nome: usuario.nome,
      bio: usuario.bio || "",
    },
  });

  const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setSelectedFile(file);
      const reader = new FileReader();
      reader.onloadend = () => {
        setPreviewImage(reader.result as string);
        setShouldRemoveImage(false);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleRemoveImage = () => {
    setPreviewImage(null);
    setSelectedFile(null);
    setShouldRemoveImage(true);
  };

  const onSubmit = async (data: PerfilFormData) => {
    try {
      setApiError("");

      await updateUsuario.mutateAsync({
        id: usuario.id,
        data: {
          nome: data.nome,
          bio: data.bio || undefined,
          profilePicture: selectedFile || undefined,
          removePicture: shouldRemoveImage,
        },
      });

      onSuccess();
    } catch (error: any) {
      const message =
        error?.response?.data?.detail ||
        error?.response?.data?.title ||
        "Erro ao atualizar perfil. Tente novamente.";
      setApiError(message);
    }
  };

  return (
    <>
      <Card>
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-2xl font-bold text-black">Editar Perfil</h2>
          <button
            onClick={onCancel}
            className="p-2 hover:bg-background rounded-lg transition-smooth"
          >
            <X size={24} />
          </button>
        </div>

        {apiError && (
          <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg text-sm mb-4">
            {apiError}
          </div>
        )}

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
          <div className="flex flex-col items-center gap-4">
            <Avatar src={previewImage} alt={usuario.nome} size="lg" />

            <div className="flex gap-2">
              <label className="cursor-pointer">
                <input
                  type="file"
                  accept="image/*"
                  className="hidden"
                  onChange={handleImageChange}
                />
                <span className="inline-block px-4 py-2 bg-primary text-white rounded-lg font-medium hover:bg-primary-dark transition-smooth">
                  Alterar Foto
                </span>
              </label>

              {previewImage && (
                <Button
                  type="button"
                  variant="secondary"
                  onClick={handleRemoveImage}
                >
                  Remover Foto
                </Button>
              )}
            </div>
          </div>

          <Input
            label="Nome"
            {...register("nome")}
            error={errors.nome?.message}
            disabled={updateUsuario.isPending}
          />

          <div>
            <label className="block text-sm font-medium text-black mb-1">
              Bio
            </label>
            <textarea
              {...register("bio")}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary resize-none"
              rows={4}
              placeholder="Conte um pouco sobre você..."
              disabled={updateUsuario.isPending}
            />
            {errors.bio && (
              <p className="text-sm text-red-500 mt-1">{errors.bio.message}</p>
            )}
          </div>

          <AreasInteresse
            usuario={usuario}
            disabled={updateUsuario.isPending}
          />

          <div className="flex gap-4">
            <Button
              type="submit"
              disabled={updateUsuario.isPending}
              className="flex-1"
            >
              {updateUsuario.isPending ? "Salvando..." : "Salvar Alterações"}
            </Button>
            <Button
              type="button"
              variant="secondary"
              onClick={onCancel}
              disabled={updateUsuario.isPending}
            >
              Cancelar
            </Button>
          </div>
        </form>

        <div className="mt-8 pt-8 border-t border-gray-200">
          <h3 className="text-lg font-bold text-black mb-4">Alterar Email</h3>
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-secondary">Email atual:</p>
              <p className="font-medium">{usuario.email}</p>
            </div>
            <Button onClick={() => setShowEmailModal(true)}>
              Alterar Email
            </Button>
          </div>
        </div>
      </Card>

      {showEmailModal && (
        <EmailChangeModal
          usuarioId={usuario.id}
          onClose={() => setShowEmailModal(false)}
        />
      )}
    </>
  );
}

interface EmailChangeModalProps {
  usuarioId: string;
  onClose: () => void;
}

const emailSchema = z.object({
  novoEmail: z.string().email("Email inválido"),
});

type EmailFormData = z.infer<typeof emailSchema>;

function EmailChangeModal({ usuarioId, onClose }: EmailChangeModalProps) {
  const [apiError, setApiError] = useState("");
  const updateEmail = useUpdateEmail();
  const { logout } = useAuth();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<EmailFormData>({
    resolver: zodResolver(emailSchema),
  });

  const onSubmit = async (data: EmailFormData) => {
    try {
      setApiError("");
      await updateEmail.mutateAsync({
        id: usuarioId,
        novoEmail: data.novoEmail,
      });

      alert("Email alterado com sucesso! Você será desconectado.");
      logout();
    } catch (error: any) {
      const message =
        error?.response?.data?.detail ||
        error?.response?.data?.title ||
        "Erro ao alterar email. Tente novamente.";
      setApiError(message);
    }
  };

  return (
    <>
      <div className="fixed inset-0 bg-black/50 z-40" onClick={onClose} />

      <div className="fixed inset-0 flex items-center justify-center z-50 p-4">
        <Card className="w-full max-w-md">
          <div className="flex justify-between items-center mb-4">
            <h3 className="text-xl font-bold text-black">Alterar Email</h3>
            <button
              onClick={onClose}
              className="p-2 hover:bg-background rounded-lg transition-smooth"
            >
              <X size={20} />
            </button>
          </div>

          <p className="text-secondary text-sm mb-4">
            ⚠️ Ao alterar seu email, você será desconectado e precisará fazer
            login novamente com o novo email.
          </p>

          {apiError && (
            <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg text-sm mb-4">
              {apiError}
            </div>
          )}

          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <Input
              label="Novo Email"
              type="email"
              {...register("novoEmail")}
              error={errors.novoEmail?.message}
              disabled={updateEmail.isPending}
            />

            <div className="flex gap-4">
              <Button
                type="submit"
                disabled={updateEmail.isPending}
                className="flex-1"
              >
                {updateEmail.isPending ? "Alterando..." : "Confirmar"}
              </Button>
              <Button
                type="button"
                variant="secondary"
                onClick={onClose}
                disabled={updateEmail.isPending}
              >
                Cancelar
              </Button>
            </div>
          </form>
        </Card>
      </div>
    </>
  );
}

import { useAuth } from "@/hooks/useAuth";

interface AreasInteresseProps {
  usuario: CompleteUsuarioResponse;
  disabled?: boolean;
}

function AreasInteresse({ usuario, disabled }: AreasInteresseProps) {
  const [categoriaSearch, setCategoriaSearch] = useState("");
  const [showCategoriaDropdown, setShowCategoriaDropdown] = useState(false);
  const [interesses, setInteresses] = useState(
    usuario.categoriasDeInteresse || []
  );
  const categoriaDropdownRef = useRef<HTMLDivElement>(null);

  const { data: categorias, isLoading: isLoadingCategorias } = useCategorias(
    { nome: categoriaSearch, pageSize: 10 },
    { enabled: categoriaSearch.length > 0 }
  );

  const setInterestsMutation = useSetInterests();

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (
        categoriaDropdownRef.current &&
        !categoriaDropdownRef.current.contains(event.target as Node)
      ) {
        setShowCategoriaDropdown(false);
      }
    }

    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const handleAddCategoria = async (categoria: {
    id: string;
    nome: string;
  }) => {
    const jaExiste = interesses.some((c) => c.id === categoria.id);
    if (jaExiste) {
      alert("Esta categoria já está na lista de interesses");
      return;
    }
    const novaListaInteresses = [...interesses, categoria];

    const idsParaEnviar = novaListaInteresses.map((c) => c.id) as [string];

    try {
      await setInterestsMutation.mutateAsync({
        usuarioId: usuario.id,
        categoriasIds: idsParaEnviar,
      });

      setInteresses(novaListaInteresses);
      setCategoriaSearch("");
      setShowCategoriaDropdown(false);
    } catch (error) {
      alert("Erro ao atualizar interesses");
      console.error(error);
    }
  };

  const handleRemoveCategoria = async (categoriaId: string) => {
    const novaListaInteresses = interesses.filter((c) => c.id !== categoriaId);

    const idsParaEnviar = novaListaInteresses.map((c) => c.id) as [string];

    try {
      await setInterestsMutation.mutateAsync({
        usuarioId: usuario.id,
        categoriasIds: idsParaEnviar,
      });

      setInteresses(novaListaInteresses);
    } catch (error) {
      alert("Erro ao remover interesse");
      console.error(error);
    }
  };

  return (
    <div>
      <label className="block text-sm font-medium text-black mb-1">
        Áreas de Interesse
      </label>

      {interesses.length > 0 && (
        <div className="flex flex-wrap gap-2 mb-2">
          {interesses.map((categoria) => (
            <div
              key={categoria.id}
              className="flex items-center gap-2 px-3 py-1 bg-background rounded-lg"
            >
              <span className="text-sm">{categoria.nome}</span>
              <button
                type="button"
                onClick={() => handleRemoveCategoria(categoria.id)}
                className="text-primary hover:text-primary-dark"
                disabled={disabled || setInterestsMutation.isPending}
              >
                ✕
              </button>
            </div>
          ))}
        </div>
      )}

      <div ref={categoriaDropdownRef} className="relative">
        <Input
          value={categoriaSearch}
          onChange={(e) => {
            setCategoriaSearch(e.target.value);
            setShowCategoriaDropdown(e.target.value.length > 0);
          }}
          placeholder="Adicionar categoria de interesse..."
          disabled={disabled || setInterestsMutation.isPending}
        />

        {showCategoriaDropdown && (
          <div className="absolute w-full mt-2 bg-white rounded-lg shadow-lg border border-gray-200 max-h-64 overflow-y-auto z-10 animate-fadeIn">
            {isLoadingCategorias ? (
              <div className="flex justify-center py-4">
                <Spinner />
              </div>
            ) : categorias && categorias.items.length > 0 ? (
              <div className="py-2">
                {categorias.items.map((categoria) => {
                  const jaAdicionada = interesses.some(
                    (c) => c.id === categoria.id
                  );
                  return (
                    <button
                      key={categoria.id}
                      type="button"
                      onClick={() =>
                        !jaAdicionada && handleAddCategoria(categoria)
                      }
                      className={`w-full text-left px-4 py-2 transition-smooth ${
                        jaAdicionada
                          ? "bg-gray-100 text-gray-400 cursor-not-allowed"
                          : "hover:bg-background"
                      }`}
                      disabled={jaAdicionada || setInterestsMutation.isPending}
                    >
                      {categoria.nome} {jaAdicionada && "(já adicionada)"}
                    </button>
                  );
                })}
              </div>
            ) : (
              <div className="py-4 text-center text-secondary text-sm">
                Nenhuma categoria encontrada
              </div>
            )}
          </div>
        )}
      </div>
    </div>
  );
}
