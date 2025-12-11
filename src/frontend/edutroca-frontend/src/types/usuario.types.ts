import type { SimpleCategoriaResponse } from "./categoria.types";

export enum ENivel {
  Usuario = "usuario",
  Criador = "criador",
  CriadorPleno = "criadorPleno",
  CriadorCertificado = "criadorCertificado",
}

export enum ERole {
  Admin = "admin",
  User = "user",
  Owner = "owner",
}

export interface SimpleUsuarioResponse {
  id: string;
  nome: string;
  caminhoImagem: string | null;
  nivel: ENivel;
}

export interface CompleteUsuarioResponse {
  id: string;
  nome: string;
  email: string;
  bio: string | null;
  caminhoImagem: string | null;
  nivel: ENivel;
  roles: ERole[];
  categoriasDeInteresse: SimpleCategoriaResponse[];
}

export interface UpdateUsuarioRequest {
  nome?: string;
  bio?: string;
  profilePicture?: File;
  removePicture?: boolean;
}
