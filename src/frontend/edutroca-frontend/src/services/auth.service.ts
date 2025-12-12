import api from "@/lib/axios";
import type {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
} from "@/types/auth.types";
import type { CompleteUsuarioResponse } from "@/types/usuario.types";

export const authService = {
  async register(data: RegisterRequest): Promise<CompleteUsuarioResponse> {
    const response = await api.post("/api/Usuarios", data);
    return response.data;
  },

  async login(data: LoginRequest): Promise<LoginResponse> {
    const response = await api.post("/api/Usuarios/login", data);
    const { accessToken, refreshToken, usuario } = response.data;

    localStorage.setItem("accessToken", accessToken);
    localStorage.setItem("refreshToken", refreshToken);
    localStorage.setItem("user", JSON.stringify(usuario));

    return response.data;
  },

  async refreshToken(refreshToken: string): Promise<LoginResponse> {
    const response = await api.post("/api/Usuarios/refresh-token", {
      refreshToken,
    });
    return response.data;
  },

  logout() {
    localStorage.clear();
    window.location.href = "/login";
  },

  getCurrentUser(): CompleteUsuarioResponse | null {
    const user = localStorage.getItem("user");
    return user ? JSON.parse(user) : null;
  },

  isAuthenticated(): boolean {
    return !!localStorage.getItem("accessToken");
  },
};
