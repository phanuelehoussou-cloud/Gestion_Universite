-- ============================================================
-- BASE DE DONNÉES : GESTION D'UNE UNIVERSITÉ PRIVÉE (CÔTE D'IVOIRE)
-- SGBD cible : SQL Server
-- 14 tables + procédures stockées (CRUD + contrôles métier)
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'dbGest_universite')
BEGIN
    CREATE DATABASE dbGest_universite;
END
GO

USE dbGest_universite;
GO

-- ============================================================
-- MODULE 1 : GESTION DES INSCRIPTIONS
-- ============================================================

-- ------------------------------------------------------------
-- Table 3 : ANNEE_ACADEMIQUE (créée avant les tables qui la référencent)
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.ANNEE_ACADEMIQUE', 'U') IS NULL
BEGIN
CREATE TABLE dbo.ANNEE_ACADEMIQUE (
    id_annee    INT IDENTITY(1,1)  NOT NULL,
    libelle     VARCHAR(20)        NOT NULL,
    date_debut  DATE               NOT NULL,
    date_fin    DATE               NOT NULL,
    statut      VARCHAR(20)        NOT NULL DEFAULT 'OUVERTE',
    CONSTRAINT PK_ANNEE_ACADEMIQUE PRIMARY KEY (id_annee),
    CONSTRAINT UQ_ANNEE_LIBELLE UNIQUE (libelle),
    CONSTRAINT CK_ANNEE_DATES CHECK (date_fin > date_debut),
    CONSTRAINT CK_ANNEE_STATUT CHECK (statut IN ('OUVERTE', 'CLOTUREE', 'ARCHIVEE'))
);
END
GO

-- ------------------------------------------------------------
-- Table 1 : ETUDIANT
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.ETUDIANT', 'U') IS NULL
BEGIN
CREATE TABLE dbo.ETUDIANT (
    id_etudiant           INT IDENTITY(1,1)  NOT NULL,
    matricule             VARCHAR(30)        NOT NULL,
    nom                   VARCHAR(60)        NOT NULL,
    prenom                VARCHAR(60)        NOT NULL,
    date_naissance        DATE               NOT NULL,
    sexe                  CHAR(1)            NOT NULL,
    nationalite           VARCHAR(50)        NOT NULL,
    telephone             VARCHAR(20)        NULL,
    email                 VARCHAR(100)       NULL,
    adresse               VARCHAR(150)       NULL,
    date_creation_dossier DATETIME           NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_ETUDIANT PRIMARY KEY (id_etudiant),
    CONSTRAINT UQ_ETUDIANT_MATRICULE UNIQUE (matricule),
    CONSTRAINT CK_ETUDIANT_SEXE CHECK (sexe IN ('M', 'F'))
);
END
GO

-- ------------------------------------------------------------
-- Table 4 : FILIERE
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.FILIERE', 'U') IS NULL
BEGIN
CREATE TABLE dbo.FILIERE (
    id_filiere   INT IDENTITY(1,1)  NOT NULL,
    code_filiere VARCHAR(20)        NOT NULL,
    nom_filiere  VARCHAR(100)       NOT NULL,
    description  VARCHAR(255)       NULL,
    statut       VARCHAR(20)        NOT NULL DEFAULT 'ACTIVE',
    CONSTRAINT PK_FILIERE PRIMARY KEY (id_filiere),
    CONSTRAINT UQ_FILIERE_CODE UNIQUE (code_filiere),
    CONSTRAINT CK_FILIERE_STATUT CHECK (statut IN ('ACTIVE', 'SUSPENDUE', 'FERMEE'))
);
END
GO

-- ------------------------------------------------------------
-- Table 5 : NIVEAU
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.NIVEAU', 'U') IS NULL
BEGIN
CREATE TABLE dbo.NIVEAU (
    id_niveau   INT IDENTITY(1,1)  NOT NULL,
    code_niveau VARCHAR(10)        NOT NULL,
    libelle     VARCHAR(50)       NOT NULL,
    cycle       VARCHAR(20)       NOT NULL,
    CONSTRAINT PK_NIVEAU PRIMARY KEY (id_niveau),
    CONSTRAINT UQ_NIVEAU_CODE UNIQUE (code_niveau),
    CONSTRAINT CK_NIVEAU_CYCLE CHECK (cycle IN ('LICENCE', 'MASTER', 'DOCTORAT'))
);
END
GO

-- ------------------------------------------------------------
-- Table 6 : CLASSE
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.CLASSE', 'U') IS NULL
BEGIN
CREATE TABLE dbo.CLASSE (
    id_classe  INT IDENTITY(1,1)  NOT NULL,
    nom_classe VARCHAR(100)       NOT NULL,
    id_filiere INT                NOT NULL,
    id_niveau  INT                NOT NULL,
    id_annee   INT                NOT NULL,
    groupe     VARCHAR(10)        NULL,
    CONSTRAINT PK_CLASSE PRIMARY KEY (id_classe),
    CONSTRAINT FK_CLASSE_FILIERE FOREIGN KEY (id_filiere) REFERENCES dbo.FILIERE (id_filiere),
    CONSTRAINT FK_CLASSE_NIVEAU FOREIGN KEY (id_niveau) REFERENCES dbo.NIVEAU (id_niveau),
    CONSTRAINT FK_CLASSE_ANNEE FOREIGN KEY (id_annee) REFERENCES dbo.ANNEE_ACADEMIQUE (id_annee)
);
END
GO

-- ------------------------------------------------------------
-- Table 2 : INSCRIPTION
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.INSCRIPTION', 'U') IS NULL
BEGIN
CREATE TABLE dbo.INSCRIPTION (
    id_inscription   INT IDENTITY(1,1)  NOT NULL,
    id_etudiant      INT                NOT NULL,
    id_classe        INT                NOT NULL,
    id_annee         INT                NOT NULL,
    date_inscription DATETIME           NOT NULL DEFAULT GETDATE(),
    type_inscription VARCHAR(20)        NOT NULL,
    statut           VARCHAR(20)        NOT NULL DEFAULT 'ACTIVE',
    CONSTRAINT PK_INSCRIPTION PRIMARY KEY (id_inscription),
    CONSTRAINT FK_INSCRIPTION_ETUDIANT FOREIGN KEY (id_etudiant) REFERENCES dbo.ETUDIANT (id_etudiant),
    CONSTRAINT FK_INSCRIPTION_CLASSE FOREIGN KEY (id_classe) REFERENCES dbo.CLASSE (id_classe),
    CONSTRAINT FK_INSCRIPTION_ANNEE FOREIGN KEY (id_annee) REFERENCES dbo.ANNEE_ACADEMIQUE (id_annee),
    CONSTRAINT UQ_INSCRIPTION_ETUDIANT_ANNEE_ACTIVE UNIQUE (id_etudiant, id_annee, statut),
    CONSTRAINT CK_INSCRIPTION_TYPE CHECK (type_inscription IN ('PREMIERE', 'REINSCRIPTION')),
    CONSTRAINT CK_INSCRIPTION_STATUT CHECK (statut IN ('ACTIVE', 'ANNULEE', 'SUSPENDUE'))
);
END
GO

-- ------------------------------------------------------------
-- Table 7 : PAIEMENT
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.PAIEMENT', 'U') IS NULL
BEGIN
CREATE TABLE dbo.PAIEMENT (
    id_paiement        INT IDENTITY(1,1)  NOT NULL,
    id_inscription     INT                NOT NULL,
    montant            DECIMAL(18,2)      NOT NULL,
    date_paiement      DATETIME           NOT NULL DEFAULT GETDATE(),
    mode_paiement      VARCHAR(20)        NOT NULL,
    reference_paiement VARCHAR(50)        NULL,
    statut             VARCHAR(20)        NOT NULL DEFAULT 'EFFECTUE',
    CONSTRAINT PK_PAIEMENT PRIMARY KEY (id_paiement),
    CONSTRAINT FK_PAIEMENT_INSCRIPTION FOREIGN KEY (id_inscription) REFERENCES dbo.INSCRIPTION (id_inscription),
    CONSTRAINT CK_PAIEMENT_MONTANT CHECK (montant > 0),
    CONSTRAINT CK_PAIEMENT_MODE CHECK (mode_paiement IN ('ESPECES', 'MOBILE_MONEY', 'VIREMENT', 'CHEQUE')),
    CONSTRAINT CK_PAIEMENT_STATUT CHECK (statut IN ('EFFECTUE', 'EN_ATTENTE', 'ANNULE'))
);
END
GO

-- ------------------------------------------------------------
-- Table 8 : PIECE_ADMINISTRATIVE
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.PIECE_ADMINISTRATIVE', 'U') IS NULL
BEGIN
CREATE TABLE dbo.PIECE_ADMINISTRATIVE (
    id_piece          INT IDENTITY(1,1)  NOT NULL,
    id_etudiant       INT                NOT NULL,
    type_piece        VARCHAR(50)        NOT NULL,
    chemin_fichier     VARCHAR(255)       NOT NULL,
    date_depot        DATETIME           NOT NULL DEFAULT GETDATE(),
    statut_validation VARCHAR(20)        NOT NULL DEFAULT 'EN_ATTENTE',
    CONSTRAINT PK_PIECE_ADMINISTRATIVE PRIMARY KEY (id_piece),
    CONSTRAINT FK_PIECE_ETUDIANT FOREIGN KEY (id_etudiant) REFERENCES dbo.ETUDIANT (id_etudiant),
    CONSTRAINT CK_PIECE_STATUT CHECK (statut_validation IN ('VALIDEE', 'REJETEE', 'EN_ATTENTE'))
);
END
GO

-- ============================================================
-- MODULE 2 : PLANIFICATION DES COURS
-- ============================================================

-- ------------------------------------------------------------
-- Table 9 : ENSEIGNANT
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.ENSEIGNANT', 'U') IS NULL
BEGIN
CREATE TABLE dbo.ENSEIGNANT (
    id_enseignant        INT IDENTITY(1,1)  NOT NULL,
    matricule_enseignant VARCHAR(30)        NOT NULL,
    nom                  VARCHAR(60)        NOT NULL,
    prenom               VARCHAR(60)        NOT NULL,
    telephone            VARCHAR(20)        NULL,
    email                VARCHAR(100)       NULL,
    specialite           VARCHAR(100)       NULL,
    type_enseignant      VARCHAR(20)        NOT NULL DEFAULT 'PERMANENT',
    statut               VARCHAR(20)        NOT NULL DEFAULT 'ACTIF',
    CONSTRAINT PK_ENSEIGNANT PRIMARY KEY (id_enseignant),
    CONSTRAINT UQ_ENSEIGNANT_MATRICULE UNIQUE (matricule_enseignant),
    CONSTRAINT CK_ENSEIGNANT_TYPE CHECK (type_enseignant IN ('PERMANENT', 'VACATAIRE')),
    CONSTRAINT CK_ENSEIGNANT_STATUT CHECK (statut IN ('ACTIF', 'INACTIF'))
);
END
GO

-- ------------------------------------------------------------
-- Table 10 : COURS
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.COURS', 'U') IS NULL
BEGIN
CREATE TABLE dbo.COURS (
    id_cours       INT IDENTITY(1,1)  NOT NULL,
    code_cours     VARCHAR(20)        NOT NULL,
    nom_cours      VARCHAR(100)       NOT NULL,
    description    VARCHAR(255)       NULL,
    volume_horaire INT                NOT NULL,
    CONSTRAINT PK_COURS PRIMARY KEY (id_cours),
    CONSTRAINT UQ_COURS_CODE UNIQUE (code_cours),
    CONSTRAINT CK_COURS_VOLUME CHECK (volume_horaire > 0)
);
END
GO

-- ------------------------------------------------------------
-- Table 11 : SALLE
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.SALLE', 'U') IS NULL
BEGIN
CREATE TABLE dbo.SALLE (
    id_salle     INT IDENTITY(1,1)  NOT NULL,
    nom_salle    VARCHAR(50)        NOT NULL,
    capacite     INT                NOT NULL,
    localisation VARCHAR(100)       NULL,
    statut       VARCHAR(20)        NOT NULL DEFAULT 'DISPONIBLE',
    CONSTRAINT PK_SALLE PRIMARY KEY (id_salle),
    CONSTRAINT UQ_SALLE_NOM UNIQUE (nom_salle),
    CONSTRAINT CK_SALLE_CAPACITE CHECK (capacite > 0),
    CONSTRAINT CK_SALLE_STATUT CHECK (statut IN ('DISPONIBLE', 'INDISPONIBLE'))
);
END
GO

-- ------------------------------------------------------------
-- Table 12 : SEMESTRE
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.SEMESTRE', 'U') IS NULL
BEGIN
CREATE TABLE dbo.SEMESTRE (
    id_semestre INT IDENTITY(1,1)  NOT NULL,
    libelle     VARCHAR(30)        NOT NULL,
    id_annee    INT                NOT NULL,
    date_debut  DATE               NOT NULL,
    date_fin    DATE               NOT NULL,
    CONSTRAINT PK_SEMESTRE PRIMARY KEY (id_semestre),
    CONSTRAINT FK_SEMESTRE_ANNEE FOREIGN KEY (id_annee) REFERENCES dbo.ANNEE_ACADEMIQUE (id_annee),
    CONSTRAINT CK_SEMESTRE_DATES CHECK (date_fin > date_debut)
);
END
GO

-- ------------------------------------------------------------
-- Table 13 : EMPLOI_DU_TEMPS
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.EMPLOI_DU_TEMPS', 'U') IS NULL
BEGIN
CREATE TABLE dbo.EMPLOI_DU_TEMPS (
    id_emploi     INT IDENTITY(1,1)  NOT NULL,
    id_classe     INT                NOT NULL,
    id_semestre   INT                NOT NULL,
    date_creation DATETIME           NOT NULL DEFAULT GETDATE(),
    statut        VARCHAR(20)        NOT NULL DEFAULT 'BROUILLON',
    CONSTRAINT PK_EMPLOI_DU_TEMPS PRIMARY KEY (id_emploi),
    CONSTRAINT FK_EMPLOI_CLASSE FOREIGN KEY (id_classe) REFERENCES dbo.CLASSE (id_classe),
    CONSTRAINT FK_EMPLOI_SEMESTRE FOREIGN KEY (id_semestre) REFERENCES dbo.SEMESTRE (id_semestre),
    CONSTRAINT CK_EMPLOI_STATUT CHECK (statut IN ('BROUILLON', 'PUBLIE', 'ARCHIVE'))
);
END
GO

-- ------------------------------------------------------------
-- Table 14 : SEANCE
-- Règles 13, 14, 15 : pas de chevauchement salle / enseignant / classe
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.SEANCE', 'U') IS NULL
BEGIN
CREATE TABLE dbo.SEANCE (
    id_seance    INT IDENTITY(1,1)  NOT NULL,
    id_emploi    INT                NOT NULL,
    id_cours     INT                NOT NULL,
    id_enseignant INT                NOT NULL,
    id_salle     INT                NOT NULL,
    jour         VARCHAR(15)        NOT NULL,
    heure_debut  TIME               NOT NULL,
    heure_fin    TIME               NOT NULL,
    date_seance  DATE               NOT NULL,
    CONSTRAINT PK_SEANCE PRIMARY KEY (id_seance),
    CONSTRAINT FK_SEANCE_EMPLOI FOREIGN KEY (id_emploi) REFERENCES dbo.EMPLOI_DU_TEMPS (id_emploi),
    CONSTRAINT FK_SEANCE_COURS FOREIGN KEY (id_cours) REFERENCES dbo.COURS (id_cours),
    CONSTRAINT FK_SEANCE_ENSEIGNANT FOREIGN KEY (id_enseignant) REFERENCES dbo.ENSEIGNANT (id_enseignant),
    CONSTRAINT FK_SEANCE_SALLE FOREIGN KEY (id_salle) REFERENCES dbo.SALLE (id_salle),
    CONSTRAINT CK_SEANCE_HEURES CHECK (heure_fin > heure_debut),
    CONSTRAINT CK_SEANCE_JOUR CHECK (jour IN ('LUNDI','MARDI','MERCREDI','JEUDI','VENDREDI','SAMEDI'))
);
END
GO

-- Index de recherche pour les contrôles de chevauchement
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_SEANCE_SALLE')
    CREATE INDEX IX_SEANCE_SALLE ON dbo.SEANCE (id_salle, date_seance);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_SEANCE_ENSEIGNANT')
    CREATE INDEX IX_SEANCE_ENSEIGNANT ON dbo.SEANCE (id_enseignant, date_seance);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_SEANCE_EMPLOI')
    CREATE INDEX IX_SEANCE_EMPLOI ON dbo.SEANCE (id_emploi, date_seance);
GO

-- ============================================================
-- PROCÉDURES STOCKÉES
-- ============================================================

-- ------------------------------------------------------------
-- ANNEE_ACADEMIQUE
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_AnneeAcademique_Insert
    @libelle    VARCHAR(20),
    @date_debut DATE,
    @date_fin   DATE,
    @id_annee   INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF @date_fin <= @date_debut
    BEGIN
        RAISERROR('La date de fin doit être postérieure à la date de début.', 16, 1);
        RETURN;
    END
    INSERT INTO dbo.ANNEE_ACADEMIQUE (libelle, date_debut, date_fin)
    VALUES (@libelle, @date_debut, @date_fin);
    SET @id_annee = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_AnneeAcademique_Update
    @id_annee   INT,
    @libelle    VARCHAR(20),
    @date_debut DATE,
    @date_fin   DATE,
    @statut     VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @date_fin <= @date_debut
    BEGIN
        RAISERROR('La date de fin doit être postérieure à la date de début.', 16, 1);
        RETURN;
    END
    UPDATE dbo.ANNEE_ACADEMIQUE
       SET libelle = @libelle,
           date_debut = @date_debut,
           date_fin = @date_fin,
           statut = ISNULL(@statut, statut)
     WHERE id_annee = @id_annee;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_AnneeAcademique_Delete
    @id_annee INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.ANNEE_ACADEMIQUE WHERE id_annee = @id_annee;
END
GO

-- ------------------------------------------------------------
-- ETUDIANT
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Etudiant_Insert
    @matricule      VARCHAR(30),
    @nom            VARCHAR(60),
    @prenom         VARCHAR(60),
    @date_naissance DATE,
    @sexe           CHAR(1),
    @nationalite    VARCHAR(50),
    @telephone      VARCHAR(20)  = NULL,
    @email          VARCHAR(100) = NULL,
    @adresse        VARCHAR(150) = NULL,
    @id_etudiant    INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.ETUDIANT WHERE matricule = @matricule)
    BEGIN
        RAISERROR('Ce matricule existe déjà.', 16, 1);
        RETURN;
    END
    INSERT INTO dbo.ETUDIANT (matricule, nom, prenom, date_naissance, sexe, nationalite, telephone, email, adresse)
    VALUES (@matricule, @nom, @prenom, @date_naissance, @sexe, @nationalite, @telephone, @email, @adresse);
    SET @id_etudiant = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Etudiant_Update
    @id_etudiant    INT,
    @matricule      VARCHAR(30),
    @nom            VARCHAR(60),
    @prenom         VARCHAR(60),
    @date_naissance DATE,
    @sexe           CHAR(1),
    @nationalite    VARCHAR(50),
    @telephone      VARCHAR(20)  = NULL,
    @email          VARCHAR(100) = NULL,
    @adresse        VARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.ETUDIANT WHERE matricule = @matricule AND id_etudiant <> @id_etudiant)
    BEGIN
        RAISERROR('Ce matricule est déjà utilisé par un autre étudiant.', 16, 1);
        RETURN;
    END
    UPDATE dbo.ETUDIANT
       SET matricule = @matricule,
           nom = @nom,
           prenom = @prenom,
           date_naissance = @date_naissance,
           sexe = @sexe,
           nationalite = @nationalite,
           telephone = @telephone,
           email = @email,
           adresse = @adresse
     WHERE id_etudiant = @id_etudiant;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Etudiant_Delete
    @id_etudiant INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.INSCRIPTION WHERE id_etudiant = @id_etudiant)
    BEGIN
        RAISERROR('Impossible de supprimer : cet étudiant possède des inscriptions.', 16, 1);
        RETURN;
    END
    DELETE FROM dbo.ETUDIANT WHERE id_etudiant = @id_etudiant;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Etudiant_GetById
    @id_etudiant INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.ETUDIANT WHERE id_etudiant = @id_etudiant;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Etudiant_Search
    @critere VARCHAR(60) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.ETUDIANT
     WHERE @critere IS NULL
        OR matricule LIKE '%' + @critere + '%'
        OR nom LIKE '%' + @critere + '%'
        OR prenom LIKE '%' + @critere + '%';
END
GO

-- ------------------------------------------------------------
-- FILIERE
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Filiere_Insert
    @code_filiere VARCHAR(20),
    @nom_filiere  VARCHAR(100),
    @description  VARCHAR(255) = NULL,
    @id_filiere   INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.FILIERE (code_filiere, nom_filiere, description)
    VALUES (@code_filiere, @nom_filiere, @description);
    SET @id_filiere = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Filiere_Update
    @id_filiere   INT,
    @code_filiere VARCHAR(20),
    @nom_filiere  VARCHAR(100),
    @description  VARCHAR(255) = NULL,
    @statut       VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.FILIERE
       SET code_filiere = @code_filiere,
           nom_filiere = @nom_filiere,
           description = @description,
           statut = ISNULL(@statut, statut)
     WHERE id_filiere = @id_filiere;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Filiere_Delete
    @id_filiere INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.CLASSE WHERE id_filiere = @id_filiere)
    BEGIN
        RAISERROR('Impossible de supprimer : des classes sont rattachées à cette filière.', 16, 1);
        RETURN;
    END
    DELETE FROM dbo.FILIERE WHERE id_filiere = @id_filiere;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Filiere_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.FILIERE ORDER BY nom_filiere;
END
GO

-- ------------------------------------------------------------
-- NIVEAU
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Niveau_Insert
    @code_niveau VARCHAR(10),
    @libelle     VARCHAR(50),
    @cycle       VARCHAR(20),
    @id_niveau   INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.NIVEAU (code_niveau, libelle, cycle)
    VALUES (@code_niveau, @libelle, @cycle);
    SET @id_niveau = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Niveau_Update
    @id_niveau   INT,
    @code_niveau VARCHAR(10),
    @libelle     VARCHAR(50),
    @cycle       VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.NIVEAU
       SET code_niveau = @code_niveau, libelle = @libelle, cycle = @cycle
     WHERE id_niveau = @id_niveau;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Niveau_Delete
    @id_niveau INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.CLASSE WHERE id_niveau = @id_niveau)
    BEGIN
        RAISERROR('Impossible de supprimer : des classes utilisent ce niveau.', 16, 1);
        RETURN;
    END
    DELETE FROM dbo.NIVEAU WHERE id_niveau = @id_niveau;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Niveau_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.NIVEAU ORDER BY code_niveau;
END
GO

-- ------------------------------------------------------------
-- CLASSE
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Classe_Insert
    @nom_classe VARCHAR(100),
    @id_filiere INT,
    @id_niveau  INT,
    @id_annee   INT,
    @groupe     VARCHAR(10) = NULL,
    @id_classe  INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM dbo.FILIERE WHERE id_filiere = @id_filiere)
    BEGIN
        RAISERROR('Filière inexistante.', 16, 1); RETURN;
    END
    IF NOT EXISTS (SELECT 1 FROM dbo.NIVEAU WHERE id_niveau = @id_niveau)
    BEGIN
        RAISERROR('Niveau inexistant.', 16, 1); RETURN;
    END
    IF NOT EXISTS (SELECT 1 FROM dbo.ANNEE_ACADEMIQUE WHERE id_annee = @id_annee)
    BEGIN
        RAISERROR('Année académique inexistante.', 16, 1); RETURN;
    END
    INSERT INTO dbo.CLASSE (nom_classe, id_filiere, id_niveau, id_annee, groupe)
    VALUES (@nom_classe, @id_filiere, @id_niveau, @id_annee, @groupe);
    SET @id_classe = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Classe_Update
    @id_classe  INT,
    @nom_classe VARCHAR(100),
    @id_filiere INT,
    @id_niveau  INT,
    @id_annee   INT,
    @groupe     VARCHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.CLASSE
       SET nom_classe = @nom_classe,
           id_filiere = @id_filiere,
           id_niveau = @id_niveau,
           id_annee = @id_annee,
           groupe = @groupe
     WHERE id_classe = @id_classe;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Classe_Delete
    @id_classe INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.INSCRIPTION WHERE id_classe = @id_classe)
       OR EXISTS (SELECT 1 FROM dbo.EMPLOI_DU_TEMPS WHERE id_classe = @id_classe)
    BEGIN
        RAISERROR('Impossible de supprimer : classe utilisée par des inscriptions ou emplois du temps.', 16, 1);
        RETURN;
    END
    DELETE FROM dbo.CLASSE WHERE id_classe = @id_classe;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Classe_GetByAnnee
    @id_annee INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.*, f.nom_filiere, n.libelle AS niveau
      FROM dbo.CLASSE c
      JOIN dbo.FILIERE f ON f.id_filiere = c.id_filiere
      JOIN dbo.NIVEAU n ON n.id_niveau = c.id_niveau
     WHERE c.id_annee = @id_annee
     ORDER BY c.nom_classe;
END
GO

-- ------------------------------------------------------------
-- INSCRIPTION
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Inscription_Insert
    @id_etudiant      INT,
    @id_classe        INT,
    @id_annee         INT,
    @type_inscription VARCHAR(20),
    @id_inscription   INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Règle : empêcher une double inscription active d'un étudiant
    -- dans une même année académique
    IF EXISTS (SELECT 1
                 FROM dbo.INSCRIPTION
                WHERE id_etudiant = @id_etudiant
                  AND id_annee = @id_annee
                  AND statut = 'ACTIVE')
    BEGIN
        RAISERROR('Cet étudiant possède déjà une inscription active pour cette année académique.', 16, 1);
        RETURN;
    END

    -- Cohérence : la classe doit appartenir à la même année académique
    DECLARE @annee_classe INT;
    SELECT @annee_classe = id_annee FROM dbo.CLASSE WHERE id_classe = @id_classe;
    IF @annee_classe IS NULL
    BEGIN
        RAISERROR('Classe inexistante.', 16, 1); RETURN;
    END
    IF @annee_classe <> @id_annee
    BEGIN
        RAISERROR('L''année de la classe ne correspond pas à l''année de l''inscription.', 16, 1);
        RETURN;
    END

    INSERT INTO dbo.INSCRIPTION (id_etudiant, id_classe, id_annee, type_inscription)
    VALUES (@id_etudiant, @id_classe, @id_annee, @type_inscription);
    SET @id_inscription = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Inscription_Update
    @id_inscription   INT,
    @id_etudiant      INT,
    @id_classe        INT,
    @id_annee         INT,
    @type_inscription VARCHAR(20),
    @statut           VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.INSCRIPTION
       SET id_etudiant = @id_etudiant,
           id_classe = @id_classe,
           id_annee = @id_annee,
           type_inscription = @type_inscription,
           statut = @statut
     WHERE id_inscription = @id_inscription;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Inscription_Annuler
    @id_inscription INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.INSCRIPTION SET statut = 'ANNULEE' WHERE id_inscription = @id_inscription;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Inscription_Delete
    @id_inscription INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.PAIEMENT WHERE id_inscription = @id_inscription)
    BEGIN
        RAISERROR('Impossible de supprimer : des paiements sont rattachés à cette inscription.', 16, 1);
        RETURN;
    END
    DELETE FROM dbo.INSCRIPTION WHERE id_inscription = @id_inscription;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Inscription_GetByEtudiant
    @id_etudiant INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT i.*, c.nom_classe, a.libelle AS annee
      FROM dbo.INSCRIPTION i
      JOIN dbo.CLASSE c ON c.id_classe = i.id_classe
      JOIN dbo.ANNEE_ACADEMIQUE a ON a.id_annee = i.id_annee
     WHERE i.id_etudiant = @id_etudiant
     ORDER BY i.date_inscription DESC;
END
GO

-- ------------------------------------------------------------
-- PAIEMENT
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Paiement_Insert
    @id_inscription     INT,
    @montant            DECIMAL(18,2),
    @mode_paiement      VARCHAR(20),
    @reference_paiement VARCHAR(50) = NULL,
    @id_paiement        INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM dbo.INSCRIPTION WHERE id_inscription = @id_inscription)
    BEGIN
        RAISERROR('Inscription inexistante.', 16, 1); RETURN;
    END
    INSERT INTO dbo.PAIEMENT (id_inscription, montant, mode_paiement, reference_paiement)
    VALUES (@id_inscription, @montant, @mode_paiement, @reference_paiement);
    SET @id_paiement = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Paiement_Update
    @id_paiement        INT,
    @montant            DECIMAL(18,2),
    @mode_paiement      VARCHAR(20),
    @reference_paiement VARCHAR(50) = NULL,
    @statut             VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.PAIEMENT
       SET montant = @montant,
           mode_paiement = @mode_paiement,
           reference_paiement = @reference_paiement,
           statut = ISNULL(@statut, statut)
     WHERE id_paiement = @id_paiement;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Paiement_Delete
    @id_paiement INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.PAIEMENT WHERE id_paiement = @id_paiement;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Paiement_GetTotalParInscription
    @id_inscription INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ISNULL(SUM(montant), 0) AS total_paye
      FROM dbo.PAIEMENT
     WHERE id_inscription = @id_inscription
       AND statut = 'EFFECTUE';
END
GO

-- ------------------------------------------------------------
-- PIECE_ADMINISTRATIVE
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_PieceAdministrative_Insert
    @id_etudiant INT,
    @type_piece  VARCHAR(50),
    @chemin_fichier VARCHAR(255),
    @id_piece    INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.PIECE_ADMINISTRATIVE (id_etudiant, type_piece, chemin_fichier)
    VALUES (@id_etudiant, @type_piece, @chemin_fichier);
    SET @id_piece = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_PieceAdministrative_Valider
    @id_piece          INT,
    @statut_validation VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    IF @statut_validation NOT IN ('VALIDEE', 'REJETEE', 'EN_ATTENTE')
    BEGIN
        RAISERROR('Statut de validation invalide.', 16, 1); RETURN;
    END
    UPDATE dbo.PIECE_ADMINISTRATIVE
       SET statut_validation = @statut_validation
     WHERE id_piece = @id_piece;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_PieceAdministrative_Delete
    @id_piece INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.PIECE_ADMINISTRATIVE WHERE id_piece = @id_piece;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_PieceAdministrative_GetByEtudiant
    @id_etudiant INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.PIECE_ADMINISTRATIVE
     WHERE id_etudiant = @id_etudiant
     ORDER BY date_depot DESC;
END
GO

-- ------------------------------------------------------------
-- ENSEIGNANT
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Enseignant_Insert
    @matricule_enseignant VARCHAR(30),
    @nom                  VARCHAR(60),
    @prenom               VARCHAR(60),
    @telephone            VARCHAR(20)  = NULL,
    @email                VARCHAR(100) = NULL,
    @specialite           VARCHAR(100) = NULL,
    @type_enseignant      VARCHAR(20),
    @id_enseignant        INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.ENSEIGNANT WHERE matricule_enseignant = @matricule_enseignant)
    BEGIN
        RAISERROR('Ce matricule enseignant existe déjà.', 16, 1); RETURN;
    END
    INSERT INTO dbo.ENSEIGNANT (matricule_enseignant, nom, prenom, telephone, email, specialite, type_enseignant)
    VALUES (@matricule_enseignant, @nom, @prenom, @telephone, @email, @specialite, @type_enseignant);
    SET @id_enseignant = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Enseignant_Update
    @id_enseignant        INT,
    @matricule_enseignant VARCHAR(30),
    @nom                  VARCHAR(60),
    @prenom               VARCHAR(60),
    @telephone            VARCHAR(20)  = NULL,
    @email                VARCHAR(100) = NULL,
    @specialite           VARCHAR(100) = NULL,
    @type_enseignant      VARCHAR(20),
    @statut               VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.ENSEIGNANT
                WHERE matricule_enseignant = @matricule_enseignant
                  AND id_enseignant <> @id_enseignant)
    BEGIN
        RAISERROR('Ce matricule est déjà utilisé par un autre enseignant.', 16, 1); RETURN;
    END
    UPDATE dbo.ENSEIGNANT
       SET matricule_enseignant = @matricule_enseignant,
           nom = @nom,
           prenom = @prenom,
           telephone = @telephone,
           email = @email,
           specialite = @specialite,
           type_enseignant = @type_enseignant,
           statut = ISNULL(@statut, statut)
     WHERE id_enseignant = @id_enseignant;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Enseignant_Delete
    @id_enseignant INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.SEANCE WHERE id_enseignant = @id_enseignant)
    BEGIN
        RAISERROR('Impossible de supprimer : cet enseignant anime des séances.', 16, 1);
        RETURN;
    END
    DELETE FROM dbo.ENSEIGNANT WHERE id_enseignant = @id_enseignant;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Enseignant_Search
    @critere VARCHAR(60) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.ENSEIGNANT
     WHERE @critere IS NULL
        OR matricule_enseignant LIKE '%' + @critere + '%'
        OR nom LIKE '%' + @critere + '%'
        OR prenom LIKE '%' + @critere + '%'
        OR specialite LIKE '%' + @critere + '%';
END
GO

-- ------------------------------------------------------------
-- COURS
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Cours_Insert
    @code_cours     VARCHAR(20),
    @nom_cours      VARCHAR(100),
    @description    VARCHAR(255) = NULL,
    @volume_horaire INT,
    @id_cours       INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.COURS WHERE code_cours = @code_cours)
    BEGIN
        RAISERROR('Ce code de cours existe déjà.', 16, 1); RETURN;
    END
    INSERT INTO dbo.COURS (code_cours, nom_cours, description, volume_horaire)
    VALUES (@code_cours, @nom_cours, @description, @volume_horaire);
    SET @id_cours = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Cours_Update
    @id_cours       INT,
    @code_cours     VARCHAR(20),
    @nom_cours      VARCHAR(100),
    @description    VARCHAR(255) = NULL,
    @volume_horaire INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.COURS WHERE code_cours = @code_cours AND id_cours <> @id_cours)
    BEGIN
        RAISERROR('Ce code de cours est déjà utilisé.', 16, 1); RETURN;
    END
    UPDATE dbo.COURS
       SET code_cours = @code_cours,
           nom_cours = @nom_cours,
           description = @description,
           volume_horaire = @volume_horaire
     WHERE id_cours = @id_cours;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Cours_Delete
    @id_cours INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.SEANCE WHERE id_cours = @id_cours)
    BEGIN
        RAISERROR('Impossible de supprimer : ce cours est programmé dans des séances.', 16, 1);
        RETURN;
    END
    DELETE FROM dbo.COURS WHERE id_cours = @id_cours;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Cours_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.COURS ORDER BY nom_cours;
END
GO

-- ------------------------------------------------------------
-- SALLE
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Salle_Insert
    @nom_salle    VARCHAR(50),
    @capacite     INT,
    @localisation VARCHAR(100) = NULL,
    @id_salle     INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.SALLE (nom_salle, capacite, localisation)
    VALUES (@nom_salle, @capacite, @localisation);
    SET @id_salle = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Salle_Update
    @id_salle     INT,
    @nom_salle    VARCHAR(50),
    @capacite     INT,
    @localisation VARCHAR(100) = NULL,
    @statut       VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.SALLE
       SET nom_salle = @nom_salle,
           capacite = @capacite,
           localisation = @localisation,
           statut = ISNULL(@statut, statut)
     WHERE id_salle = @id_salle;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Salle_Delete
    @id_salle INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.SEANCE WHERE id_salle = @id_salle)
    BEGIN
        RAISERROR('Impossible de supprimer : des séances sont programmées dans cette salle.', 16, 1);
        RETURN;
    END
    DELETE FROM dbo.SALLE WHERE id_salle = @id_salle;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Salle_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.SALLE ORDER BY nom_salle;
END
GO

-- ------------------------------------------------------------
-- SEMESTRE
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Semestre_Insert
    @libelle    VARCHAR(30),
    @id_annee   INT,
    @date_debut DATE,
    @date_fin   DATE,
    @id_semestre INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF @date_fin <= @date_debut
    BEGIN
        RAISERROR('La date de fin doit être postérieure à la date de début.', 16, 1); RETURN;
    END
    INSERT INTO dbo.SEMESTRE (libelle, id_annee, date_debut, date_fin)
    VALUES (@libelle, @id_annee, @date_debut, @date_fin);
    SET @id_semestre = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Semestre_Update
    @id_semestre INT,
    @libelle     VARCHAR(30),
    @id_annee    INT,
    @date_debut  DATE,
    @date_fin    DATE
AS
BEGIN
    SET NOCOUNT ON;
    IF @date_fin <= @date_debut
    BEGIN
        RAISERROR('La date de fin doit être postérieure à la date de début.', 16, 1); RETURN;
    END
    UPDATE dbo.SEMESTRE
       SET libelle = @libelle,
           id_annee = @id_annee,
           date_debut = @date_debut,
           date_fin = @date_fin
     WHERE id_semestre = @id_semestre;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Semestre_Delete
    @id_semestre INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.EMPLOI_DU_TEMPS WHERE id_semestre = @id_semestre)
    BEGIN
        RAISERROR('Impossible de supprimer : des emplois du temps utilisent ce semestre.', 16, 1);
        RETURN;
    END
    DELETE FROM dbo.SEMESTRE WHERE id_semestre = @id_semestre;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Semestre_GetByAnnee
    @id_annee INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.SEMESTRE WHERE id_annee = @id_annee ORDER BY date_debut;
END
GO

-- ------------------------------------------------------------
-- EMPLOI_DU_TEMPS
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_EmploiDuTemps_Insert
    @id_classe   INT,
    @id_semestre INT,
    @id_emploi   INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM dbo.CLASSE WHERE id_classe = @id_classe)
    BEGIN
        RAISERROR('Classe inexistante.', 16, 1); RETURN;
    END
    IF NOT EXISTS (SELECT 1 FROM dbo.SEMESTRE WHERE id_semestre = @id_semestre)
    BEGIN
        RAISERROR('Semestre inexistant.', 16, 1); RETURN;
    END
    INSERT INTO dbo.EMPLOI_DU_TEMPS (id_classe, id_semestre)
    VALUES (@id_classe, @id_semestre);
    SET @id_emploi = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_EmploiDuTemps_Publier
    @id_emploi INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Vérifier qu'il contient au moins une séance avant publication
    IF NOT EXISTS (SELECT 1 FROM dbo.SEANCE WHERE id_emploi = @id_emploi)
    BEGIN
        RAISERROR('Impossible de publier un emploi du temps vide.', 16, 1); RETURN;
    END
    UPDATE dbo.EMPLOI_DU_TEMPS SET statut = 'PUBLIE' WHERE id_emploi = @id_emploi;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_EmploiDuTemps_Delete
    @id_emploi INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.SEANCE WHERE id_emploi = @id_emploi)
    BEGIN
        RAISERROR('Impossible de supprimer : des séances sont rattachées à cet emploi du temps.', 16, 1);
        RETURN;
    END
    DELETE FROM dbo.EMPLOI_DU_TEMPS WHERE id_emploi = @id_emploi;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_EmploiDuTemps_GetByClasseSemestre
    @id_classe   INT,
    @id_semestre INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.EMPLOI_DU_TEMPS
     WHERE id_classe = @id_classe AND id_semestre = @id_semestre;
END
GO

-- ------------------------------------------------------------
-- SEANCE (avec contrôle des règles 13, 14, 15)
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Seance_Insert
    @id_emploi    INT,
    @id_cours     INT,
    @id_enseignant INT,
    @id_salle     INT,
    @jour         VARCHAR(15),
    @heure_debut  TIME,
    @heure_fin    TIME,
    @date_seance  DATE,
    @id_seance    INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @heure_fin <= @heure_debut
    BEGIN
        RAISERROR('L''heure de fin doit être supérieure à l''heure de début.', 16, 1);
        RETURN;
    END

    DECLARE @id_classe INT;
    SELECT @id_classe = id_classe FROM dbo.EMPLOI_DU_TEMPS WHERE id_emploi = @id_emploi;
    IF @id_classe IS NULL
    BEGIN
        RAISERROR('Emploi du temps inexistant.', 16, 1); RETURN;
    END

    -- Règle 13 : la salle ne peut pas accueillir deux séances simultanées
    IF EXISTS (SELECT 1 FROM dbo.SEANCE s
                WHERE s.id_salle = @id_salle
                  AND s.date_seance = @date_seance
                  AND s.heure_debut < @heure_fin
                  AND s.heure_fin > @heure_debut)
    BEGIN
        RAISERROR('Règle 13 violée : cette salle est déjà occupée sur ce créneau.', 16, 1);
        RETURN;
    END

    -- Règle 14 : un enseignant ne peut pas dispenser deux cours au même moment
    IF EXISTS (SELECT 1 FROM dbo.SEANCE s
                WHERE s.id_enseignant = @id_enseignant
                  AND s.date_seance = @date_seance
                  AND s.heure_debut < @heure_fin
                  AND s.heure_fin > @heure_debut)
    BEGIN
        RAISERROR('Règle 14 violée : cet enseignant est déjà occupé sur ce créneau.', 16, 1);
        RETURN;
    END

    -- Règle 15 : une classe ne peut pas avoir deux séances simultanées
    IF EXISTS (SELECT 1 FROM dbo.SEANCE s
                JOIN dbo.EMPLOI_DU_TEMPS e ON e.id_emploi = s.id_emploi
                WHERE e.id_classe = @id_classe
                  AND s.date_seance = @date_seance
                  AND s.heure_debut < @heure_fin
                  AND s.heure_fin > @heure_debut)
    BEGIN
        RAISERROR('Règle 15 violée : cette classe a déjà une séance sur ce créneau.', 16, 1);
        RETURN;
    END

    INSERT INTO dbo.SEANCE (id_emploi, id_cours, id_enseignant, id_salle, jour, heure_debut, heure_fin, date_seance)
    VALUES (@id_emploi, @id_cours, @id_enseignant, @id_salle, @jour, @heure_debut, @heure_fin, @date_seance);
    SET @id_seance = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Seance_Update
    @id_seance    INT,
    @id_cours     INT,
    @id_enseignant INT,
    @id_salle     INT,
    @jour         VARCHAR(15),
    @heure_debut  TIME,
    @heure_fin    TIME,
    @date_seance  DATE
AS
BEGIN
    SET NOCOUNT ON;

    IF @heure_fin <= @heure_debut
    BEGIN
        RAISERROR('L''heure de fin doit être supérieure à l''heure de début.', 16, 1);
        RETURN;
    END

    DECLARE @id_emploi INT, @id_classe INT;
    SELECT @id_emploi = id_emploi FROM dbo.SEANCE WHERE id_seance = @id_seance;
    SELECT @id_classe = id_classe FROM dbo.EMPLOI_DU_TEMPS WHERE id_emploi = @id_emploi;

    IF EXISTS (SELECT 1 FROM dbo.SEANCE s
                WHERE s.id_salle = @id_salle
                  AND s.date_seance = @date_seance
                  AND s.heure_debut < @heure_fin
                  AND s.heure_fin > @heure_debut
                  AND s.id_seance <> @id_seance)
    BEGIN
        RAISERROR('Règle 13 violée : cette salle est déjà occupée sur ce créneau.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM dbo.SEANCE s
                WHERE s.id_enseignant = @id_enseignant
                  AND s.date_seance = @date_seance
                  AND s.heure_debut < @heure_fin
                  AND s.heure_fin > @heure_debut
                  AND s.id_seance <> @id_seance)
    BEGIN
        RAISERROR('Règle 14 violée : cet enseignant est déjà occupé sur ce créneau.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM dbo.SEANCE s
                JOIN dbo.EMPLOI_DU_TEMPS e ON e.id_emploi = s.id_emploi
                WHERE e.id_classe = @id_classe
                  AND s.date_seance = @date_seance
                  AND s.heure_debut < @heure_fin
                  AND s.heure_fin > @heure_debut
                  AND s.id_seance <> @id_seance)
    BEGIN
        RAISERROR('Règle 15 violée : cette classe a déjà une séance sur ce créneau.', 16, 1);
        RETURN;
    END

    UPDATE dbo.SEANCE
       SET id_cours = @id_cours,
           id_enseignant = @id_enseignant,
           id_salle = @id_salle,
           jour = @jour,
           heure_debut = @heure_debut,
           heure_fin = @heure_fin,
           date_seance = @date_seance
     WHERE id_seance = @id_seance;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Seance_Delete
    @id_seance INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.SEANCE WHERE id_seance = @id_seance;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Seance_GetByEmploi
    @id_emploi INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT s.*,
           c.nom_cours,
           en.nom + ' ' + en.prenom AS enseignant,
           sa.nom_salle
      FROM dbo.SEANCE s
      JOIN dbo.COURS c ON c.id_cours = s.id_cours
      JOIN dbo.ENSEIGNANT en ON en.id_enseignant = s.id_enseignant
      JOIN dbo.SALLE sa ON sa.id_salle = s.id_salle
     WHERE s.id_emploi = @id_emploi
     ORDER BY s.date_seance, s.heure_debut;
END
GO

-- ------------------------------------------------------------
-- Procédures de reporting
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Etudiant_GetByClasse
    @id_classe INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.matricule, e.nom, e.prenom, e.sexe, e.telephone, e.email,
           i.type_inscription, i.date_inscription, i.statut
      FROM dbo.INSCRIPTION i
      JOIN dbo.ETUDIANT e ON e.id_etudiant = i.id_etudiant
     WHERE i.id_classe = @id_classe
       AND i.statut = 'ACTIVE'
     ORDER BY e.nom, e.prenom;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Paiement_GetTotalParAnnee
    @id_annee INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT SUM(p.montant) AS total_encaisse
      FROM dbo.PAIEMENT p
      JOIN dbo.INSCRIPTION i ON i.id_inscription = p.id_inscription
     WHERE i.id_annee = @id_annee
       AND p.statut = 'EFFECTUE';
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_EmploiDuTemps_GetByClasseSemaine
    @id_classe   INT,
    @id_semestre INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.id_emploi, s.jour, s.heure_debut, s.heure_fin,
           c.nom_cours, en.nom + ' ' + en.prenom AS enseignant, sa.nom_salle
      FROM dbo.EMPLOI_DU_TEMPS e
      JOIN dbo.SEANCE s ON s.id_emploi = e.id_emploi
      JOIN dbo.COURS c ON c.id_cours = s.id_cours
      JOIN dbo.ENSEIGNANT en ON en.id_enseignant = s.id_enseignant
      JOIN dbo.SALLE sa ON sa.id_salle = s.id_salle
     WHERE e.id_classe = @id_classe
       AND e.id_semestre = @id_semestre
       AND e.statut = 'PUBLIE'
     ORDER BY s.jour, s.heure_debut;
END
GO