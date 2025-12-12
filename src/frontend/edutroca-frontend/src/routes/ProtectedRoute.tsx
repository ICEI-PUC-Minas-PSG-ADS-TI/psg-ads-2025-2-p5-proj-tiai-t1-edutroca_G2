import { Navigate, Outlet } from "react-router-dom";
import { authService } from "@/services/auth.service";
import { Layout } from "@/components/layout/Layout";

export function ProtectedRoute() {
  const isAuthenticated = authService.isAuthenticated();

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return (
    <Layout>
      <Outlet />
    </Layout>
  );
}
