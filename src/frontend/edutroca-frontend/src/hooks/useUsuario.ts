import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { usuarioService } from "@/services/usuario.service";
import type { UpdateUsuarioRequest } from "@/types/usuario.types";

export function useUsuarios(
  params?: {
    nome?: string;
    categoriasIds?: string[];
    pageNumber?: number;
    pageSize?: number;
  },
  options?: {
    enabled?: boolean;
  }
) {
  return useQuery({
    queryKey: ["usuarios", params],
    queryFn: () => usuarioService.getAll(params),
    enabled: options?.enabled !== false,
  });
}

export function useUsuario(id: string) {
  return useQuery({
    queryKey: ["usuario", id],
    queryFn: () => usuarioService.getById(id),
    enabled: !!id,
  });
}

export function useUpdateUsuario() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateUsuarioRequest }) =>
      usuarioService.update(id, data),
    onSuccess: (updatedUser) => {
      queryClient.invalidateQueries({ queryKey: ["usuario", updatedUser.id] });
      queryClient.invalidateQueries({ queryKey: ["usuarios"] });
      localStorage.setItem("user", JSON.stringify(updatedUser));
    },
  });
}

export function useUpdateEmail() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, novoEmail }: { id: string; novoEmail: string }) =>
      usuarioService.updateEmail(id, novoEmail),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["usuarios"] });
    },
  });
}

export function useSetInterests() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      usuarioId,
      categoriasIds,
    }: {
      usuarioId: string;
      categoriasIds: [string];
    }) => usuarioService.setInterests(usuarioId, categoriasIds),
    onSuccess: (_, { usuarioId }) => {
      queryClient.invalidateQueries({ queryKey: ["usuario", usuarioId] });
    },
  });
}
