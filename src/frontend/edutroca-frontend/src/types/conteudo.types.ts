import type { SimpleCategoriaResponse } from "./categoria.types";
import type { ENivel, SimpleUsuarioResponse } from "./usuario.types";

export enum EConteudoTipo {
  Video = "video",
  Pergunta = "pergunta",
}

export enum EConteudoOrderBy {
  MaisRecente = "maisRecente",
  MaisAntigo = "maisAntigo",
  MaisVisualizacoes = "maisVisualizacoes",
  MenosVisualizacoes = "menosVisualizacoes",
  MaisLikes = "maisLikes",
  MenosLikes = "menosLikes",
  MaiorReputacao = "maiorReputacao",
  MenorReputacao = "menorReputacao",
}

export { ENivel } from "./usuario.types";

export interface SimpleConteudoResponse {
  id: string;
  titulo: string;
  descricao: string;
  caminhoImagem: string | null;
  dataPublicacao: string;
  visualizacoes: number;
  likes: number;
  dislikes: number;
  tipo: EConteudoTipo;
  autor: SimpleUsuarioResponse;
  categoria: SimpleCategoriaResponse;
  totalComentarios: number;
}

export interface CompleteConteudoResponse {
  id: string;
  titulo: string;
  descricao: string;
  caminhoImagem: string | null;
  caminhoVideo: string | null;
  textoCompleto: string | null;
  dataPublicacao: string;
  visualizacoes: number;
  likes: number;
  liked: boolean;
  disikes: number;
  disliked: boolean;
  tipo: EConteudoTipo;
  autor: SimpleUsuarioResponse;
  categoria: SimpleCategoriaResponse;
  comentarios: ComentarioResponse[];
  totalComentarios: number;
}

export interface ComentarioResponse {
  id: string;
  texto: string;
  dataPublicacao: string;
  autor: SimpleUsuarioResponse;
}

export interface CreateVideoRequest {
  titulo: string;
  descricao: string;
  categoriaId: string;
  video: File;
  imagem?: File;
}

export interface CreatePerguntaRequest {
  titulo: string;
  descricao: string;
  categoriaId: string;
  texto: string;
}

export interface UpdateVideoRequest {
  titulo?: string;
  descricao?: string;
  categoriaId?: string;
  imagem?: File;
}

export interface UpdatePerguntaRequest {
  titulo?: string;
  descricao?: string;
  categoriaId?: string;
  texto?: string;
}

export interface CommentRequest {
  texto: string;
}

export interface ConteudoFilters {
  titulo?: string;
  visualizacoesMin?: number;
  visualizacoesMax?: number;
  likesMin?: number;
  likesMax?: number;
  periodoFrom?: string;
  periodoTo?: string;
  nivelUsuario?: ENivel;
  autorId?: string;
  categoriasIds?: string[];
  tipo?: EConteudoTipo;
  orderBy?: EConteudoOrderBy;
  pageNumber?: number;
  pageSize?: number;
}
