import api from "@/lib/axios";
import type { PagedResult } from "@/types/common.types";
import type {
  SimpleCategoriaResponse,
  CompleteCategoriaResponse,
} from "@/types/categoria.types";

export const categoriaService = {
  async getAll(params?: {
    nome?: string;
    pageNumber?: number;
    pageSize?: number;
  }): Promise<PagedResult<SimpleCategoriaResponse>> {
    const response = await api.get("/api/Categorias", { params });
    return response.data;
  },

  async getById(id: string): Promise<CompleteCategoriaResponse> {
    const response = await api.get(`/api/Categorias/${id}`);
    return response.data;
  },
};
