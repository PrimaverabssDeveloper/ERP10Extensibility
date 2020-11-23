DELETE FROM StdTabelasVar WHERE Tabela IN ('TDU_CabecAnomalias', 'TDU_LinhasAnomalias')
DELETE FROM StdCamposVar WHERE Tabela IN ('TDU_CabecAnomalias', 'TDU_LinhasAnomalias')

IF dbo.STD_TableExists('TDU_CabecAnomalias') = 1
	EXEC STD_DropTable [TDU_CabecAnomalias]
GO

CREATE TABLE TDU_CabecAnomalias
(
		CDU_Numero		bigint IDENTITY(1,1)	NOT NULL
	,	CDU_Data		datetime				NOT NULL	CONSTRAINT [TDU_CabecAnomalias_CDU_Data_DF]  DEFAULT (GETDATE())
	,	CDU_Utilizador	varchar(20)				NULL

	,	CONSTRAINT [TDU_CabecAnomalias_PK] PRIMARY KEY CLUSTERED
		(
			[CDU_Numero] ASC
		)
) ON [PRIMARY]
GO

IF dbo.STD_TableExists('TDU_LinhasAnomalias') = 1
	EXEC STD_DropTable [TDU_LinhasAnomalias]
GO

CREATE TABLE TDU_LinhasAnomalias
(
		CDU_Numero			bigint

	,	CDU_Artigo			nvarchar(48)		NOT NULL
	,	CDU_Lote			varchar(20)			NULL
	,	CDU_NumSerie		varchar(30)			NULL

	,	CDU_TipoEntidade	varchar(1)			NULL
	,	CDU_Entidade		varchar(12)			NULL

	,	CDU_TipoAnomalia	int					NULL
	,	CDU_Descricao		nvarchar(2000)		NULL
	,	CDU_Quantidade		float				NULL
	,	CDU_Unidade			nvarchar(5)			NULL

	,	CDU_Armazem			nvarchar(5)			NULL
	,	CDU_Localizacao		varchar(30)			NULL
	,	CDU_Estado			nvarchar(10)		NULL
	
	,	CONSTRAINT [TDU_LinhasAnomalias_PK] PRIMARY KEY CLUSTERED
		(
				[CDU_Numero] ASC
			,	[CDU_Artigo] ASC
		)
) ON [PRIMARY]
GO

INSERT INTO StdTabelasVar 
			SELECT 'TDU_CabecAnomalias', 'ERP'
	UNION	SELECT 'TDU_LinhasAnomalias', 'ERP'
GO

INSERT INTO StdCamposVar
			SELECT 'TDU_CabecAnomalias', 'CDU_Numero', 'Numero', 'Numero', 1, 1, NULL, NULL, NULL, 0, 0
	UNION	SELECT 'TDU_CabecAnomalias', 'CDU_Data', 'Data', 'Data', 1, 2, NULL, NULL, NULL, 0, 0
	UNION	SELECT 'TDU_CabecAnomalias', 'CDU_Utilizador', 'Utilizador', 'Utilizador', 1, 3, NULL, NULL, NULL, 0, 0
	UNION	SELECT 'TDU_CabecAnomalias', 'CDU_Operador', 'Operador', 'Operador', 1, 4, NULL, NULL, NULL, 0, 0
GO

INSERT INTO StdCamposVar
			SELECT 'TDU_LinhasAnomalias', 'CDU_Numero', 'Numero', 'Numero', 1, 1, NULL, NULL, NULL, 0, 0

	UNION	SELECT 'TDU_LinhasAnomalias', 'CDU_Artigo', 'Artigo', 'Artigo', 1, 2, NULL, NULL, NULL, 0, 0
	UNION	SELECT 'TDU_LinhasAnomalias', 'CDU_Lote', 'Lote', 'Lote', 1, 3, NULL, NULL, NULL, 0, 0
	UNION	SELECT 'TDU_LinhasAnomalias', 'CDU_NumSerie', 'NumSerie', 'NumSerie', 1, 4, NULL, NULL, NULL, 0, 0

	UNION	SELECT 'TDU_LinhasAnomalias', 'CDU_TipoEntidade', 'TipoEntidade', 'TipoEntidade', 1, 5, NULL, NULL, NULL, 0, 0
	UNION	SELECT 'TDU_LinhasAnomalias', 'CDU_Entidade', 'Entidade', 'Entidade', 1, 6, NULL, NULL, NULL, 0, 0

	UNION	SELECT 'TDU_LinhasAnomalias', 'CDU_TipoAnomalia', 'TipoAnomalia', 'TipoAnomalia', 1, 7, NULL, NULL, NULL, 0, 0
	UNION	SELECT 'TDU_LinhasAnomalias', 'CDU_Descricao', 'Descricao', 'Descricao', 1, 8, NULL, NULL, NULL, 0, 0
	UNION	SELECT 'TDU_LinhasAnomalias', 'CDU_Quantidade', 'Quantidade', 'Quantidade', 1, 9, NULL, NULL, NULL, 0, 0
	UNION	SELECT 'TDU_LinhasAnomalias', 'CDU_Unidade', 'Unidade', 'Unidade', 1, 10, NULL, NULL, NULL, 0, 0

	UNION	SELECT 'TDU_LinhasAnomalias', 'CDU_Armazem', 'Armazem', 'Armazem', 1, 11, NULL, NULL, NULL, 0, 0
	UNION	SELECT 'TDU_LinhasAnomalias', 'CDU_Localizacao', 'Localizacao', 'Localizacao', 1, 12, NULL, NULL, NULL, 0, 0
	UNION	SELECT 'TDU_LinhasAnomalias', 'CDU_Estado', 'Estado', 'Estado', 1, 13, NULL, NULL, NULL, 0, 0
	
GO