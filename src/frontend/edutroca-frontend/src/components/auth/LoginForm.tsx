import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { useAuth } from "@/hooks/useAuth";
import { Button } from "@/components/common/Button";
import { Input } from "@/components/common/Input";
import { useState } from "react";

const loginSchema = z.object({
  email: z.string().email("Email inválido"),
  senha: z.string().min(1, "Senha é obrigatória"),
});

type LoginFormData = z.infer<typeof loginSchema>;

export function LoginForm() {
  const { login, isLoggingIn } = useAuth();
  const [apiError, setApiError] = useState<string>("");

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
  });

  const onSubmit = async (data: LoginFormData) => {
    try {
      setApiError("");
      await login(data);
    } catch (error: any) {
      const message =
        error?.response?.data?.detail ||
        error?.response?.data?.title ||
        "Erro ao fazer login. Verifique suas credenciais.";
      setApiError(message);
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      {apiError && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg text-sm">
          {apiError}
        </div>
      )}

      <Input
        label="Email"
        type="email"
        {...register("email")}
        error={errors.email?.message}
        disabled={isLoggingIn}
      />

      <Input
        label="Senha"
        type="password"
        {...register("senha")}
        error={errors.senha?.message}
        disabled={isLoggingIn}
      />

      <Button type="submit" fullWidth disabled={isLoggingIn}>
        {isLoggingIn ? "Entrando..." : "Entrar"}
      </Button>
    </form>
  );
}
