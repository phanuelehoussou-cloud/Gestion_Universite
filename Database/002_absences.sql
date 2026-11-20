-- Extension facultative : suivi des absences par étudiant et séance.
-- À exécuter après dbGest_universite.sql dans la base dbGest_universite.
USE dbGest_universite;
GO

IF OBJECT_ID('dbo.ABSENCE', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ABSENCE
    (
        id_absence INT IDENTITY(1,1) NOT NULL,
        id_etudiant INT NOT NULL,
        id_seance INT NOT NULL,
        date_absence DATE NOT NULL,
        statut_justification VARCHAR(20) NOT NULL CONSTRAINT DF_ABSENCE_STATUT DEFAULT 'NON_JUSTIFIEE',
        motif VARCHAR(255) NULL,
        date_enregistrement DATETIME NOT NULL CONSTRAINT DF_ABSENCE_DATE DEFAULT GETDATE(),
        CONSTRAINT PK_ABSENCE PRIMARY KEY (id_absence),
        CONSTRAINT FK_ABSENCE_ETUDIANT FOREIGN KEY (id_etudiant) REFERENCES dbo.ETUDIANT (id_etudiant),
        CONSTRAINT FK_ABSENCE_SEANCE FOREIGN KEY (id_seance) REFERENCES dbo.SEANCE (id_seance),
        CONSTRAINT UQ_ABSENCE_ETUDIANT_SEANCE UNIQUE (id_etudiant, id_seance),
        CONSTRAINT CK_ABSENCE_STATUT CHECK (statut_justification IN ('JUSTIFIEE', 'NON_JUSTIFIEE'))
    );
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Absence_Insert
    @id_etudiant INT,
    @id_seance INT,
    @date_absence DATE,
    @statut_justification VARCHAR(20),
    @motif VARCHAR(255) = NULL,
    @id_absence INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.ABSENCE (id_etudiant, id_seance, date_absence, statut_justification, motif)
    VALUES (@id_etudiant, @id_seance, @date_absence, @statut_justification, @motif);
    SET @id_absence = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Absence_Update
    @id_absence INT,
    @id_etudiant INT,
    @id_seance INT,
    @date_absence DATE,
    @statut_justification VARCHAR(20),
    @motif VARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.ABSENCE
       SET id_etudiant = @id_etudiant,
           id_seance = @id_seance,
           date_absence = @date_absence,
           statut_justification = @statut_justification,
           motif = @motif
     WHERE id_absence = @id_absence;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Absence_Delete
    @id_absence INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.ABSENCE WHERE id_absence = @id_absence;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Absence_GetByDate
    @date_absence DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT a.id_absence, a.date_absence, a.statut_justification, a.motif,
           e.id_etudiant, e.matricule, e.nom, e.prenom,
           s.id_seance, s.id_emploi, s.jour, s.heure_debut, s.heure_fin,
           c.nom_classe, co.nom_cours
      FROM dbo.ABSENCE a
      JOIN dbo.ETUDIANT e ON e.id_etudiant = a.id_etudiant
      JOIN dbo.SEANCE s ON s.id_seance = a.id_seance
      JOIN dbo.EMPLOI_DU_TEMPS edt ON edt.id_emploi = s.id_emploi
      JOIN dbo.CLASSE c ON c.id_classe = edt.id_classe
      JOIN dbo.COURS co ON co.id_cours = s.id_cours
     WHERE a.date_absence = @date_absence
     ORDER BY e.nom, e.prenom;
END
GO
