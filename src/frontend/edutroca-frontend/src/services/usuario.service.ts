import api from "@/lib/axios";
import type { PagedResult } from "@/types/common.types";
import type {
  SimpleUsuarioResponse,
  CompleteUsuarioResponse,
  UpdateUsuarioRequest,
} from "@/types/usuario.types";

export const usuarioService = {
  async getAll(params?: {
    nome?: string;
    categoriasIds?: string[];
    pageNumber?: number;
    pageSize?: number;
  }): Promise<PagedResult<SimpleUsuarioResponse>> {
    const queryParams = new URLSearchParams();
    if (params?.nome) queryParams.append("nome", params.nome);
    if (params?.categoriasIds) {
      params.categoriasIds.forEach((id) =>
        queryParams.append("categoriasIds", id)
      );
    }
    if (params?.pageNumber)
      queryParams.append("pageNumber", params.pageNumber.toString());
    if (params?.pageSize)
      queryParams.append("pageSize", params.pageSize.toString());

    const response = await api.get("/api/Usuarios", { params: queryParams });
    return response.data;
  },

  async getById(id: string): Promise<CompleteUsuarioResponse> {
    const response = await api.get(`/api/Usuarios/${id}`);
    return response.data;
  },

  async update(
    id: string,
    data: UpdateUsuarioRequest
  ): Promise<CompleteUsuarioResponse> {
    const formData = new FormData();
    if (data.nome) formData.append("nome", data.nome);
    if (data.bio) formData.append("bio", data.bio);
    if (data.profilePicture)
      formData.append("profilePicture", data.profilePicture);
    if (data.removePicture !== undefined)
      formData.append("removePicture", data.removePicture.toString());

    const response = await api.patch(`/api/Usuarios/${id}`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return response.data;
  },

  async updateEmail(id: string, novoEmail: string): Promise<void> {
    await api.patch(`/api/Usuarios/email/${id}`, { novoEmail });
  },

  async confirmEmail(code: string): Promise<void> {
    await api.patch(`/api/Usuarios/confirm-email/${code}`);
  },

  async setInterests(
    usuarioId: string,
    categoriasIds: [string]
  ): Promise<void> {
    await api.patch("/api/Usuarios/set-interests", {
      usuarioId,
      categoriasIds,
    });
  },
};
