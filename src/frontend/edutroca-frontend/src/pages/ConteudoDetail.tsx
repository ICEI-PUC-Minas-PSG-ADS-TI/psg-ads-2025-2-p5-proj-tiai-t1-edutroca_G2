import { useState } from "react";
import { useParams, useNavigate, Link } from "react-router-dom";
import { ThumbsUp, MessageCircle, Eye, ThumbsDown } from "lucide-react";
import {
  useConteudo,
  useToggleLike,
  useToggleDislike,
  useAddComment,
} from "@/hooks/useConteudos";
import { useAuth } from "@/hooks/useAuth";
import { Card } from "@/components/common/Card";
import { Button } from "@/components/common/Button";
import { Avatar } from "@/components/common/Avatar";
import { Spinner } from "@/components/common/Spinner";
import { Input } from "@/components/common/Input";
import { EConteudoTipo } from "@/types/conteudo.types";

export function ConteudoDetail() {
  const { id } = useParams<{ id: string }>();
  const { currentUser } = useAuth();
  const navigate = useNavigate();
  const { data: conteudo, isLoading } = useConteudo(id!);
  const toggleLike = useToggleLike();
  const toggleDislike = useToggleDislike();
  const addComment = useAddComment();

  const [comentario, setComentario] = useState("");
  const [isPostingComment, setIsPostingComment] = useState(false);

  const formatDate = (date: string) => {
    return new Date(date).toLocaleDateString("pt-BR", {
      day: "2-digit",
      month: "long",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  const handleLike = async () => {
    if (!id) return;
    try {
      await toggleLike.mutateAsync(id);
      if (conteudo) {
        if (!conteudo.liked) {
          conteudo.liked = true;
          conteudo.likes++;
        } else {
          conteudo.liked = false;
          conteudo.likes--;
        }
        if (conteudo.disliked) {
          conteudo.disliked = false;
          conteudo.disikes--;
        }
      }
    } catch (error) {
      console.error("Erro ao dar like:", error);
    }
  };

  const handleDislike = async () => {
    if (!id) return;
    try {
      await toggleDislike.mutateAsync(id);
      if (conteudo) {
        if (!conteudo.disliked) {
          conteudo.disliked = true;
          conteudo.disikes++;
        } else {
          conteudo.disliked = false;
          conteudo.disikes--;
        }
        if (conteudo.liked) {
          conteudo.liked = false;
          conteudo.likes--;
        }
      }
    } catch (error) {
      console.error("Erro ao dar dislike:", error);
    }
  };

  const handlePostComment = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!id || !comentario.trim()) return;

    try {
      setIsPostingComment(true);
      await addComment.mutateAsync({
        id,
        data: { texto: comentario },
      });
      setComentario("");
    } catch (error) {
      alert("Erro ao postar comentário. Tente novamente.");
    } finally {
      setIsPostingComment(false);
    }
  };

  if (isLoading) {
    return (
      <div className="flex justify-center items-center min-h-[400px]">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!conteudo) {
    return (
      <Card className="max-w-4xl mx-auto">
        <h2 className="text-xl font-bold mb-2">Conteúdo não encontrado</h2>
        <Button onClick={() => navigate("/app/feed")}>
          Voltar para o feed
        </Button>
      </Card>
    );
  }

  const isVideo = conteudo.tipo === EConteudoTipo.Video;

  return (
    <div className="max-w-4xl mx-auto space-y-6">
      <Card>
        {isVideo && conteudo.caminhoVideo ? (
          <div className="mb-4">
            <video controls className="w-full rounded-lg">
              <source
                src={import.meta.env.VITE_FILES_URL + conteudo.caminhoVideo}
                type="video/mp4"
              />
              Seu navegador não suporta o elemento de vídeo.
            </video>
          </div>
        ) : (
          conteudo.caminhoImagem && (
            <img
              src={import.meta.env.VITE_FILES_URL + conteudo.caminhoImagem}
              alt={conteudo.titulo}
              className="w-full h-64 object-cover rounded-lg mb-4"
            />
          )
        )}

        <div className="mb-4">
          <h1 className="text-3xl font-bold text-black mb-3">
            {conteudo.titulo}
          </h1>

          <div className="flex items-center justify-between">
            <Link
              to={`/app/perfil/${conteudo.autor.id}`}
              className="flex items-center gap-3 hover:opacity-80 transition-smooth"
            >
              <Avatar
                src={conteudo.autor.caminhoImagem}
                alt={conteudo.autor.nome}
                size="md"
              />
              <div>
                <p className="font-semibold text-black">
                  {conteudo.autor.nome}
                </p>
                <p className="text-sm text-secondary">
                  {formatDate(conteudo.dataPublicacao)}
                </p>
              </div>
            </Link>

            <div className="flex items-center gap-4 text-secondary">
              <span className="flex items-center gap-1">
                <Eye size={20} />
                {conteudo.visualizacoes}
              </span>
            </div>
          </div>
        </div>

        <div className="mb-4">
          <span className="inline-block px-3 py-1 bg-primary/10 text-primary rounded-lg text-sm font-medium">
            {conteudo.categoria.nome}
          </span>
        </div>

        <div className="mb-4">
          <p className="text-secondary whitespace-pre-wrap">
            {conteudo.descricao}
          </p>
        </div>

        {!isVideo && conteudo.textoCompleto && (
          <div className="mb-4 p-4 bg-background rounded-lg">
            <h3 className="font-semibold text-black mb-2">Pergunta:</h3>
            <p className="text-black whitespace-pre-wrap">
              {conteudo.textoCompleto}
            </p>
          </div>
        )}

        <div className="flex items-center gap-4 pt-4 border-t border-gray-200">
          <button
            onClick={handleLike}
            disabled={toggleLike.isPending}
            className={`flex items-center gap-2 px-4 py-2 rounded-lg transition-smooth ${
              conteudo.liked
                ? "bg-primary text-white"
                : "bg-background text-black hover:bg-gray-300"
            }`}
          >
            <ThumbsUp
              size={20}
              fill={conteudo.liked ? "currentColor" : "none"}
            />
            <span>{conteudo.likes}</span>
          </button>

          <button
            onClick={handleDislike}
            disabled={toggleDislike.isPending}
            className={`flex items-center gap-2 px-4 py-2 rounded-lg transition-smooth ${
              conteudo.disliked
                ? "bg-inactive text-white"
                : "bg-background text-black hover:bg-gray-300"
            }`}
          >
            <ThumbsDown
              size={20}
              fill={conteudo.disliked ? "currentColor" : "none"}
            />
            <span>{conteudo.disikes}</span>
          </button>

          <div className="flex items-center gap-2 text-secondary ml-auto">
            <MessageCircle size={20} />
            <span>{conteudo.totalComentarios} comentários</span>
          </div>
        </div>
      </Card>

      <Card>
        <h2 className="text-2xl font-bold text-black mb-4">
          Comentários ({conteudo.totalComentarios})
        </h2>

        {currentUser && (
          <form onSubmit={handlePostComment} className="mb-6">
            <div className="flex gap-3">
              <Avatar
                src={currentUser.caminhoImagem}
                alt={currentUser.nome}
                size="md"
              />
              <div className="flex-1">
                <Input
                  value={comentario}
                  onChange={(e) => setComentario(e.target.value)}
                  placeholder="Adicione um comentário..."
                  disabled={isPostingComment}
                />
                <div className="flex gap-2 mt-2">
                  <Button
                    type="submit"
                    disabled={!comentario.trim() || isPostingComment}
                  >
                    {isPostingComment ? "Postando..." : "Comentar"}
                  </Button>
                  <Button
                    type="button"
                    variant="secondary"
                    onClick={() => setComentario("")}
                    disabled={isPostingComment}
                  >
                    Cancelar
                  </Button>
                </div>
              </div>
            </div>
          </form>
        )}

        <div className="space-y-4">
          {conteudo.comentarios && conteudo.comentarios.length > 0 ? (
            conteudo.comentarios.map((comentario) => (
              <div
                key={comentario.id}
                className="flex gap-3 p-4 bg-background rounded-lg"
              >
                <Link to={`/app/perfil/${comentario.autor.id}`}>
                  <Avatar
                    src={comentario.autor.caminhoImagem}
                    alt={comentario.autor.nome}
                    size="md"
                  />
                </Link>
                <div className="flex-1">
                  <div className="flex items-center gap-2 mb-1">
                    <Link
                      to={`/app/perfil/${comentario.autor.id}`}
                      className="font-semibold text-black hover:text-primary transition-smooth"
                    >
                      {comentario.autor.nome}
                    </Link>
                    <span className="text-sm text-secondary">
                      {formatDate(comentario.dataPublicacao)}
                    </span>
                  </div>
                  <p className="text-black whitespace-pre-wrap">
                    {comentario.texto}
                  </p>
                </div>
              </div>
            ))
          ) : (
            <p className="text-center text-secondary py-8">
              Nenhum comentário ainda. Seja o primeiro a comentar!
            </p>
          )}
        </div>
      </Card>
    </div>
  );
}
