import { useState, useRef, useEffect } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { EConteudoTipo } from "@/types/conteudo.types";
import { useCreateVideo, useCreatePergunta } from "@/hooks/useConteudos";
import { useCategorias } from "@/hooks/useCategorias";
import { Button } from "@/components/common/Button";
import { Input } from "@/components/common/Input";
import { Card } from "@/components/common/Card";
import { Spinner } from "@/components/common/Spinner";
import { ChevronDown, ChevronUp } from "lucide-react";

interface ConteudoFormProps {
  tipo: EConteudoTipo;
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

export function ConteudoForm({ tipo }: ConteudoFormProps) {
  const [isExpanded, setIsExpanded] = useState(false);
  const [videoFile, setVideoFile] = useState<File | null>(null);
  const [imagemFile, setImagemFile] = useState<File | null>(null);
  const [apiError, setApiError] = useState("");

  const [categoriaSearch, setCategoriaSearch] = useState("");
  const [selectedCategoria, setSelectedCategoria] = useState<{
    id: string;
    nome: string;
  } | null>(null);
  const [showCategoriaDropdown, setShowCategoriaDropdown] = useState(false);
  const categoriaDropdownRef = useRef<HTMLDivElement>(null);

  const { data: categorias, isLoading: isLoadingCategorias } = useCategorias(
    { nome: categoriaSearch, pageSize: 10 },
    { enabled: categoriaSearch.length > 0 }
  );

  const createVideo = useCreateVideo();
  const createPergunta = useCreatePergunta();

  const isVideo = tipo === EConteudoTipo.Video;

  const {
    register: registerVideo,
    handleSubmit: handleSubmitVideo,
    reset: resetVideo,
    formState: { errors: errorsVideo },
  } = useForm<VideoFormData>({
    resolver: zodResolver(videoSchema),
  });

  const {
    register: registerPergunta,
    handleSubmit: handleSubmitPergunta,
    reset: resetPergunta,
    formState: { errors: errorsPergunta },
  } = useForm<PerguntaFormData>({
    resolver: zodResolver(perguntaSchema),
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

  const handleVideoChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setVideoFile(file);
    }
  };

  const handleImagemChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setImagemFile(file);
    }
  };

  const resetForm = () => {
    if (isVideo) {
      resetVideo();
    } else {
      resetPergunta();
    }
    setVideoFile(null);
    setImagemFile(null);
    setSelectedCategoria(null);
    setCategoriaSearch("");
    setIsExpanded(false);
  };

  const onSubmitVideo = async (data: VideoFormData) => {
    try {
      setApiError("");

      if (!videoFile) {
        setApiError("Vídeo é obrigatório");
        return;
      }

      if (!imagemFile) {
        setApiError("Imagem (thumbnail) é obrigatória");
        return;
      }

      if (!selectedCategoria) {
        setApiError("Categoria é obrigatória");
        return;
      }

      await createVideo.mutateAsync({
        titulo: data.titulo,
        descricao: data.descricao,
        categoriaId: selectedCategoria.id,
        video: videoFile,
        imagem: imagemFile,
      });

      resetForm();
    } catch (error: any) {
      const message =
        error?.response?.data?.detail ||
        error?.response?.data?.title ||
        "Erro ao publicar conteúdo. Tente novamente.";
      setApiError(message);
    }
  };

  const onSubmitPergunta = async (data: PerguntaFormData) => {
    try {
      setApiError("");

      if (!selectedCategoria) {
        setApiError("Categoria é obrigatória");
        return;
      }

      await createPergunta.mutateAsync({
        titulo: data.titulo,
        descricao: data.descricao,
        categoriaId: selectedCategoria.id,
        texto: data.texto,
      });

      resetForm();
    } catch (error: any) {
      const message =
        error?.response?.data?.detail ||
        error?.response?.data?.title ||
        "Erro ao publicar conteúdo. Tente novamente.";
      setApiError(message);
    }
  };

  const isPending = createVideo.isPending || createPergunta.isPending;

  return (
    <Card className="mb-6">
      <button
        onClick={() => setIsExpanded(!isExpanded)}
        className="w-full flex items-center justify-between hover:opacity-80 transition-smooth"
      >
        <h2 className="text-xl font-bold text-black">
          Publicar {isVideo ? "Vídeo" : "Pergunta"}
        </h2>
        {isExpanded ? <ChevronUp size={24} /> : <ChevronDown size={24} />}
      </button>

      {isExpanded && (
        <div className="mt-4 animate-slideDown">
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
                Categoria *
              </label>
              {selectedCategoria ? (
                <div className="flex items-center gap-2 p-2 bg-background rounded-lg">
                  <span className="flex-1">{selectedCategoria.nome}</span>
                  <button
                    type="button"
                    onClick={() => setSelectedCategoria(null)}
                    className="text-primary hover:text-primary-dark"
                    disabled={isPending}
                  >
                    ✕
                  </button>
                </div>
              ) : (
                <div className="relative">
                  <Input
                    value={categoriaSearch}
                    onChange={(e) => {
                      setCategoriaSearch(e.target.value);
                      setShowCategoriaDropdown(e.target.value.length > 0);
                    }}
                    placeholder="Buscar categoria..."
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
              )}
            </div>

            {isVideo ? (
              <>
                <div>
                  <label className="block text-sm font-medium text-black mb-1">
                    Vídeo *
                  </label>
                  <input
                    type="file"
                    accept="video/*"
                    onChange={handleVideoChange}
                    className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary"
                    disabled={isPending}
                  />
                  {videoFile && (
                    <p className="text-sm text-secondary mt-1">
                      Arquivo selecionado: {videoFile.name}
                    </p>
                  )}
                </div>

                <div>
                  <label className="block text-sm font-medium text-black mb-1">
                    Imagem (thumbnail) *
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
                      Arquivo selecionado: {imagemFile.name}
                    </p>
                  )}
                </div>
              </>
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

            <div className="flex gap-2">
              <Button type="submit" disabled={isPending} className="flex-1">
                {isPending ? "Publicando..." : "Publicar"}
              </Button>
              <Button
                type="button"
                variant="secondary"
                onClick={resetForm}
                disabled={isPending}
              >
                Cancelar
              </Button>
            </div>
          </form>
        </div>
      )}
    </Card>
  );
}
