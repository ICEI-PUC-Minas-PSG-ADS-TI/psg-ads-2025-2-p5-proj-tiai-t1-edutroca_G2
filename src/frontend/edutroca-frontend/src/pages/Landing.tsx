import { Link } from "react-router-dom";
import { Button } from "@/components/common/Button";
import LogoImage from "@/assets/images/logo.png";

export function Landing() {
  return (
    <div className="min-h-dvh flex flex-col items-center justify-center bg-background px-4">
      <div className="max-w-2xl mx-auto text-center">
        <img
          src={LogoImage}
          alt="EduTroca Logo"
          className="h-24 w-24 mx-auto mb-6 object-contain"
        />

        <h1 className="text-5xl font-bold text-black mb-4">
          Bem-vindo ao EduTroca
        </h1>

        <p className="text-xl text-secondary mb-8">
          Uma plataforma colaborativa de compartilhamento de conhecimento
          através de vídeos e perguntas. Aprenda, ensine e conecte-se com
          pessoas apaixonadas por educação.
        </p>

        <Link to="/register">
          <Button className="text-lg px-8 py-3">Começar</Button>
        </Link>

        <p className="mt-4 text-secondary">
          Já tem uma conta?{" "}
          <Link
            to="/login"
            className="text-primary hover:underline font-medium"
          >
            Faça login
          </Link>
        </p>
      </div>
    </div>
  );
}
