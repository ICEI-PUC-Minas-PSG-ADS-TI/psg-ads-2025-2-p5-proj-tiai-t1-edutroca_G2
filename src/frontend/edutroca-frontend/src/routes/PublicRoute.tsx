import { Navigate, Outlet } from "react-router-dom";
import { authService } from "@/services/auth.service";
import { Layout } from "@/components/layout/Layout";

export function PublicRoute() {
  const isAuthenticated = authService.isAuthenticated();

  if (isAuthenticated) {
    return <Navigate to="/app" replace />;
  }

  return (
    <Layout>
      <Outlet />
    </Layout>
  );
}
