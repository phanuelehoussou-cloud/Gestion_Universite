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

    private ComboBox _cboClasse = null!;
    private ComboBox _cboSemestre = null!;
    private ComboBox _cboCours = null!;
    private ComboBox _cboEnseignant = null!;
    private ComboBox _cboSalle = null!;
    private ComboBox _cboJour = null!;
    private ComboBox _cboCreneau = null!;
    private int _seanceId = 0;

    public event EventHandler? DataSaved;

    public FormSeanceDetail(int seanceId = 0, int defaultClasseId = 0, int defaultSemestreId = 0)
    {
        _seanceId = seanceId;
        InitializeCustomComponents();
        LoadLookups(defaultClasseId, defaultSemestreId);
        if (_seanceId > 0)
            LoadSeanceData();
    }

    private void InitializeCustomComponents()
    {
        Text = _seanceId == 0 ? "Programmer une séance de cours" : "Modifier la séance";
        Size = new Size(580, 500);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        var titleBar = UITheme.CreateTitleBar(this, _seanceId == 0 ? "11. Emploi du temps - Nouvelle Séance" : "11. Emploi du temps - Modification", "🗓️", isDialog: true);
        Controls.Add(titleBar);

        var card = UITheme.CreateCard("Paramètres de la séance", "🗓️");
        card.Dock = DockStyle.Fill;
        card.Padding = new Padding(16);

        var pnlForm = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 8,
            Padding = new Padding(10)
        };
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        pnlForm.Controls.Add(new Label { Text = "Classe :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _cboClasse = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_cboClasse, 1, 0);

        pnlForm.Controls.Add(new Label { Text = "Semestre :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _cboSemestre = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_cboSemestre, 1, 1);

        pnlForm.Controls.Add(new Label { Text = "Jour :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _cboJour = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboJour.Items.AddRange(new object[] { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi" });
        _cboJour.SelectedIndex = 0;
        pnlForm.Controls.Add(_cboJour, 1, 2);

        pnlForm.Controls.Add(new Label { Text = "Créneau :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 3);
        _cboCreneau = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboCreneau.Items.AddRange(new object[] { "08h-10h", "10h-12h", "12h-14h", "14h-16h", "16h-18h", "18h-20h" });
        _cboCreneau.SelectedIndex = 0;
        pnlForm.Controls.Add(_cboCreneau, 1, 3);

        pnlForm.Controls.Add(new Label { Text = "Cours :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 4);
        _cboCours = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_cboCours, 1, 4);

        pnlForm.Controls.Add(new Label { Text = "Enseignant :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 5);
        _cboEnseignant = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_cboEnseignant, 1, 5);

        pnlForm.Controls.Add(new Label { Text = "Salle :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 6);
        _cboSalle = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_cboSalle, 1, 6);

        var pnlBtns = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        var btnSave = UITheme.CreatePrimaryButton("Vérifier et Enregistrer");
        btnSave.Click += BtnSave_Click;
        var btnCancel = UITheme.CreateSecondaryButton("Annuler");
        btnCancel.Click += (s, e) => Close();
        pnlBtns.Controls.Add(btnSave);
        pnlBtns.Controls.Add(btnCancel);
        pnlForm.Controls.Add(pnlBtns, 1, 7);

        card.Controls.Add(pnlForm);
        Controls.Add(card);
    }

    private void LoadLookups(int defaultClasseId, int defaultSemestreId)
    {
        var classes = _classeDao.GetAll();
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

        _cboCours.DataSource = _coursDao.GetAll();
        _cboEnseignant.DataSource = _enseignantDao.GetAll();
        _cboSalle.DataSource = _salleDao.GetAll();
    }

    private void LoadSeanceData()
    {
        var s = _edtDao.GetAll().FirstOrDefault(x => x.Id == _seanceId);
        if (s != null)
        {
            var classes = (List<Classe>)_cboClasse.DataSource;
            var cl = classes.FirstOrDefault(x => x.Id == s.ClasseId);
            if (cl != null) _cboClasse.SelectedItem = cl;

            var cours = (List<Cours>)_cboCours.DataSource;
            var cr = cours.FirstOrDefault(x => x.Id == s.CoursId);
            if (cr != null) _cboCours.SelectedItem = cr;

            var enseignants = (List<Enseignant>)_cboEnseignant.DataSource;
            var en = enseignants.FirstOrDefault(x => x.Id == s.EnseignantId);
            if (en != null) _cboEnseignant.SelectedItem = en;

            var salles = (List<Salle>)_cboSalle.DataSource;
            var sa = salles.FirstOrDefault(x => x.Id == s.SalleId);
            if (sa != null) _cboSalle.SelectedItem = sa;

            _cboJour.SelectedItem = s.Jour;
            _cboCreneau.SelectedItem = s.HeureDebut;
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
        var creneau = _cboCreneau.SelectedItem?.ToString() ?? "08h-10h";

        var seance = new SeanceEmploiDuTemps
        {
            Id = _seanceId,
            ClasseId = classe.Id,
            SemestreId = semId,
            CoursId = cours.Id,
            EnseignantId = enseignant.Id,
            SalleId = salle.Id,
            Jour = jour,
            HeureDebut = creneau,
            HeureFin = creneau.Contains('-') ? creneau.Split('-')[1] : "10h",
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
}
