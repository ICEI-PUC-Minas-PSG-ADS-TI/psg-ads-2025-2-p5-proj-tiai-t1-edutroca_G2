import type { CompleteUsuarioResponse } from "./usuario.types";

export interface LoginRequest {
  email: string;
  senha: string;
}

export interface RegisterRequest {
  nome: string;
  email: string;
  senha: string;
}

export interface LoginResponse {
  usuario: CompleteUsuarioResponse;
  accessToken: string;
  refreshToken: string;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}
