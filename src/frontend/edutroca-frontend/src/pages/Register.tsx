import { Link } from "react-router-dom";
import { RegisterForm } from "@/components/auth/RegisterForm";
import { Card } from "@/components/common/Card";
import LogoImage from "@/assets/images/logo.png";

export function Register() {
  return (
    <div className="min-h-screen flex items-center justify-center bg-background px-4 py-8">
      <Card className="w-full max-w-md">
        <div className="text-center mb-6">
          <img
            src={LogoImage}
            alt="EduTroca Logo"
            className="h-16 w-16 mx-auto mb-4 object-contain"
          />
          <h1 className="text-3xl font-bold text-black">Criar conta</h1>
          <p className="text-secondary mt-2">Junte-se à comunidade EduTroca</p>
        </div>

        <RegisterForm />

        <p className="text-center mt-4 text-sm text-secondary">
          Já tem uma conta?{" "}
          <Link
            to="/login"
            className="text-primary hover:underline font-medium"
          >
            Faça login
          </Link>
        </p>
      </Card>
    </div>
  );
}
