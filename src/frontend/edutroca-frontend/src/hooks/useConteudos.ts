import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { conteudoService } from "@/services/conteudo.service";
import type {
  ConteudoFilters,
  CreateVideoRequest,
  CreatePerguntaRequest,
  UpdateVideoRequest,
  UpdatePerguntaRequest,
  CommentRequest,
} from "@/types/conteudo.types";

export function useConteudos(filters?: ConteudoFilters) {
  return useQuery({
    queryKey: ["conteudos", filters],
    queryFn: () => conteudoService.getAll(filters),
  });
}

export function useConteudo(id: string) {
  return useQuery({
    queryKey: ["conteudo", id],
    queryFn: () => conteudoService.getById(id),
    enabled: !!id,
  });
}

export function useCreateVideo() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateVideoRequest) => conteudoService.createVideo(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["conteudos"] });
    },
  });
}

export function useCreatePergunta() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreatePerguntaRequest) =>
      conteudoService.createPergunta(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["conteudos"] });
    },
  });
}

export function useUpdateVideo() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateVideoRequest }) =>
      conteudoService.updateVideo(id, data),
    onSuccess: (_, { id }) => {
      queryClient.invalidateQueries({ queryKey: ["conteudo", id] });
      queryClient.invalidateQueries({ queryKey: ["conteudos"] });
    },
  });
}

export function useUpdatePergunta() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdatePerguntaRequest }) =>
      conteudoService.updatePergunta(id, data),
    onSuccess: (_, { id }) => {
      queryClient.invalidateQueries({ queryKey: ["conteudo", id] });
      queryClient.invalidateQueries({ queryKey: ["conteudos"] });
    },
  });
}

export function useDeleteVideo() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => conteudoService.deleteVideo(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["conteudos"] });
    },
  });
}

export function useDeletePergunta() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => conteudoService.deletePergunta(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["conteudos"] });
    },
  });
}

export function useToggleLike() {
  return useMutation({
    mutationFn: (id: string) => conteudoService.toggleLike(id),
  });
}

export function useToggleDislike() {
  return useMutation({
    mutationFn: (id: string) => conteudoService.toggleDislike(id),
  });
}

export function useAddComment() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: CommentRequest }) =>
      conteudoService.addComment(id, data),
    onSuccess: (_, { id }) => {
      queryClient.invalidateQueries({ queryKey: ["conteudo", id] });
    },
  });
}
