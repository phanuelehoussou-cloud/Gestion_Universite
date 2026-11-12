using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormInscriptionDetail : Form
{
    private readonly InscriptionDAO _inscriptionDao = new();
    private readonly EtudiantDAO _etudiantDao = new();
    private readonly ClasseDAO _classeDao = new();
    private readonly AnneeAcademiqueDAO _anneeDao = new();

    private ComboBox _cboEtudiant = null!;
    private ComboBox _cboClasse = null!;
    private ComboBox _cboAnnee = null!;
    private Label _lblSoldeMontant = null!;
    private DataGridView _dgvPaiements = null!;
    private TextBox _txtMontantPaiement = null!;
    private ComboBox _cboModePaiement = null!;
    private TextBox _txtRefPaiement = null!;
    private int _inscriptionId = 0;

    public event EventHandler? DataSaved;

    public FormInscriptionDetail(int inscriptionId = 0)
    {
        _inscriptionId = inscriptionId;
        InitializeCustomComponents();
        LoadLookups();
        if (_inscriptionId > 0)
            LoadInscriptionData();
    }

    private void InitializeCustomComponents()
    {
        Text = "Gestion de l'inscription";
        Size = new Size(820, 620);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        var titleBar = UITheme.CreateTitleBar(this, "7. Inscriptions & Paiements", "📝", isDialog: true);
        Controls.Add(titleBar);

        var content = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16), BackColor = Color.Transparent };

        var card = UITheme.CreateCard("Détails de l'inscription", "📝");
        card.Dock = DockStyle.Fill;
        card.Padding = new Padding(16);

        // Header section: 2 columns (Form on left, Big Red Solde Box on right)
        var topSection = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 150,
            ColumnCount = 2,
            RowCount = 1
        };
        topSection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
        topSection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));

        // Form fields on left
        var pnlFields = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 3,
            Padding = new Padding(4)
        };
        pnlFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
        pnlFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        pnlFields.Controls.Add(new Label { Text = "Étudiant :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _cboEtudiant = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlFields.Controls.Add(_cboEtudiant, 1, 0);

        pnlFields.Controls.Add(new Label { Text = "Classe :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _cboClasse = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlFields.Controls.Add(_cboClasse, 1, 1);

        pnlFields.Controls.Add(new Label { Text = "Année :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _cboAnnee = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlFields.Controls.Add(_cboAnnee, 1, 2);

        topSection.Controls.Add(pnlFields, 0, 0);

        // Large Red Solde Box on right
        var pnlSolde = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = UITheme.PrimaryRed,
            Padding = new Padding(16),
            Margin = new Padding(10)
        };
        pnlSolde.Paint += (s, e) =>
        {
            using var path = UITheme.GetRoundedRectangle(new Rectangle(0, 0, pnlSolde.Width - 1, pnlSolde.Height - 1), 12);
            pnlSolde.Region = new Region(path);
        };

        var lblSoldeTitre = new Label
        {
            Text = "Solde actuel :",
            Font = new Font("Segoe UI", 13f, FontStyle.Bold),
            ForeColor = Color.White,
            Dock = DockStyle.Top,
            Height = 32,
            TextAlign = ContentAlignment.MiddleCenter
        };

        _lblSoldeMontant = new Label
        {
            Text = "50 000 FCFA",
            Font = new Font("Segoe UI", 18f, FontStyle.Bold),
            ForeColor = Color.White,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };

        pnlSolde.Controls.Add(_lblSoldeMontant);
        pnlSolde.Controls.Add(lblSoldeTitre);
        topSection.Controls.Add(pnlSolde, 1, 0);

        // Section: Paiements
        var pnlPaiementsHeader = new Panel { Dock = DockStyle.Top, Height = 45, Padding = new Padding(4) };
        var lblPaiements = new Label
        {
            Text = "Historique des paiements",
            Font = UITheme.FontBold(11f),
            ForeColor = UITheme.PrimaryRed,
            Dock = DockStyle.Left,
            AutoSize = true
        };
        pnlPaiementsHeader.Controls.Add(lblPaiements);

        // Quick add payment bar
        var pnlAddPayment = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 45, FlowDirection = FlowDirection.LeftToRight };
        _txtMontantPaiement = new TextBox { PlaceholderText = "Montant FCFA (ex: 50000)", Width = 150, Font = UITheme.FontRegular(9.5f) };
        _cboModePaiement = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 120, Font = UITheme.FontRegular(9.5f) };
        _cboModePaiement.Items.AddRange(new object[] { "Virement", "Espèces", "Mobile Money", "Chèque" });
        _cboModePaiement.SelectedIndex = 0;
        _txtRefPaiement = new TextBox { PlaceholderText = "Référence (ex: REF2026001)", Width = 160, Font = UITheme.FontRegular(9.5f) };
        var btnAddPaiement = UITheme.CreatePrimaryButton("Ajouter paiement");
        btnAddPaiement.Click += BtnAddPaiement_Click;

        pnlAddPayment.Controls.Add(_txtMontantPaiement);
        pnlAddPayment.Controls.Add(_cboModePaiement);
        pnlAddPayment.Controls.Add(_txtRefPaiement);
        pnlAddPayment.Controls.Add(btnAddPaiement);

        // Table des paiements
        _dgvPaiements = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvPaiements);

        _dgvPaiements.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColDate", HeaderText = "Date", FillWeight = 100 });
        _dgvPaiements.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColMontant", HeaderText = "Montant (FCFA)", FillWeight = 120 });
        _dgvPaiements.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColMode", HeaderText = "Mode", FillWeight = 100 });
        _dgvPaiements.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColReference", HeaderText = "Référence", FillWeight = 130 });
        _dgvPaiements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        // Bottom action buttons
        var bottomBar = new Panel { Dock = DockStyle.Bottom, Height = 50, Padding = new Padding(6) };
        var btnSave = UITheme.CreatePrimaryButton("Enregistrer");
        btnSave.Location = new Point(card.Width - 360, 8);
        btnSave.Click += BtnSave_Click;

        var btnPrint = UITheme.CreateSecondaryButton("Imprimer reçu", "🖨");
        btnPrint.Location = new Point(card.Width - 230, 8);
        btnPrint.Click += BtnPrint_Click;

        var btnClose = UITheme.CreateSecondaryButton("Fermer");
        btnClose.Location = new Point(card.Width - 95, 8);
        btnClose.Click += (s, e) => Close();

        bottomBar.Controls.Add(btnSave);
        bottomBar.Controls.Add(btnPrint);
        bottomBar.Controls.Add(btnClose);

        card.Controls.Add(_dgvPaiements);
        card.Controls.Add(pnlAddPayment);
        card.Controls.Add(pnlPaiementsHeader);
        card.Controls.Add(topSection);
        card.Controls.Add(bottomBar);

        content.Controls.Add(card);
        Controls.Add(content);
    }

    private void LoadLookups()
    {
        _cboEtudiant.DataSource = _etudiantDao.GetAll();
        _cboClasse.DataSource = _classeDao.GetAll();
        _cboAnnee.DataSource = _anneeDao.GetAll();
    }

    private void LoadInscriptionData()
    {
        var ins = _inscriptionDao.GetById(_inscriptionId);
        if (ins != null)
        {
            var etudiants = (List<Etudiant>)_cboEtudiant.DataSource;
            var e = etudiants.FirstOrDefault(x => x.Id == ins.EtudiantId);
            if (e != null) _cboEtudiant.SelectedItem = e;

            var classes = (List<Classe>)_cboClasse.DataSource;
            var c = classes.FirstOrDefault(x => x.Id == ins.ClasseId);
            if (c != null) _cboClasse.SelectedItem = c;

            var annees = (List<AnneeAcademique>)_cboAnnee.DataSource;
            var a = annees.FirstOrDefault(x => x.Id == ins.AnneeAcademiqueId);
            if (a != null) _cboAnnee.SelectedItem = a;

            UpdateSoldeUI(ins);
        }
    }

    private void UpdateSoldeUI(Inscription ins)
    {
        _lblSoldeMontant.Text = $"{ins.SoldeActuel:N0} FCFA";

        _dgvPaiements.Rows.Clear();
        if (ins.Paiements != null)
        {
            foreach (var p in ins.Paiements)
            {
                _dgvPaiements.Rows.Add(p.DatePaiement.ToString("dd/MM/yyyy"), $"{p.Montant:N0} FCFA", p.ModePaiement, p.Reference);
            }
        }
    }

    private void BtnAddPaiement_Click(object? sender, EventArgs e)
    {
        if (_inscriptionId == 0)
        {
            MessageBox.Show("Veuillez d'abord enregistrer l'inscription avant d'ajouter des paiements.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!decimal.TryParse(_txtMontantPaiement.Text.Trim(), out var montant) || montant <= 0)
        {
            MessageBox.Show("Veuillez saisir un montant valide.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var refPaiement = _txtRefPaiement.Text.Trim();
        if (string.IsNullOrEmpty(refPaiement))
            refPaiement = $"REF{DateTime.Now:yyyyMMddHHmmss}";

        var p = new Paiement
        {
            DatePaiement = DateTime.Now,
            Montant = montant,
            ModePaiement = _cboModePaiement.SelectedItem?.ToString() ?? "Virement",
            Reference = refPaiement,
            Remarque = "Paiement de scolarité"
        };

        if (_inscriptionDao.AjouterPaiement(_inscriptionId, p, out var error))
        {
            MessageBox.Show("Paiement enregistré avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _txtMontantPaiement.Text = "";
            _txtRefPaiement.Text = "";
            LoadInscriptionData();
            DataSaved?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            MessageBox.Show(error, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (_cboEtudiant.SelectedItem is not Etudiant etudiant ||
            _cboClasse.SelectedItem is not Classe classe ||
            _cboAnnee.SelectedItem is not AnneeAcademique annee)
        {
            MessageBox.Show("Veuillez sélectionner un étudiant, une classe et une année académique.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var ins = _inscriptionId > 0 ? _inscriptionDao.GetById(_inscriptionId) ?? new Inscription() : new Inscription();
        ins.EtudiantId = etudiant.Id;
        ins.ClasseId = classe.Id;
        ins.AnneeAcademiqueId = annee.Id;
        ins.DateInscription = DateTime.Now;
        ins.Statut = "Active";

        if (_inscriptionDao.Inscrire(ins, out var error))
        {
            _inscriptionId = ins.Id;
            MessageBox.Show("Inscription enregistrée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateSoldeUI(ins);
            DataSaved?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            // Message de blocage de double inscription active
            MessageBox.Show(error, "Règle de Gestion : Inscription impossible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnPrint_Click(object? sender, EventArgs e)
    {
        if (_inscriptionId == 0)
        {
            MessageBox.Show("Aucune inscription sélectionnée.", "Impression", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var ins = _inscriptionDao.GetById(_inscriptionId);
        if (ins != null)
        {
            var msg = $"========================================\n" +
                      $"    IIT - REÇU DE PAIEMENT SCOLARITÉ    \n" +
                      $"========================================\n" +
                      $"Étudiant : {ins.EtudiantNomComplet}\n" +
                      $"Matricule : {ins.EtudiantMatricule}\n" +
                      $"Classe   : {ins.ClasseCode}\n" +
                      $"Année    : {ins.AnneeCode}\n" +
                      $"Total scolarité : {ins.MontantTotal:N0} FCFA\n" +
                      $"Total réglé     : {ins.MontantPaye:N0} FCFA\n" +
                      $"SOLDE RESTANT   : {ins.SoldeActuel:N0} FCFA\n" +
                      $"========================================\n" +
                      $"Généré le {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

            MessageBox.Show(msg, "Reçu d'inscription officiel IIT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
