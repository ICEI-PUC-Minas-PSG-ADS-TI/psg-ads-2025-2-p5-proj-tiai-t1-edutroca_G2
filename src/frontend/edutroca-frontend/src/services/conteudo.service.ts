import api from "@/lib/axios";
import type { PagedResult } from "@/types/common.types";
import type {
  SimpleConteudoResponse,
  CompleteConteudoResponse,
  CreateVideoRequest,
  CreatePerguntaRequest,
  UpdateVideoRequest,
  UpdatePerguntaRequest,
  CommentRequest,
  ConteudoFilters,
} from "@/types/conteudo.types";

export const conteudoService = {
  async getAll(
    filters?: ConteudoFilters
  ): Promise<PagedResult<SimpleConteudoResponse>> {
    const params = new URLSearchParams();

    if (filters?.titulo) params.append("Titulo", filters.titulo);
    if (filters?.visualizacoesMin)
      params.append("Visualizacoes.Min", filters.visualizacoesMin.toString());
    if (filters?.visualizacoesMax)
      params.append("Visualizacoes.Max", filters.visualizacoesMax.toString());
    if (filters?.likesMin)
      params.append("Likes.Min", filters.likesMin.toString());
    if (filters?.likesMax)
      params.append("Likes.Max", filters.likesMax.toString());
    if (filters?.periodoFrom)
      params.append("Periodo.From", filters.periodoFrom);
    if (filters?.periodoTo) params.append("Periodo.To", filters.periodoTo);
    if (filters?.nivelUsuario)
      params.append("NivelUsuario", filters.nivelUsuario);
    if (filters?.autorId) params.append("AutorId", filters.autorId);
    if (filters?.categoriasIds) {
      filters.categoriasIds.forEach((id) => params.append("CategoriasIds", id));
    }
    if (filters?.tipo) params.append("Tipo", filters.tipo);
    if (filters?.orderBy) params.append("OrderBy", filters.orderBy);
    if (filters?.pageNumber)
      params.append("pageNumber", filters.pageNumber.toString());
    if (filters?.pageSize)
      params.append("pageSize", filters.pageSize.toString());

    const response = await api.get("/api/Conteudos", { params });
    return response.data;
  },

  async getById(id: string): Promise<CompleteConteudoResponse> {
    const response = await api.get(`/api/Conteudos/${id}`);
    return response.data;
  },

  async createVideo(data: CreateVideoRequest): Promise<string> {
    const formData = new FormData();
    formData.append("titulo", data.titulo);
    formData.append("descricao", data.descricao);
    formData.append("categoriaId", data.categoriaId);
    formData.append("video", data.video);
    if (data.imagem) formData.append("imagem", data.imagem);

    const response = await api.post("/api/Conteudos/video", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return response.data;
  },

  async createPergunta(data: CreatePerguntaRequest): Promise<string> {
    const formData = new FormData();
    formData.append("titulo", data.titulo);
    formData.append("descricao", data.descricao);
    formData.append("categoriaId", data.categoriaId);
    formData.append("texto", data.texto);

    const response = await api.post("/api/Conteudos/pergunta", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return response.data;
  },

  async updateVideo(id: string, data: UpdateVideoRequest): Promise<void> {
    const formData = new FormData();
    if (data.titulo) formData.append("titulo", data.titulo);
    if (data.descricao) formData.append("descricao", data.descricao);
    if (data.categoriaId) formData.append("categoriaId", data.categoriaId);
    if (data.imagem) formData.append("imagem", data.imagem);

    await api.patch(`/api/Conteudos/video/${id}`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
  },

  async updatePergunta(id: string, data: UpdatePerguntaRequest): Promise<void> {
    const formData = new FormData();
    if (data.titulo) formData.append("titulo", data.titulo);
    if (data.descricao) formData.append("descricao", data.descricao);
    if (data.categoriaId) formData.append("categoriaId", data.categoriaId);
    if (data.texto) formData.append("texto", data.texto);

    await api.patch(`/api/Conteudos/pergunta/${id}`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
  },

  async deleteVideo(id: string): Promise<void> {
    await api.delete(`/api/Conteudos/video/${id}`);
  },

  async deletePergunta(id: string): Promise<void> {
    await api.delete(`/api/Conteudos/pergunta/${id}`);
  },

  async toggleLike(id: string): Promise<void> {
    await api.patch(`/api/Conteudos/toggle-like/${id}`);
  },

  async toggleDislike(id: string): Promise<void> {
    await api.patch(`/api/Conteudos/toggle-dislike/${id}`);
  },

  async addComment(id: string, data: CommentRequest): Promise<void> {
    await api.post(`/api/Conteudos/comment/${id}`, data);
  },
};
