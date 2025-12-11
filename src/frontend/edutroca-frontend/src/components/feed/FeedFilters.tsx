import { useState, useEffect, useRef } from "react";
import type { ConteudoFilters } from "@/types/conteudo.types";
import { EConteudoOrderBy } from "@/types/conteudo.types";
import { ENivel } from "@/types/usuario.types";
import { useCategorias } from "@/hooks/useCategorias";
import { useUsuarios } from "@/hooks/useUsuario";
import { Card } from "@/components/common/Card";
import { Input } from "@/components/common/Input";
import { Button } from "@/components/common/Button";
import { Avatar } from "@/components/common/Avatar";
import { Spinner } from "@/components/common/Spinner";

interface FeedFiltersProps {
  filters: ConteudoFilters;
  onFiltersChange: (filters: Partial<ConteudoFilters>) => void;
}

const nivelOptions = [
  { value: "", label: "Todos" },
  { value: ENivel.Usuario, label: "Usuário" },
  { value: ENivel.Criador, label: "Criador" },
  { value: ENivel.CriadorPleno, label: "Criador Pleno" },
  { value: ENivel.CriadorCertificado, label: "Criador Certificado" },
];

const orderByOptions = [
  { value: EConteudoOrderBy.MaisRecente, label: "Mais Recente" },
  { value: EConteudoOrderBy.MaisAntigo, label: "Mais Antigo" },
  { value: EConteudoOrderBy.MaisVisualizacoes, label: "Mais Visualizações" },
  { value: EConteudoOrderBy.MenosVisualizacoes, label: "Menos Visualizações" },
  { value: EConteudoOrderBy.MaisLikes, label: "Mais Likes" },
  { value: EConteudoOrderBy.MenosLikes, label: "Menos Likes" },
  { value: EConteudoOrderBy.MaiorReputacao, label: "Maior Reputação" },
  { value: EConteudoOrderBy.MenorReputacao, label: "Menor Reputação" },
];

export function FeedFilters({ filters, onFiltersChange }: FeedFiltersProps) {
  const [localFilters, setLocalFilters] = useState({
    titulo: filters.titulo || "",
    visualizacoesMin: filters.visualizacoesMin?.toString() || "",
    visualizacoesMax: filters.visualizacoesMax?.toString() || "",
    likesMin: filters.likesMin?.toString() || "",
    likesMax: filters.likesMax?.toString() || "",
    categoriasIds: filters.categoriasIds || [],
    periodoFrom: filters.periodoFrom || "",
    periodoTo: filters.periodoTo || "",
    nivelUsuario: filters.nivelUsuario || "",
    orderBy: filters.orderBy || EConteudoOrderBy.MaisRecente,
  });

  const [autorSearch, setAutorSearch] = useState("");
  const [selectedAutor, setSelectedAutor] = useState<{
    id: string;
    nome: string;
    imagem: string | null;
  } | null>(null);
  const [showAutorDropdown, setShowAutorDropdown] = useState(false);
  const autorDropdownRef = useRef<HTMLDivElement>(null);

  const [categoriaSearch, setCategoriaSearch] = useState("");
  const [selectedCategoria, setSelectedCategoria] = useState<{
    id: string;
    nome: string;
  } | null>(null);
  const [showCategoriaDropdown, setShowCategoriaDropdown] = useState(false);
  const categoriaDropdownRef = useRef<HTMLDivElement>(null);

  const { data: autores, isLoading: isLoadingAutores } = useUsuarios(
    { nome: autorSearch, pageSize: 10 },
    { enabled: autorSearch.length > 0 }
  );

  const { data: categorias, isLoading: isLoadingCategorias } = useCategorias(
    { nome: categoriaSearch, pageSize: 10 },
    { enabled: categoriaSearch.length > 0 }
  );

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (
        autorDropdownRef.current &&
        !autorDropdownRef.current.contains(event.target as Node)
      ) {
        setShowAutorDropdown(false);
      }
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

  const handleLocalChange = (field: string, value: string) => {
    setLocalFilters((prev) => ({ ...prev, [field]: value }));
  };

  const handleApplyFilters = () => {
    const newFilters: Partial<ConteudoFilters> = {
      titulo: localFilters.titulo || undefined,
      visualizacoesMin: localFilters.visualizacoesMin
        ? parseInt(localFilters.visualizacoesMin)
        : undefined,
      visualizacoesMax: localFilters.visualizacoesMax
        ? parseInt(localFilters.visualizacoesMax)
        : undefined,
      likesMin: localFilters.likesMin
        ? parseInt(localFilters.likesMin)
        : undefined,
      likesMax: localFilters.likesMax
        ? parseInt(localFilters.likesMax)
        : undefined,
      periodoFrom: localFilters.periodoFrom || undefined,
      periodoTo: localFilters.periodoTo || undefined,
      nivelUsuario: (localFilters.nivelUsuario as ENivel) || undefined,
      autorId: selectedAutor?.id,
      categoriasIds: selectedCategoria ? [selectedCategoria.id] : undefined,
      orderBy: localFilters.orderBy,
    };

    onFiltersChange(newFilters);
  };

  const handleClearFilters = () => {
    setLocalFilters({
      titulo: "",
      visualizacoesMin: "",
      visualizacoesMax: "",
      likesMin: "",
      likesMax: "",
      categoriasIds: [],
      periodoFrom: "",
      periodoTo: "",
      nivelUsuario: "",
      orderBy: EConteudoOrderBy.MaisRecente,
    });
    setSelectedAutor(null);
    setSelectedCategoria(null);
    setAutorSearch("");
    setCategoriaSearch("");
    onFiltersChange({
      titulo: undefined,
      visualizacoesMin: undefined,
      visualizacoesMax: undefined,
      likesMin: undefined,
      likesMax: undefined,
      periodoFrom: undefined,
      periodoTo: undefined,
      nivelUsuario: undefined,
      autorId: undefined,
      categoriasIds: undefined,
      orderBy: EConteudoOrderBy.MaisRecente,
    });
  };

  return (
    <Card className="sticky top-20">
      <h2 className="text-xl font-bold text-black mb-4">Filtros</h2>

      <div className="space-y-4">
        <Input
          label="Título"
          value={localFilters.titulo}
          onChange={(e) => handleLocalChange("titulo", e.target.value)}
          placeholder="Buscar por título..."
        />

        <div>
          <label className="block text-sm font-medium text-black mb-1">
            Visualizações
          </label>
          <div className="flex gap-2">
            <Input
              type="number"
              placeholder="Min"
              value={localFilters.visualizacoesMin}
              onChange={(e) =>
                handleLocalChange("visualizacoesMin", e.target.value)
              }
              min="0"
            />
            <Input
              type="number"
              placeholder="Max"
              value={localFilters.visualizacoesMax}
              onChange={(e) => {
                const max = parseInt(e.target.value);
                const min = parseInt(localFilters.visualizacoesMin || "0");
                if (!e.target.value || max >= min) {
                  handleLocalChange("visualizacoesMax", e.target.value);
                }
              }}
              min={localFilters.visualizacoesMin || "0"}
            />
          </div>
        </div>

        <div>
          <label className="block text-sm font-medium text-black mb-1">
            Likes
          </label>
          <div className="flex gap-2">
            <Input
              type="number"
              placeholder="Min"
              value={localFilters.likesMin}
              onChange={(e) => handleLocalChange("likesMin", e.target.value)}
              min="0"
            />
            <Input
              type="number"
              placeholder="Max"
              value={localFilters.likesMax}
              onChange={(e) => {
                const max = parseInt(e.target.value);
                const min = parseInt(localFilters.likesMin || "0");
                if (!e.target.value || max >= min) {
                  handleLocalChange("likesMax", e.target.value);
                }
              }}
              min={localFilters.likesMin || "0"}
            />
          </div>
        </div>

        <div>
          <label className="block text-sm font-medium text-black mb-1">
            Período
          </label>
          <div className="flex flex-col gap-2">
            <Input
              type="date"
              max={new Date().toISOString().split("T")[0]}
              value={localFilters.periodoFrom}
              onChange={(e) => handleLocalChange("periodoFrom", e.target.value)}
            />
            <Input
              type="date"
              max={new Date().toISOString().split("T")[0]}
              value={localFilters.periodoTo}
              onChange={(e) => handleLocalChange("periodoTo", e.target.value)}
              min={localFilters.periodoFrom}
            />
          </div>
        </div>

        <div>
          <label className="block text-sm font-medium text-black mb-1">
            Nível do Usuário
          </label>
          <select
            value={localFilters.nivelUsuario}
            onChange={(e) => handleLocalChange("nivelUsuario", e.target.value)}
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary"
          >
            {nivelOptions.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
        </div>

        <div ref={autorDropdownRef}>
          <label className="block text-sm font-medium text-black mb-1">
            Autor
          </label>
          {selectedAutor ? (
            <div className="flex items-center gap-2 p-2 bg-background rounded-lg">
              <Avatar
                src={selectedAutor.imagem}
                alt={selectedAutor.nome}
                size="sm"
              />
              <span className="flex-1">{selectedAutor.nome}</span>
              <button
                onClick={() => setSelectedAutor(null)}
                className="text-primary hover:text-primary-dark"
              >
                ✕
              </button>
            </div>
          ) : (
            <div className="relative">
              <Input
                value={autorSearch}
                onChange={(e) => {
                  setAutorSearch(e.target.value);
                  setShowAutorDropdown(e.target.value.length > 0);
                }}
                placeholder="Buscar autor..."
              />
              {showAutorDropdown && (
                <div className="absolute w-full mt-2 bg-white rounded-lg shadow-lg border border-gray-200 max-h-64 overflow-y-auto z-10 animate-fadeIn">
                  {isLoadingAutores ? (
                    <div className="flex justify-center py-4">
                      <Spinner />
                    </div>
                  ) : autores && autores.items.length > 0 ? (
                    <div className="py-2">
                      {autores.items.map((autor) => (
                        <button
                          key={autor.id}
                          onClick={() => {
                            setSelectedAutor({
                              id: autor.id,
                              nome: autor.nome,
                              imagem: autor.caminhoImagem,
                            });
                            setAutorSearch("");
                            setShowAutorDropdown(false);
                          }}
                          className="w-full flex items-center gap-2 px-4 py-2 hover:bg-background transition-smooth"
                        >
                          <Avatar
                            src={autor.caminhoImagem}
                            alt={autor.nome}
                            size="sm"
                          />
                          <span>{autor.nome}</span>
                        </button>
                      ))}
                    </div>
                  ) : (
                    <div className="py-4 text-center text-secondary text-sm">
                      Nenhum autor encontrado
                    </div>
                  )}
                </div>
              )}
            </div>
          )}
        </div>

        <div ref={categoriaDropdownRef}>
          <label className="block text-sm font-medium text-black mb-1">
            Categoria
          </label>
          {selectedCategoria ? (
            <div className="flex items-center gap-2 p-2 bg-background rounded-lg">
              <span className="flex-1">{selectedCategoria.nome}</span>
              <button
                onClick={() => setSelectedCategoria(null)}
                className="text-primary hover:text-primary-dark"
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

        <div>
          <label className="block text-sm font-medium text-black mb-1">
            Ordenar por
          </label>
          <select
            value={localFilters.orderBy}
            onChange={(e) => handleLocalChange("orderBy", e.target.value)}
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary"
          >
            {orderByOptions.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
        </div>

        <div className="space-y-2 pt-4">
          <Button onClick={handleApplyFilters} fullWidth>
            Aplicar Filtros
          </Button>
          <Button onClick={handleClearFilters} variant="secondary" fullWidth>
            Limpar Filtros
          </Button>
        </div>
      </div>
    </Card>
  );
}
