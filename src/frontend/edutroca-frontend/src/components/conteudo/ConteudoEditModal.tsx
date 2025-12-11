import { useState, useRef, useEffect } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import type { SimpleConteudoResponse } from "@/types/conteudo.types";
import { EConteudoTipo } from "@/types/conteudo.types";
import {
  useUpdateVideo,
  useUpdatePergunta,
  useConteudo,
} from "@/hooks/useConteudos";
import { useCategorias } from "@/hooks/useCategorias";
import { Card } from "@/components/common/Card";
import { Button } from "@/components/common/Button";
import { Input } from "@/components/common/Input";
import { Spinner } from "@/components/common/Spinner";
import { X } from "lucide-react";

interface ConteudoEditModalProps {
  conteudo: SimpleConteudoResponse;
  onClose: () => void;
}

const videoSchema = z.object({
  titulo: z.string().min(3, "Título deve ter no mínimo 3 caracteres"),
  descricao: z.string().min(10, "Descrição deve ter no mínimo 10 caracteres"),
});

const perguntaSchema = z.object({
  titulo: z.string().min(3, "Título deve ter no mínimo 3 caracteres"),
  descricao: z.string().min(10, "Descrição deve ter no mínimo 10 caracteres"),
  texto: z.string().min(20, "Pergunta deve ter no mínimo 20 caracteres"),
});

type VideoFormData = z.infer<typeof videoSchema>;
type PerguntaFormData = z.infer<typeof perguntaSchema>;

export function ConteudoEditModal({
  conteudo,
  onClose,
}: ConteudoEditModalProps) {
  const [imagemFile, setImagemFile] = useState<File | null>(null);
  const [apiError, setApiError] = useState("");

  const [categoriaSearch, setCategoriaSearch] = useState("");
  const [selectedCategoria, setSelectedCategoria] = useState<{
    id: string;
    nome: string;
  }>({ id: conteudo.categoria.id, nome: conteudo.categoria.nome });
  const [showCategoriaDropdown, setShowCategoriaDropdown] = useState(false);
  const categoriaDropdownRef = useRef<HTMLDivElement>(null);

  const { data: conteudoCompleto, isLoading: isLoadingConteudo } = useConteudo(
    conteudo.id
  );

  const { data: categorias, isLoading: isLoadingCategorias } = useCategorias(
    { nome: categoriaSearch, pageSize: 10 },
    { enabled: categoriaSearch.length > 0 }
  );

  const updateVideo = useUpdateVideo();
  const updatePergunta = useUpdatePergunta();

  const isVideo = conteudo.tipo === EConteudoTipo.Video;

  const {
    register: registerVideo,
    handleSubmit: handleSubmitVideo,
    formState: { errors: errorsVideo },
  } = useForm<VideoFormData>({
    resolver: zodResolver(videoSchema),
    defaultValues: {
      titulo: conteudo.titulo,
      descricao: conteudo.descricao,
    },
  });

  const {
    register: registerPergunta,
    handleSubmit: handleSubmitPergunta,
    formState: { errors: errorsPergunta },
  } = useForm<PerguntaFormData>({
    resolver: zodResolver(perguntaSchema),
    defaultValues: {
      titulo: conteudo.titulo,
      descricao: conteudo.descricao,
      texto: conteudoCompleto?.textoCompleto || "",
    },
  });

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

  const handleImagemChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setImagemFile(file);
    }
  };

  const onSubmitVideo = async (data: VideoFormData) => {
    try {
      setApiError("");

      await updateVideo.mutateAsync({
        id: conteudo.id,
        data: {
          titulo: data.titulo,
          descricao: data.descricao,
          categoriaId: selectedCategoria.id,
          imagem: imagemFile || undefined,
        },
      });

      onClose();
    } catch (error: any) {
      const message =
        error?.response?.data?.detail ||
        error?.response?.data?.title ||
        "Erro ao atualizar conteúdo. Tente novamente.";
      setApiError(message);
    }
  };

  const onSubmitPergunta = async (data: PerguntaFormData) => {
    try {
      setApiError("");

      await updatePergunta.mutateAsync({
        id: conteudo.id,
        data: {
          titulo: data.titulo,
          descricao: data.descricao,
          categoriaId: selectedCategoria.id,
          texto: data.texto,
        },
      });

      onClose();
    } catch (error: any) {
      const message =
        error?.response?.data?.detail ||
        error?.response?.data?.title ||
        "Erro ao atualizar conteúdo. Tente novamente.";
      setApiError(message);
    }
  };

  const isPending = updateVideo.isPending || updatePergunta.isPending;

  if (isLoadingConteudo) {
    return (
      <>
        <div className="fixed inset-0 bg-black/50 z-40" onClick={onClose} />
        <div className="fixed inset-0 flex items-center justify-center z-50 p-4">
          <Card className="w-full max-w-2xl">
            <div className="flex justify-center py-8">
              <Spinner size="lg" />
            </div>
          </Card>
        </div>
      </>
    );
  }

  return (
    <>
      <div className="fixed inset-0 bg-black/50 z-40" onClick={onClose} />

      <div className="fixed inset-0 flex items-center justify-center z-50 p-4 overflow-y-auto">
        <Card className="w-full max-w-2xl my-8">
          <div className="flex justify-between items-center mb-4">
            <h3 className="text-xl font-bold text-black">
              Editar {isVideo ? "Vídeo" : "Pergunta"}
            </h3>
            <button
              onClick={onClose}
              className="p-2 hover:bg-background rounded-lg transition-smooth"
            >
              <X size={20} />
            </button>
          </div>

          {apiError && (
            <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg text-sm mb-4">
              {apiError}
            </div>
          )}

          <form
            onSubmit={
              isVideo
                ? handleSubmitVideo(onSubmitVideo)
                : handleSubmitPergunta(onSubmitPergunta)
            }
            className="space-y-4"
          >
            <Input
              label="Título"
              {...(isVideo
                ? registerVideo("titulo")
                : registerPergunta("titulo"))}
              error={
                (isVideo
                  ? errorsVideo.titulo?.message
                  : errorsPergunta.titulo?.message) as string
              }
              disabled={isPending}
            />

            <div>
              <label className="block text-sm font-medium text-black mb-1">
                Descrição
              </label>
              <textarea
                {...(isVideo
                  ? registerVideo("descricao")
                  : registerPergunta("descricao"))}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary resize-none"
                rows={3}
                disabled={isPending}
              />
              {(isVideo ? errorsVideo.descricao : errorsPergunta.descricao) && (
                <p className="text-sm text-red-500 mt-1">
                  {
                    (isVideo
                      ? errorsVideo.descricao?.message
                      : errorsPergunta.descricao?.message) as string
                  }
                </p>
              )}
            </div>

            <div ref={categoriaDropdownRef}>
              <label className="block text-sm font-medium text-black mb-1">
                Categoria
              </label>
              <div className="flex items-center gap-2 p-2 bg-background rounded-lg mb-2">
                <span className="flex-1">{selectedCategoria.nome}</span>
                <button
                  type="button"
                  onClick={() => {
                    setSelectedCategoria({
                      id: conteudo.categoria.id,
                      nome: conteudo.categoria.nome,
                    });
                    setCategoriaSearch("");
                  }}
                  className="text-primary hover:text-primary-dark text-sm"
                  disabled={isPending}
                >
                  Resetar
                </button>
              </div>

              <div className="relative">
                <Input
                  value={categoriaSearch}
                  onChange={(e) => {
                    setCategoriaSearch(e.target.value);
                    setShowCategoriaDropdown(e.target.value.length > 0);
                  }}
                  placeholder="Buscar outra categoria..."
                  disabled={isPending}
                />
                {showCategoriaDropdown && (
                  <div className="absolute w-full mt-2 bg-white rounded-lg shadow-lg border border-gray-200 max-h-64 overflow-y-auto z-10 animate-fadeIn">
                    {isLoadingCategorias ? (
                      <div className="flex justify-center py-4">
                        <Spinner />
                      </div>
                    ) : categorias && categorias.items.length > 0 ? (
                      <div className="py-2">
                        {categorias.items.map((categoria) => (
                          <button
                            key={categoria.id}
                            type="button"
                            onClick={() => {
                              setSelectedCategoria({
                                id: categoria.id,
                                nome: categoria.nome,
                              });
                              setCategoriaSearch("");
                              setShowCategoriaDropdown(false);
                            }}
                            className="w-full text-left px-4 py-2 hover:bg-background transition-smooth"
                          >
                            {categoria.nome}
                          </button>
                        ))}
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

            {isVideo ? (
              <div>
                <label className="block text-sm font-medium text-black mb-1">
                  Nova Imagem (thumbnail) - Opcional
                </label>
                <input
                  type="file"
                  accept="image/*"
                  onChange={handleImagemChange}
                  className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary"
                  disabled={isPending}
                />
                {imagemFile && (
                  <p className="text-sm text-secondary mt-1">
                    Novo arquivo selecionado: {imagemFile.name}
                  </p>
                )}
                <p className="text-sm text-secondary mt-1">
                  Deixe em branco para manter a imagem atual
                </p>
              </div>
            ) : (
              <div>
                <label className="block text-sm font-medium text-black mb-1">
                  Pergunta completa
                </label>
                <textarea
                  {...registerPergunta("texto")}
                  className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary resize-none"
                  rows={5}
                  disabled={isPending}
                />
                {errorsPergunta.texto && (
                  <p className="text-sm text-red-500 mt-1">
                    {errorsPergunta.texto.message as string}
                  </p>
                )}
              </div>
            )}

            <div className="flex gap-4">
              <Button type="submit" disabled={isPending} className="flex-1">
                {isPending ? "Salvando..." : "Salvar Alterações"}
              </Button>
              <Button
                type="button"
                variant="secondary"
                onClick={onClose}
                disabled={isPending}
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
