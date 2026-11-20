# 🎓 IIT – Système de Gestion Universitaire
## Documentation Technique & Tableau Récapitulatif pour le Rapport

---

### 1. Architecture du Projet (N-Tiers)

Le projet a été restructuré en 3 projets interconnectés au sein de la solution `Sln_Gestion_Universite` :

1. **`Gestion_Universite` (Couche Présentation - WinForms)** :
   - Charte graphique IIT officielle : Rouge Crimson (`#8B0000` / `#A80C0C`), badges de statut arrondis, cartes blanches profilées.
   - Séparation stricte des formulaires en vue **Liste** et vue **Détail** pour toutes les entités nécessitant un affichage tabulaire et une gestion avancée (Étudiants, Inscriptions, Classes, Enseignants, etc.).
   - Navigation ergonomique via barre latérale et barre de titre personnalisée.

2. **`DAO` (Couche Métier & Objets d'Accès aux Données)** :
   - Modèles de données : `AnneeAcademique`, `Semestre`, `Filiere`, `Niveau`, `Classe`, `Etudiant`, `PieceAdministrative`, `Inscription`, `Paiement`, `Enseignant`, `Cours`, `Salle`, `SeanceEmploiDuTemps`, `Absence`, `DashboardStatistiques`.
   - Repositories avec contrôles métier :
     - Vérification de la double inscription active la même année.
     - Vérification des conflits d'emploi du temps (conflit de salle, d'enseignant ou de classe sur un même créneau).
   - Les contrôles de saisie vérifient les inscriptions, les paiements et les conflits de planning avant l'enregistrement.

3. **`DataAccess` (Couche d'Accès aux Données & Persistance)** :
   - `SqlServerDataStore` : persistance de l'application via SQL Server avec authentification Windows ; `IIT_SQLSERVER_CONNECTION_STRING` permet d'adapter la chaîne de connexion sur un autre poste.
   - `Database/dbGest_universite.sql` : script initial de SQL Server pour les 14 tables et procédures. L'application suppose que la base existe déjà et n'insère pas de fausses données au démarrage.
   - `Database/001_inscription_amount.sql` et `Database/002_absences.sql` : migrations additives et idempotentes pour les montants d'inscription et le bonus de suivi des absences.
   - `JsonDataStore` reste disponible comme stockage local de démonstration, mais le projet WinForms démarre avec SQL Server.

---

### 2. Tableau Récapitulatif des Modules (à insérer dans votre rapport)

| N° | Formulaire | Tables / Entités utilisées | Fonctionnalités & Règles Métier |
|:---|:---|:---|:---|
| **1** | **Menu principal (Accueil)** | `DashboardStatistiques`, `Inscription`, `Classe`, `Etudiant` | Point d'entrée de l'application. Affiche les indicateurs calculés depuis la base SQL Server et la navigation vers les modules. |
| **2** | **Années académiques et semestres** | `AnneeAcademique`, `Semestre` | Création et clôture d'années universitaires (ex. 2026 - 2027), dates de début/fin, rattachement de semestres (Semestre 1, Semestre 2) avec leurs périodes. Base temporelle de toutes les inscriptions et séances. |
| **3** | **Filières** | `Filiere` | Gestion des offres de formation (Informatique, Gestion commerciale, Finance comptabilité, Droit). Ajout, modification, activation/désactivation avec description. |
| **4** | **Niveaux** | `Niveau` | Gestion des niveaux académiques (L1, L2, L3, M1, M2) et rattachement aux cycles LMD (Licence, Master, Doctorat). |
| **5** | **Classes** *(Séparé en Liste & Détail)* | `Classe`, `Filiere`, `Niveau`, `AnneeAcademique` | **Partie Liste :** Affichage tabulaire avec code (`Info L2 A`), filière, niveau, groupe, détails complets et actions.<br>**Partie Détail :** Création / édition d'une classe en combinant filière, niveau, année académique et groupe. |
| **6** | **Étudiants** *(Séparé en Liste & Détail)* | `Etudiant`, `PieceAdministrative`, `Inscription` | **Partie Liste :** Recherche par nom ou matricule.<br>**Partie Détail :** Dossier avec identité, date de naissance, sexe, nationalité et coordonnées. La classe actuelle est déterminée par l'inscription ; gestion de la photo et des pièces administratives. |
| **7** | **Inscriptions** *(Séparé en Liste & Détail)* | `Inscription`, `Paiement`, `Etudiant`, `Classe`, `AnneeAcademique` | **Partie Liste :** Suivi général des inscriptions avec montants facturés, payés et soldes restants.<br>**Partie Détail :** Inscription d'un étudiant dans une classe pour une année donnée.<br>**Règle Métier : Blocage strict de la double inscription active pour un même étudiant sur la même année.**<br>Encaissement des paiements (montant FCFA, mode, référence), calcul dynamique du solde et impression du reçu officiel. |
| **8** | **Enseignants** *(Séparé en Liste & Détail)* | `Enseignant` | **Partie Liste :** Tableau récapitulatif du corps professoral avec spécialité et statut.<br>**Partie Détail :** Gestion des enseignants permanents et vacataires, coordonnées (téléphone, email), spécialité. |
| **9** | **Cours** | `Cours` | Gestion des matières dispensées (code, intitulé, description, volume horaire en heures). Table avec modification et suppression. |
| **10** | **Salles** | `Salle` | Gestion des salles de cours : nom, capacité, localisation et statuts Disponible / Indisponible, conformes aux contraintes SQL Server. |
| **11** | **Emplois du temps** *(Séparé en Grille & Détail séance)* | `SeanceEmploiDuTemps`, `Classe`, `Semestre`, `Cours`, `Enseignant`, `Salle` | **Partie Grille :** Sélection de la classe et du semestre, affichage par jour et créneau.<br>**Partie Détail séance :** Ajout / modification d'une séance avec date et heures.<br>Contrôle des chevauchements de salle, d'enseignant et de classe. |
| **12** | **Absences** *(bonus)* | `Absence`, `Etudiant`, `SeanceEmploiDuTemps`, `Inscription` | Enregistrement, modification, justification et suppression d'une absence. La séance doit être passée et l'étudiant inscrit activement dans la classe correspondante ; une absence par étudiant et séance. |

---

### 3. Exécution et Déploiement

- Ouvrez `Sln_Gestion_Universite.slnx` (ou `Sln_Gestion_Universite.sln`) sous **Visual Studio 2022 / 2026**.
- Définissez `Gestion_Universite` comme projet de démarrage.
- Lancez le projet (**F5**).
- Au démarrage, l'application vérifie l'accès à `dbGest_universite`, puis applique les deux migrations additives si nécessaire.
- Par défaut, la connexion cible `YHPC\SQLEXPRESS03` avec l'authentification Windows. Pour un autre poste, définir `IIT_SQLSERVER_CONNECTION_STRING` avec le nom du serveur et de la base.
- Le script initial `Database/dbGest_universite.sql` n'est pas réexécuté automatiquement et aucune donnée de démonstration n'est injectée dans la base existante.
