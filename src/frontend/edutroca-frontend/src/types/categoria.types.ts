export interface SimpleCategoriaResponse {
  id: string;
  nome: string;
}

export interface CompleteCategoriaResponse {
  id: string;
  nome: string;
  descricao: string | null;
}

export interface CreateCategoriaRequest {
  nome: string;
  descricao?: string;
}
