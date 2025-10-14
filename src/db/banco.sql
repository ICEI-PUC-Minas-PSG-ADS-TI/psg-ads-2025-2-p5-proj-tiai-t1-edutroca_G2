CREATE DATABASE IF NOT EXISTS edutrocaDB
USE edutrocaDB;

CREATE TABLE usuario (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    senha VARCHAR(255) NOT NULL,
    foto_perfil VARCHAR(255),
    tipo_usuario ENUM('comum', 'admin', 'moderador') DEFAULT 'comum',
    classificacao DECIMAL(3,2) DEFAULT 0.00,
    bio TEXT,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    ativo TINYINT(1) DEFAULT 1,
    email_verificado TINYINT(1) DEFAULT 0,
    ultimo_acesso TIMESTAMP NULL,
    INDEX (email)
);

CREATE TABLE area_interesse (
    id_area INT AUTO_INCREMENT PRIMARY KEY,
    nome_area VARCHAR(100) NOT NULL,
    descricao TEXT,
    cor_hex VARCHAR(7),
    ativo TINYINT(1) DEFAULT 1,
    data_criacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE conteudo (
    id_conteudo INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_area INT NOT NULL,
    titulo VARCHAR(150) NOT NULL,
    descricao TEXT,
    tipo ENUM('video', 'artigo', 'podcast', 'curso') NOT NULL,
    url_video VARCHAR(255),
    duracao INT,
    data_publicacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    publico TINYINT(1) DEFAULT 1,
    ativo TINYINT(1) DEFAULT 1,
    visualizacoes INT DEFAULT 0,
    FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario),
    FOREIGN KEY (id_area) REFERENCES area_interesse(id_area),
    INDEX (id_usuario),
    INDEX (id_area)
);

CREATE TABLE comentario (
    id_comentario INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_conteudo INT NOT NULL,
    texto TEXT NOT NULL,
    data_comentario TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    ativo TINYINT(1) DEFAULT 1,
    FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario),
    FOREIGN KEY (id_conteudo) REFERENCES conteudo(id_conteudo)
);

CREATE TABLE interacao (
    id_interacao INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_conteudo INT NOT NULL,
    tipo_interacao ENUM('curtida', 'compartilhamento', 'salvo') NOT NULL,
    data_interacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario),
    FOREIGN KEY (id_conteudo) REFERENCES conteudo(id_conteudo)
);

CREATE TABLE preferencia_usuario (
    id_preferencia INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    tipo_conteudo_preferido ENUM('video', 'artigo', 'podcast', 'curso'),
    id_area INT,
    data_preferencia TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    nivel_interesse ENUM('baixo', 'medio', 'alto') DEFAULT 'medio',
    FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario),
    FOREIGN KEY (id_area) REFERENCES area_interesse(id_area)
);

CREATE TABLE recomendacao (
    id_recomendacao INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_conteudo INT NOT NULL,
    prioridade DECIMAL(5,2) DEFAULT 0.00,
    data_recomendacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    visualizada TINYINT(1) DEFAULT 0,
    FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario),
    FOREIGN KEY (id_conteudo) REFERENCES conteudo(id_conteudo)
);

CREATE TABLE recuperacao_senha (
    id_recuperacao INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    token VARCHAR(100) NOT NULL UNIQUE,
    data_solicitacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_expiracao TIMESTAMP NOT NULL,
    utilizado TINYINT(1) DEFAULT 0,
    FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario)
);

CREATE TABLE usuario_area_interesse (
    id_usuario_area INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_area INT NOT NULL,
    data_associacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario),
    FOREIGN KEY (id_area) REFERENCES area_interesse(id_area),
    UNIQUE (id_usuario, id_area)
);

CREATE TABLE visualizacao (
    id_visualizacao INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_conteudo INT NOT NULL,
    tempo_assistido INT DEFAULT 0,
    data_visualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    concluido TINYINT(1) DEFAULT 0,
    FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario),
    FOREIGN KEY (id_conteudo) REFERENCES conteudo(id_conteudo)
);
