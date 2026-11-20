using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormSeanceDetail : Form
{
    private readonly EmploiDuTempsDAO _edtDao = new();
    private readonly ClasseDAO _classeDao = new();
    private readonly AnneeAcademiqueDAO _anneeDao = new();
    private readonly CoursDAO _coursDao = new();
    private readonly EnseignantDAO _enseignantDao = new();
    private readonly SalleDAO _salleDao = new();


    private int _seanceId = 0;

    public event EventHandler? DataSaved;

    public FormSeanceDetail() : this(0, 0, 0) { }

    public FormSeanceDetail(int seanceId) : this(seanceId, 0, 0) { }

    public FormSeanceDetail(int seanceId, int defaultClasseId, int defaultSemestreId)
    {
        _seanceId = seanceId;
        InitializeComponent();
        Load += (_, _) =>
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            LoadLookups(defaultClasseId, defaultSemestreId);
            if (_seanceId > 0)
                LoadSeanceData();
        };
    }



    private void LoadLookups(int defaultClasseId, int defaultSemestreId)
    {
        var classes = _classeDao.GetAll();
        _cboClasse.DisplayMember = nameof(Classe.Code);
        _cboClasse.ValueMember = nameof(Classe.Id);
        _cboClasse.DataSource = classes;
        if (defaultClasseId > 0)
        {
            var match = classes.FirstOrDefault(c => c.Id == defaultClasseId);
            if (match != null) _cboClasse.SelectedItem = match;
        }

        var annees = _anneeDao.GetAll();
        var currentYear = annees.FirstOrDefault(a => a.Statut == "Ouverte") ?? annees.FirstOrDefault();
        if (currentYear?.Semestres != null && currentYear.Semestres.Count > 0)
        {
            _cboSemestre.DisplayMember = nameof(Semestre.Nom);
            _cboSemestre.ValueMember = nameof(Semestre.Id);
            _cboSemestre.DataSource = currentYear.Semestres;
            if (defaultSemestreId > 0)
            {
                var sMatch = currentYear.Semestres.FirstOrDefault(s => s.Id == defaultSemestreId);
                if (sMatch != null) _cboSemestre.SelectedItem = sMatch;
            }
        }
        else
        {
            _cboSemestre.Items.Add(new Semestre { Id = 1, Nom = "Semestre 1" });
            _cboSemestre.SelectedIndex = 0;
        }

        _cboCours.ValueMember = nameof(Cours.Id);
        _cboCours.DataSource = _coursDao.GetAll();
        _cboEnseignant.ValueMember = nameof(Enseignant.Id);
        _cboEnseignant.DataSource = _enseignantDao.GetAll();
        _cboSalle.ValueMember = nameof(Salle.Id);
        _cboSalle.DataSource = _salleDao.GetAll();
    }

    private void LoadSeanceData()
    {
        var s = _edtDao.GetAll().FirstOrDefault(x => x.Id == _seanceId);
        if (s != null)
        {
            if (_cboClasse.DataSource is List<Classe> classes)
            {
                var cl = classes.FirstOrDefault(x => x.Id == s.ClasseId);
                if (cl != null) _cboClasse.SelectedItem = cl;
            }

            if (_cboCours.DataSource is List<Cours> cours)
            {
                var cr = cours.FirstOrDefault(x => x.Id == s.CoursId);
                if (cr != null) _cboCours.SelectedItem = cr;
            }

            if (_cboEnseignant.DataSource is List<Enseignant> enseignants)
            {
                var en = enseignants.FirstOrDefault(x => x.Id == s.EnseignantId);
                if (en != null) _cboEnseignant.SelectedItem = en;
            }

            if (_cboSalle.DataSource is List<Salle> salles)
            {
                var sa = salles.FirstOrDefault(x => x.Id == s.SalleId);
                if (sa != null) _cboSalle.SelectedItem = sa;
            }

            _cboJour.SelectedItem = s.Jour;
            _dateSeance.Value = s.DateSeance == DateTime.MinValue ? DateTime.Today : s.DateSeance;
            _cboCreneau.SelectedItem = $"{s.HeureDebut[..2]}h-{s.HeureFin[..2]}h";
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (_cboClasse.SelectedItem is not Classe classe ||
            _cboCours.SelectedItem is not Cours cours ||
            _cboEnseignant.SelectedItem is not Enseignant enseignant ||
            _cboSalle.SelectedItem is not Salle salle)
        {
            MessageBox.Show("Veuillez sélectionner la classe, le cours, l'enseignant et la salle.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var semId = (_cboSemestre.SelectedItem as Semestre)?.Id ?? 1;
        var semNom = (_cboSemestre.SelectedItem as Semestre)?.Nom ?? "Semestre 1";
        var jour = _cboJour.SelectedItem?.ToString() ?? "Lundi";
        var creneau = (_cboCreneau.SelectedItem?.ToString() ?? "08h-10h").Split('-');

        var seance = new SeanceEmploiDuTemps
        {
            Id = _seanceId,
            ClasseId = classe.Id,
            SemestreId = semId,
            CoursId = cours.Id,
            EnseignantId = enseignant.Id,
            SalleId = salle.Id,
            Jour = jour,
            HeureDebut = creneau[0].Replace('h', ':' ) + "00",
            HeureFin = creneau[1].Replace('h', ':' ) + "00",
            DateSeance = _dateSeance.Value.Date,
            ClasseCode = classe.Code,
            SemestreNom = semNom,
            CoursIntitule = cours.Intitule,
            EnseignantNom = enseignant.NomComplet,
            SalleNom = salle.Nom
        };

        // RÈGLE MÉTIER CRUCIALE : Vérifier qu'aucun conflit n'existe (salle, enseignant ou classe déjà occupés à ce créneau)
        if (!_edtDao.VerifierEtEnregistrer(seance, out var conflitMessage))
        {
            MessageBox.Show(conflitMessage, "Conflit d'emploi du temps détecté !", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show("Séance enregistrée avec succès sans aucun conflit !", "Validation réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);
        DataSaved?.Invoke(this, EventArgs.Empty);
        Close();
    }

    private void BtnCancel_Click(object? sender, EventArgs e) => Close();
}
