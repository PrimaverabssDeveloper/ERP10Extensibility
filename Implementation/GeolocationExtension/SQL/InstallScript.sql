IF dbo.STD_TableExists('ServidorMapas') = 0
BEGIN
	CREATE TABLE ServidorMapas
	(
			Nome	NVARCHAR(50)	NOT NULL
		,	Chave	VARCHAR(64)		NULL
		,	PRIMARY KEY(Nome)
	)
END
GO
--

IF dbo.STD_TableExists('Marcador') = 0
BEGIN
	CREATE TABLE Marcador
	(
			Id				UNIQUEIDENTIFIER	NOT NULL
		,	Latitude		DECIMAL(18, 15)		NOT NULL
		,	Longitude		DECIMAL(18, 15)		NOT NULL
		,	Utilizador		VARCHAR(20)			NOT NULL
		,	PontoInteresse	BIT					NOT NULL
		,	Descricao		NVARCHAR(200)		NULL
		,	PRIMARY KEY(Id)
	)

	ALTER TABLE Marcador ADD CONSTRAINT Marcador_DF					DEFAULT (NEWID())	FOR Id
	ALTER TABLE Marcador ADD CONSTRAINT Marcador_Latitude_DF		DEFAULT (0)			FOR Latitude
	ALTER TABLE Marcador ADD CONSTRAINT Marcador_Longitude_DF		DEFAULT (0)			FOR Longitude
	ALTER TABLE Marcador ADD CONSTRAINT Marcador_PontoInteresse_DF  DEFAULT (0)			FOR PontoInteresse
END
GO
--

IF dbo.STD_TableExists('Rota') = 0
BEGIN
	CREATE TABLE Rota
	(
			Id			UNIQUEIDENTIFIER	NOT NULL
		,	Utilizador	VARCHAR(20)			NOT NULL
		,	Nome		NVARCHAR(100)		NOT NULL
		,	PRIMARY KEY(Id)
	)

	ALTER TABLE Rota ADD CONSTRAINT Rota_DF DEFAULT (NEWID()) FOR Id
END
GO
--

IF dbo.STD_TableExists('MarcadorPorRota') = 0
BEGIN
	CREATE TABLE MarcadorPorRota
	(
			Id			UNIQUEIDENTIFIER	NOT NULL
		,	IdMarcador	UNIQUEIDENTIFIER	NOT NULL
		,	IdRota		UNIQUEIDENTIFIER	NOT NULL
		,	Ordem		TINYINT				NOT NULL
		,	PRIMARY KEY(Id)
	)

	ALTER TABLE MarcadorPorRota ADD CONSTRAINT MarcadorPorRota_DF		DEFAULT (NEWID())	FOR Id
	ALTER TABLE MarcadorPorRota ADD CONSTRAINT MarcadorPorRota_Ordem_DF DEFAULT (0)			FOR Ordem

	ALTER TABLE MarcadorPorRota WITH CHECK ADD CONSTRAINT FK_MarcadorPorRota_Marcador	FOREIGN KEY(IdMarcador) REFERENCES Marcador(Id)
	ALTER TABLE MarcadorPorRota CHECK CONSTRAINT FK_MarcadorPorRota_Marcador

	ALTER TABLE MarcadorPorRota WITH CHECK ADD CONSTRAINT FK_MarcadorPorRota_Rota		FOREIGN KEY(IdRota)		REFERENCES Rota(Id)
	ALTER TABLE MarcadorPorRota CHECK CONSTRAINT FK_MarcadorPorRota_Rota
END
GO
--

IF NOT EXISTS (SELECT TOP 1 1 FROM ServidorMapas)
BEGIN
	INSERT ServidorMapas(Nome, Chave) VALUES('Google', NULL)
	INSERT ServidorMapas(Nome, Chave) VALUES('Bing', NULL)
	INSERT ServidorMapas(Nome, Chave) VALUES('OpenStreetMap', NULL)
END
GO
--