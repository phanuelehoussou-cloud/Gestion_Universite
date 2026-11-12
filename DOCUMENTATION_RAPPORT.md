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
   - Modèles de données : `AnneeAcademique`, `Semestre`, `Filiere`, `Niveau`, `Classe`, `Etudiant`, `PieceAdministrative`, `Inscription`, `Paiement`, `Enseignant`, `Cours`, `Salle`, `SeanceEmploiDuTemps`, `DashboardStatistiques`.
   - Repositories avec contrôles métier :
     - Vérification de la double inscription active la même année.
     - Vérification des conflits d'emploi du temps (conflit de salle, d'enseignant ou de classe sur un même créneau).
   - `DataSeeder` : injection automatique de données de démonstration conformes aux maquettes (1 248 étudiants, 12 classes, 2 850 000 FCFA, etc.).

3. **`DataAccess` (Couche d'Accès aux Données & Persistance)** :
   - `JsonDataStore` : Moteur de stockage relationnel transactionnel sans configuration, permettant à l'application de fonctionner immédiatement sans installation préalable d'un serveur de base de données.
   - `DbHelper` & `script_creation_bdd.sql` : Script SQL complet généré automatiquement pour le déploiement sur SQL Server ou MySQL.

---

### 2. Tableau Récapitulatif des 11 Formulaires (à insérer dans votre rapport)

| N° | Formulaire | Tables / Entités utilisées | Fonctionnalités & Règles Métier |
|:---|:---|:---|:---|
| **1** | **Menu principal (Accueil)** | `DashboardStatistiques`, `Inscription`, `Classe`, `Etudiant` | Point d'entrée de l'application. Affiche les indicateurs clés (1 248 étudiants inscrits, 12 classes actives, 2 850 000 FCFA de recettes du jour). Navigation vers tous les modules. |
| **2** | **Années académiques et semestres** | `AnneeAcademique`, `Semestre` | Création et clôture d'années universitaires (ex. 2026 - 2027), dates de début/fin, rattachement de semestres (Semestre 1, Semestre 2) avec leurs périodes. Base temporelle de toutes les inscriptions et séances. |
| **3** | **Filières** | `Filiere` | Gestion des offres de formation (Informatique, Gestion commerciale, Finance comptabilité, Droit). Ajout, modification, activation/désactivation avec description. |
| **4** | **Niveaux** | `Niveau` | Gestion des niveaux académiques (L1, L2, L3, M1, M2) et rattachement aux cycles LMD (Licence, Master, Doctorat). |
| **5** | **Classes** *(Séparé en Liste & Détail)* | `Classe`, `Filiere`, `Niveau`, `AnneeAcademique` | **Partie Liste :** Affichage tabulaire avec code (`Info L2 A`), filière, niveau, groupe, détails complets et actions.<br>**Partie Détail :** Création / édition d'une classe en combinant filière, niveau, année académique et groupe. |
| **6** | **Étudiants** *(Séparé en Liste & Détail)* | `Etudiant`, `PieceAdministrative`, `Classe` | **Partie Liste :** Recherche en temps réel par nom ou matricule, badges de statut, actions rapides.<br>**Partie Détail :** Dossier complet (matricule généré, identité, téléphone, adresse, email, affectation classe). Gestion de la photo et pop-up de validation des pièces administratives (extrait de naissance, diplôme, certificat). |
| **7** | **Inscriptions** *(Séparé en Liste & Détail)* | `Inscription`, `Paiement`, `Etudiant`, `Classe`, `AnneeAcademique` | **Partie Liste :** Suivi général des inscriptions avec montants facturés, payés et soldes restants.<br>**Partie Détail :** Inscription d'un étudiant dans une classe pour une année donnée.<br>**Règle Métier : Blocage strict de la double inscription active pour un même étudiant sur la même année.**<br>Encaissement des paiements (montant FCFA, mode, référence), calcul dynamique du solde et impression du reçu officiel. |
| **8** | **Enseignants** *(Séparé en Liste & Détail)* | `Enseignant` | **Partie Liste :** Tableau récapitulatif du corps professoral avec spécialité et statut.<br>**Partie Détail :** Gestion des enseignants permanents et vacataires, coordonnées (téléphone, email), spécialité. |
| **9** | **Cours** | `Cours` | Gestion des matières dispensées (code, intitulé, description, volume horaire en heures). Table avec modification et suppression. |
| **10** | **Salles** | `Salle` | Gestion des salles de cours : nom (S101, S102, S201), capacité d'accueil, localisation (Bâtiment A, B, etc.) et statuts visuels (Disponible en vert, En maintenance en orange). |
| **11** | **Emplois du temps** *(Séparé en Grille & Détail séance)* | `SeanceEmploiDuTemps`, `Classe`, `Semestre`, `Cours`, `Enseignant`, `Salle` | **Partie Grille :** Sélection de la classe et du semestre, bouton Générer, affichage par jour et créneau.<br>**Partie Détail séance :** Ajout / modification d'une séance de cours.<br>**Règle Métier Critique : Détection préalable de 3 types de conflits avant enregistrement :**<br>1. *Conflit de salle* (salle déjà réservée sur ce créneau).<br>2. *Conflit d'enseignant* (enseignant déjà en cours ailleurs).<br>3. *Conflit de classe* (classe ayant déjà un cours programmé). |

---

### 3. Exécution et Déploiement

- Ouvrez `Sln_Gestion_Universite.slnx` (ou `Sln_Gestion_Universite.sln`) sous **Visual Studio 2022 / 2026**.
- Définissez `Gestion_Universite` comme projet de démarrage.
- Lancez le projet (**F5**).
- Les données initiales de test et de présentation s'initialisent automatiquement dès le premier lancement.
