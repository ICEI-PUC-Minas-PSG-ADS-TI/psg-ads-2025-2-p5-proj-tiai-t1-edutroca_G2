import { useEffect, useState } from "react";
import { useSearchParams, Link } from "react-router-dom";
import { Card } from "@/components/common/Card";
import { Button } from "@/components/common/Button";
import { Spinner } from "@/components/common/Spinner";
import LogoImage from "@/assets/images/logo.png";
import { usuarioService } from "@/services/usuario.service";

export function ConfirmEmail() {
  const [searchParams] = useSearchParams();
  const [status, setStatus] = useState<"loading" | "success" | "error">(
    "loading"
  );
  const [errorMessage, setErrorMessage] = useState("");

  const token = searchParams.get("token");

  useEffect(() => {
    const confirmEmail = async () => {
      if (!token) {
        setStatus("error");
        setErrorMessage("Token de confirmação não encontrado.");
        return;
      }

      try {
        await usuarioService.confirmEmail(token);
        setStatus("success");
      } catch (error: any) {
        setStatus("error");
        const message =
          error?.response?.data?.detail ||
          error?.response?.data?.title ||
          "Erro ao confirmar email. O link pode estar expirado.";
        setErrorMessage(message);
      }
    };

    confirmEmail();
  }, [token]);

  return (
    <div className="min-h-screen flex items-center justify-center bg-background px-4">
      <Card className="w-full max-w-md text-center">
        <img
          src={LogoImage}
          alt="EduTroca Logo"
          className="h-16 w-16 mx-auto mb-4 object-contain"
        />

        {status === "loading" && (
          <div className="py-8">
            <Spinner size="lg" className="mx-auto mb-4" />
            <p className="text-secondary">Confirmando seu email...</p>
          </div>
        )}

        {status === "success" && (
          <div className="py-4">
            <div className="mb-4">
              <svg
                className="w-16 h-16 mx-auto text-green-500"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
            </div>
            <h1 className="text-2xl font-bold text-black mb-2">
              Email confirmado com sucesso!
            </h1>
            <p className="text-secondary mb-6">
              Sua conta foi ativada. Agora você pode fazer login.
            </p>
            <Link to="/login">
              <Button fullWidth>Ir para Login</Button>
            </Link>
          </div>
        )}

        {status === "error" && (
          <div className="py-4">
            <div className="mb-4">
              <svg
                className="w-16 h-16 mx-auto text-primary"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
            </div>
            <h1 className="text-2xl font-bold text-black mb-2">
              Erro na Confirmação
            </h1>
            <p className="text-secondary mb-6">{errorMessage}</p>
            <Link to="/login">
              <Button variant="secondary" fullWidth>
                Voltar para Login
              </Button>
            </Link>
          </div>
        )}
      </Card>
    </div>
  );
}
