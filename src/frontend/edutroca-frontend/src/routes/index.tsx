import { createBrowserRouter } from "react-router-dom";
import { ProtectedRoute } from "./ProtectedRoute";
import { PublicRoute } from "./PublicRoute";
import { Landing } from "@/pages/Landing";
import { Login } from "@/pages/Login";
import { Register } from "@/pages/Register";
import { Home } from "@/pages/Home";
import { Feed } from "@/pages/Feed";
import { ConteudoDetail } from "@/pages/ConteudoDetail";
import { Perfil } from "@/pages/Perfil";
import { ConfirmEmail } from "@/pages/ConfirmEmail";

export const router = createBrowserRouter([
  {
    element: <PublicRoute />,
    children: [
      {
        path: "/",
        element: <Landing />,
      },
      {
        path: "/login",
        element: <Login />,
      },
      {
        path: "/register",
        element: <Register />,
      },
    ],
  },
  {
    path: "/confirmar",
    element: <ConfirmEmail />,
  },
  {
    path: "/app",
    element: <ProtectedRoute />,
    children: [
      {
        index: true,
        element: <Home />,
      },
      {
        path: "feed",
        element: <Feed />,
      },
      {
        path: "conteudo/:id",
        element: <ConteudoDetail />,
      },
      {
        path: "perfil/:id",
        element: <Perfil />,
      },
    ],
  },
  {
    path: "*",
    element: (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <h1 className="text-4xl font-bold mb-4">404</h1>
          <p className="text-gray-600">Página não encontrada</p>
        </div>
      </div>
    ),
  },
]);
