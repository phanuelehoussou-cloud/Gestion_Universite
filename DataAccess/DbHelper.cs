using System.Data;
using System.Data.Common;

namespace DataAccess;

public static class DbHelper
{
    public const string CreationScript = @"
-- ============================================================
-- SCRIPT DE CRÉATION DE LA BASE DE DONNÉES : IIT_Gestion_Universite
-- Système de Gestion Universitaire IIT
-- ============================================================

CREATE TABLE IF NOT EXISTS AnneeAcademique (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Code VARCHAR(50) NOT NULL UNIQUE,
    DateDebut DATE NOT NULL,
    DateFin DATE NOT NULL,
    Statut VARCHAR(20) DEFAULT 'Ouverte'
);

CREATE TABLE IF NOT EXISTS Semestre (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    AnneeAcademiqueId INT NOT NULL,
    Nom VARCHAR(50) NOT NULL,
    DateDebut DATE NOT NULL,
    DateFin DATE NOT NULL,
    FOREIGN KEY (AnneeAcademiqueId) REFERENCES AnneeAcademique(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Filiere (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nom VARCHAR(100) NOT NULL UNIQUE,
    Description TEXT,
    Statut VARCHAR(20) DEFAULT 'Active'
);

CREATE TABLE IF NOT EXISTS Niveau (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Code VARCHAR(20) NOT NULL UNIQUE,
    Cycle VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS Classe (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Code VARCHAR(50) NOT NULL UNIQUE,
    FiliereId INT NOT NULL,
    NiveauId INT NOT NULL,
    AnneeAcademiqueId INT NOT NULL,
    Groupe VARCHAR(10) DEFAULT 'A',
    Details VARCHAR(255),
    FOREIGN KEY (FiliereId) REFERENCES Filiere(Id),
    FOREIGN KEY (NiveauId) REFERENCES Niveau(Id),
    FOREIGN KEY (AnneeAcademiqueId) REFERENCES AnneeAcademique(Id)
);

CREATE TABLE IF NOT EXISTS Etudiant (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Matricule VARCHAR(50) NOT NULL UNIQUE,
    Nom VARCHAR(100) NOT NULL,
    Prenoms VARCHAR(150) NOT NULL,
    Telephone VARCHAR(50),
    Email VARCHAR(150),
    Adresse VARCHAR(255),
    PhotoPath VARCHAR(255),
    Statut VARCHAR(20) DEFAULT 'Actif'
);

CREATE TABLE IF NOT EXISTS PieceAdministrative (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    EtudiantId INT NOT NULL,
    TypePiece VARCHAR(100) NOT NULL,
    FichierPath VARCHAR(255),
    EstValidee BOOLEAN DEFAULT FALSE,
    DateDepot DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (EtudiantId) REFERENCES Etudiant(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Inscription (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    EtudiantId INT NOT NULL,
    ClasseId INT NOT NULL,
    AnneeAcademiqueId INT NOT NULL,
    DateInscription DATETIME DEFAULT CURRENT_TIMESTAMP,
    MontantTotal DECIMAL(18,2) DEFAULT 300000.00,
    Statut VARCHAR(20) DEFAULT 'Active',
    UNIQUE (EtudiantId, AnneeAcademiqueId), -- Règle : Bloquer la double inscription active la même année
    FOREIGN KEY (EtudiantId) REFERENCES Etudiant(Id),
    FOREIGN KEY (ClasseId) REFERENCES Classe(Id),
    FOREIGN KEY (AnneeAcademiqueId) REFERENCES AnneeAcademique(Id)
);

CREATE TABLE IF NOT EXISTS Paiement (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    InscriptionId INT NOT NULL,
    DatePaiement DATETIME DEFAULT CURRENT_TIMESTAMP,
    Montant DECIMAL(18,2) NOT NULL,
    ModePaiement VARCHAR(50) NOT NULL,
    Reference VARCHAR(100),
    Remarque VARCHAR(255),
    FOREIGN KEY (InscriptionId) REFERENCES Inscription(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Enseignant (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nom VARCHAR(100) NOT NULL,
    Prenoms VARCHAR(150) NOT NULL,
    Specialite VARCHAR(100) NOT NULL,
    TypeEnseignant VARCHAR(50) DEFAULT 'Permanent',
    Telephone VARCHAR(50),
    Email VARCHAR(150),
    Statut VARCHAR(20) DEFAULT 'Actif'
);

CREATE TABLE IF NOT EXISTS Cours (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Code VARCHAR(50) NOT NULL UNIQUE,
    Intitule VARCHAR(150) NOT NULL,
    Description TEXT,
    VolumeHoraire INT DEFAULT 45
);

CREATE TABLE IF NOT EXISTS Salle (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nom VARCHAR(50) NOT NULL UNIQUE,
    Capacite INT DEFAULT 50,
    Localisation VARCHAR(100),
    Statut VARCHAR(50) DEFAULT 'Disponible'
);

CREATE TABLE IF NOT EXISTS SeanceEmploiDuTemps (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    ClasseId INT NOT NULL,
    SemestreId INT NOT NULL,
    CoursId INT NOT NULL,
    EnseignantId INT NOT NULL,
    SalleId INT NOT NULL,
    Jour VARCHAR(20) NOT NULL,
    HeureDebut VARCHAR(20) NOT NULL,
    HeureFin VARCHAR(20) NOT NULL,
    FOREIGN KEY (ClasseId) REFERENCES Classe(Id),
    FOREIGN KEY (SemestreId) REFERENCES Semestre(Id),
    FOREIGN KEY (CoursId) REFERENCES Cours(Id),
    FOREIGN KEY (EnseignantId) REFERENCES Enseignant(Id),
    FOREIGN KEY (SalleId) REFERENCES Salle(Id)
);
";

    public static void GenerateScriptFile(string targetPath)
    {
        try
        {
            File.WriteAllText(targetPath, CreationScript);
        }
        catch { }
    }
}
