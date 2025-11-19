-- Criar o banco de dados
CREATE DATABASE GestaoOlhuz;
GO

USE GestaoOlhuz;
GO

-- 1. Criar a tabela Status
CREATE TABLE Status(
	StatusId INT PRIMARY KEY IDENTITY(1,1),
	Nome VARCHAR(20) NOT NULL UNIQUE,
	Descricao VARCHAR(255) NULL
);
GO

-- 2. Criar a tabela Usuarios
CREATE TABLE Usuarios(
	UsuarioId INT PRIMARY KEY IDENTITY(1,1),
	NomeCompleto VARCHAR(150) NOT NULL,
	Email VARCHAR(250) NOT NULL UNIQUE,
	PassWordHash VARCHAR(255) NOT NULL, -- Senha
	HashPass VARCHAR(255) NOT NULL, -- Senha criptografada para o backend
	VerificadorPass VARCHAR(8) NULL, -- Dupla verificacao de senha
	DataNascimento DATE NOT NULL,
	DataCriacao DATETIME DEFAULT GETDATE(),
	DataAtualizacao DATETIME NULL,
	StatusId INT NOT NULL,
);
GO

-- 3. Criar chave estrangeira para StatusId na tabela Usuarios
ALTER TABLE Usuarios
ADD CONSTRAINT FK_Usuarios_Status
FOREIGN KEY (StatusId) REFERENCES Status(StatusId);
GO
 