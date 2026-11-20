-- Extension applicative : montant de référence de chaque inscription.
-- Compatible avec la base existante : ajout idempotent, sans modification des lignes.
USE dbGest_universite;
GO

IF COL_LENGTH('dbo.INSCRIPTION', 'montant_total') IS NULL
BEGIN
    ALTER TABLE dbo.INSCRIPTION
    ADD montant_total DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_INSCRIPTION_MONTANT_TOTAL DEFAULT (300000.00) WITH VALUES;
END
GO
