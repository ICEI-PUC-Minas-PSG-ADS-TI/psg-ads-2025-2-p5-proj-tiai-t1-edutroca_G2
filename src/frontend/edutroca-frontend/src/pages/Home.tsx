import { useState, useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";
import { useCategorias } from "@/hooks/useCategorias";
import { Card } from "@/components/common/Card";
import { Button } from "@/components/common/Button";
import { Spinner } from "@/components/common/Spinner";
import type { SimpleCategoriaResponse } from "@/types/categoria.types";

export function Home() {
  const navigate = useNavigate();
  const [searchTerm, setSearchTerm] = useState("");
  const [showDropdown, setShowDropdown] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  const { data: categoriasIniciais, isLoading: isLoadingIniciais } =
    useCategorias({
      pageSize: 6,
    });

  const { data: categoriasFiltradas, isLoading: isLoadingFiltradas } =
    useCategorias(
      {
        nome: searchTerm,
        pageSize: 10,
      },
      {
        enabled: searchTerm.length > 0,
      }
    );

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (
        dropdownRef.current &&
        !dropdownRef.current.contains(event.target as Node)
      ) {
        setShowDropdown(false);
      }
    }

    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const handleCategoriaClick = (categoriaId: string) => {
    navigate(`/app/feed?categoria=${categoriaId}`);
  };

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setSearchTerm(value);
    setShowDropdown(value.length > 0);
  };

  const handleSelectCategoria = (categoria: SimpleCategoriaResponse) => {
    setSearchTerm("");
    setShowDropdown(false);
    handleCategoriaClick(categoria.id);
  };

  if (isLoadingIniciais) {
    return (
      <div className="flex justify-center items-center min-h-[400px]">
        <Spinner size="lg" />
      </div>
    );
  }

  const temCategorias =
    categoriasIniciais && categoriasIniciais.items.length > 0;

  return (
    <div className="max-w-4xl mx-auto">
      <Card className="mb-8 text-center">
        <h1 className="text-3xl font-bold text-black mb-2">Bem-vindo! 👋</h1>
        <p className="text-secondary text-lg">
          Escolha uma categoria para começar
        </p>
      </Card>

      {temCategorias ? (
        <>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-6">
            {categoriasIniciais.items.map((categoria) => (
              <Button
                key={categoria.id}
                onClick={() => handleCategoriaClick(categoria.id)}
                className="h-16 text-lg"
              >
                {categoria.nome}
              </Button>
            ))}
          </div>

          <div className="relative" ref={dropdownRef}>
            <input
              type="text"
              value={searchTerm}
              onChange={handleSearchChange}
              onFocus={() => searchTerm.length > 0 && setShowDropdown(true)}
              placeholder="Buscar outras categorias..."
              className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary text-base"
            />

            {showDropdown && (
              <div className="absolute w-full mt-2 bg-white rounded-lg shadow-lg border border-gray-200 max-h-64 overflow-y-auto z-10 animate-fadeIn">
                {isLoadingFiltradas ? (
                  <div className="flex justify-center items-center py-8">
                    <Spinner />
                  </div>
                ) : categoriasFiltradas &&
                  categoriasFiltradas.items.length > 0 ? (
                  <div className="py-2">
                    {categoriasFiltradas.items.map((categoria) => (
                      <button
                        key={categoria.id}
                        onClick={() => handleSelectCategoria(categoria)}
                        className="w-full text-left px-4 py-3 hover:bg-background transition-smooth text-black"
                      >
                        {categoria.nome}
                      </button>
                    ))}
                  </div>
                ) : (
                  <div className="py-8 text-center text-secondary">
                    Nenhuma categoria encontrada
                  </div>
                )}
              </div>
            )}
          </div>
        </>
      ) : (
        <Card className="text-center">
          <p className="text-secondary text-lg">
            Infelizmente ainda não possuímos nenhuma categoria disponível. Volte
            mais tarde.
          </p>
        </Card>
      )}
    </div>
  );
}
