import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { useAuth } from "@/hooks/useAuth";
import { Button } from "@/components/common/Button";
import { Input } from "@/components/common/Input";
import { useState } from "react";

const registerSchema = z
  .object({
    nome: z.string().min(3, "Nome deve ter no mínimo 3 caracteres"),
    email: z.string().email("Email inválido"),
    senha: z.string().min(6, "Senha deve ter no mínimo 6 caracteres"),
    confirmarSenha: z.string(),
  })
  .refine((data) => data.senha === data.confirmarSenha, {
    message: "As senhas não coincidem",
    path: ["confirmarSenha"],
  });

type RegisterFormData = z.infer<typeof registerSchema>;

export function RegisterForm() {
  const { register: registerUser, isRegistering } = useAuth();
  const [apiError, setApiError] = useState<string>("");
  const [success, setSuccess] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegisterFormData>({
    resolver: zodResolver(registerSchema),
  });

  const onSubmit = async (data: RegisterFormData) => {
    try {
      setApiError("");
      setSuccess(false);
      await registerUser({
        nome: data.nome,
        email: data.email,
        senha: data.senha,
      });
      setSuccess(true);
    } catch (error: any) {
      const message =
        error?.response?.data?.detail ||
        error?.response?.data?.title ||
        error?.response?.data?.errors?.Email?.[0] ||
        error?.response?.data?.errors?.Senha?.[0] ||
        "Erro ao criar conta. Tente novamente.";
      setApiError(message);
    }
  };

  if (success) {
    return (
      <div className="bg-green-50 border border-green-200 text-green-700 px-4 py-3 rounded-lg text-center">
        <p className="font-medium mb-2">Conta criada com sucesso! 🎉</p>
        <p className="text-sm">Você será redirecionado para o login...</p>
      </div>
    );
  }

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      {apiError && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg text-sm">
          {apiError}
        </div>
      )}

      <Input
        label="Nome"
        {...register("nome")}
        error={errors.nome?.message}
        disabled={isRegistering}
      />

      <Input
        label="Email"
        type="email"
        {...register("email")}
        error={errors.email?.message}
        disabled={isRegistering}
      />

      <Input
        label="Senha"
        type="password"
        {...register("senha")}
        error={errors.senha?.message}
        disabled={isRegistering}
      />

      <Input
        label="Confirmar Senha"
        type="password"
        {...register("confirmarSenha")}
        error={errors.confirmarSenha?.message}
        disabled={isRegistering}
      />

      <Button type="submit" fullWidth disabled={isRegistering}>
        {isRegistering ? "Cadastrando..." : "Cadastrar"}
      </Button>
    </form>
  );
}
