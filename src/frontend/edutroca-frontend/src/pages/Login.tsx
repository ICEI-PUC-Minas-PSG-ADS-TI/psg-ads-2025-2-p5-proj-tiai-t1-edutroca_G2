import { Link } from "react-router-dom";
import { LoginForm } from "@/components/auth/LoginForm";
import { Card } from "@/components/common/Card";
import LogoImage from "@/assets/images/logo.png";

export function Login() {
  return (
    <div className="min-h-screen flex items-center justify-center bg-background px-4 py-8">
      <Card className="w-full max-w-md">
        <div className="text-center mb-6">
          <img
            src={LogoImage}
            alt="EduTroca Logo"
            className="h-16 w-16 mx-auto mb-4 object-contain"
          />
          <h1 className="text-3xl font-bold text-black">Entrar</h1>
          <p className="text-secondary mt-2">Acesse sua conta EduTroca</p>
        </div>

        <LoginForm />

        <p className="text-center mt-4 text-sm text-secondary">
          Não tem uma conta?{" "}
          <Link
            to="/register"
            className="text-primary hover:underline font-medium"
          >
            Cadastre-se
          </Link>
        </p>
      </Card>
    </div>
  );
}
