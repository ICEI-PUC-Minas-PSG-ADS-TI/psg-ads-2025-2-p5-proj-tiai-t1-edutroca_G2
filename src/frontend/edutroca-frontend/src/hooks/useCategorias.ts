import { useQuery } from "@tanstack/react-query";
import { categoriaService } from "@/services/categoria.service";

export function useCategorias(
  params?: {
    nome?: string;
    pageNumber?: number;
    pageSize?: number;
  },
  options?: {
    enabled?: boolean;
  }
) {
  return useQuery({
    queryKey: ["categorias", params],
    queryFn: () => categoriaService.getAll(params),
    staleTime: 5 * 60 * 1000,
    enabled: options?.enabled !== false,
  });
}

export function useCategoria(id: string) {
  return useQuery({
    queryKey: ["categoria", id],
    queryFn: () => categoriaService.getById(id),
    enabled: !!id,
  });
}
