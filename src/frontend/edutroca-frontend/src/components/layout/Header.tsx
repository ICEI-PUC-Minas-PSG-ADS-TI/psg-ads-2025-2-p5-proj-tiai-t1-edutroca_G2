import { useState } from "react";
import { Link, useNavigate, useLocation } from "react-router-dom";
import { Menu, X } from "lucide-react";
import { useAuth } from "@/hooks/useAuth";
import { Avatar } from "@/components/common/Avatar";
import LogoImage from "@/assets/images/logo.png";

export interface NavItem {
  label: string;
  path: string;
  requireAuth?: boolean;
}

const navigationItems: NavItem[] = [
  { label: "Explorar", path: "/app/feed", requireAuth: true },
];

export function Header() {
  const { currentUser, logout, isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [isProfileMenuOpen, setIsProfileMenuOpen] = useState(false);

  const handleLogout = () => {
    logout();
    setIsProfileMenuOpen(false);
    navigate("/login");
  };

  const goToProfile = () => {
    if (currentUser) {
      navigate(`/app/perfil/${currentUser.id}`);
      setIsProfileMenuOpen(false);
    }
  };

  const toggleMenu = () => setIsMenuOpen(!isMenuOpen);
  const toggleProfileMenu = () => setIsProfileMenuOpen(!isProfileMenuOpen);

  const visibleNavItems = navigationItems.filter(
    (item) => !item.requireAuth || (item.requireAuth && isAuthenticated)
  );

  const isActivePath = (path: string) => location.pathname === path;

  return (
    <header className="bg-white shadow-header sticky top-0 z-50">
      <div className="container mx-auto px-4">
        <div className="flex items-center justify-between h-16">
          <Link
            to={isAuthenticated ? "/app" : "/"}
            className="flex items-center gap-3 hover:opacity-80 transition-smooth"
          >
            <img
              src={LogoImage}
              alt="EduTroca Logo"
              className="h-10 w-10 object-contain"
            />
            <span className="text-2xl font-bold text-black">EduTroca</span>
          </Link>

          {isAuthenticated && (
            <div className="hidden md:flex items-center gap-6">
              {/* Navegação */}
              {visibleNavItems.length > 0 && (
                <nav className="flex items-center gap-6">
                  {visibleNavItems.map((item) => (
                    <Link
                      key={item.path}
                      to={item.path}
                      className={`
                        text-base font-medium transition-smooth
                        ${
                          isActivePath(item.path)
                            ? "text-primary"
                            : "text-black hover:text-primary"
                        }
                      `}
                    >
                      {item.label}
                    </Link>
                  ))}
                </nav>
              )}

              {currentUser && (
                <div className="relative">
                  <button
                    onClick={toggleProfileMenu}
                    className="flex items-center gap-2 hover:opacity-80 transition-smooth"
                    aria-label="Menu do usuário"
                  >
                    <Avatar
                      src={currentUser.caminhoImagem}
                      alt={currentUser.nome}
                      size="md"
                    />
                  </button>

                  {isProfileMenuOpen && (
                    <>
                      <div
                        className="fixed inset-0 z-10"
                        onClick={() => setIsProfileMenuOpen(false)}
                      />

                      <div className="absolute right-0 mt-2 w-48 bg-white rounded-lg shadow-lg py-2 z-20 animate-fadeIn">
                        <button
                          onClick={goToProfile}
                          className="w-full text-left px-4 py-2 text-black hover:bg-background transition-smooth"
                        >
                          Perfil
                        </button>
                        <button
                          onClick={handleLogout}
                          className="w-full text-left px-4 py-2 text-primary hover:bg-background transition-smooth"
                        >
                          Sair
                        </button>
                      </div>
                    </>
                  )}
                </div>
              )}
            </div>
          )}

          {isAuthenticated && (
            <button
              onClick={toggleMenu}
              className="md:hidden p-2 hover:bg-background rounded-lg transition-smooth"
              aria-label="Toggle menu"
            >
              {isMenuOpen ? <X size={24} /> : <Menu size={24} />}
            </button>
          )}
        </div>

        {isAuthenticated && isMenuOpen && (
          <div className="md:hidden py-4 border-t border-gray-200 animate-slideDown">
            <nav className="flex flex-col gap-2">
              {visibleNavItems.map((item) => (
                <Link
                  key={item.path}
                  to={item.path}
                  onClick={() => setIsMenuOpen(false)}
                  className={`
                    px-4 py-2 rounded-lg text-base font-medium transition-smooth
                    ${
                      isActivePath(item.path)
                        ? "bg-primary text-white"
                        : "text-black hover:bg-background"
                    }
                  `}
                >
                  {item.label}
                </Link>
              ))}

              {currentUser && (
                <>
                  <div className="border-t border-gray-200 my-2" />
                  <button
                    onClick={goToProfile}
                    className="text-left px-4 py-2 rounded-lg text-base font-medium text-black hover:bg-background transition-smooth"
                  >
                    Perfil
                  </button>
                  <button
                    onClick={handleLogout}
                    className="text-left px-4 py-2 rounded-lg text-base font-medium text-primary hover:bg-background transition-smooth"
                  >
                    Sair
                  </button>
                </>
              )}
            </nav>
          </div>
        )}
      </div>
    </header>
  );
}
