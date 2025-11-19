USE GestaoOlhuz;
GO

-- Inserção dos status básicos

-- Habilita a inserção manual de IDs, já que StatusId é IDENTITY
SET IDENTITY_INSERT dbo.Status ON;
GO
 
-- Insere os status básicos se eles ainda não existirem
IF NOT EXISTS (SELECT 1 FROM dbo.Status WHERE StatusId = 1)
BEGIN
	INSERT INTO Status (StatusId, Nome, Descricao)
	VALUES (1, 'Inativo', 'Usuário desabilitado pelo administrador');
END

-- A instrução UPDATE a seguir garante que o Nome e Descricao estejam corretos
-- UPDATE Status SET Nome = 'Inativo', Descricao = 'Usuário desabilitado pelo administrador' WHERE StatusId = 1;

IF NOT EXISTS (SELECT 1 FROM dbo.Status WHERE StatusId = 2) -- Corrigido para verificar a existência da linha (SELECT 1)
BEGIN
	INSERT INTO Status (StatusId, Nome, Descricao)
	VALUES (2, 'Pendente', 'Usuário aguardando validação de email');
END

IF NOT EXISTS (SELECT 1 FROM dbo.Status WHERE StatusId = 3) -- Corrigido para verificar a existência da linha (SELECT 1)
BEGIN
 
	INSERT INTO Status (StatusId, Nome, Descricao)
	VALUES (3, 'Ativo', 'Usuário pode logar no sistema');
 
END

-- Atualiza a linha com StatusId = 1 (se a inserção acima não foi executada ou para garantir a consistência)
UPDATE Status

SET 
	Nome = 'Inativo',
	Descricao = 'Usuário desabilitado pelo administrador'
WHERE 
	StatusId = 1;

-- Desabilita a inserção manual de IDs
SET IDENTITY_INSERT dbo.Status OFF;
GO

-- Seleções para verificação
SELECT * FROM Status;
SELECT * FROM Usuarios;
GO