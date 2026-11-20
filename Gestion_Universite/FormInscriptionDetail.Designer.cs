using DAO.Models;
using DAO.Repositories;
#nullable enable
namespace Gestion_Universite;

partial class FormInscriptionDetail
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private ComboBox _cboEtudiant = null!;
    private ComboBox _cboClasse = null!;
    private ComboBox _cboAnnee = null!;
    private Label _lblSoldeMontant = null!;
    private DataGridView _dgvPaiements = null!;
    private TextBox _txtMontantPaiement = null!;
    private ComboBox _cboModePaiement = null!;
    private TextBox _txtRefPaiement = null!;

    private Panel titleBar = null!;
    private Panel content = null!;
    private Panel card = null!;
    private TableLayoutPanel topSection = null!;
    private TableLayoutPanel pnlFields = null!;
    private Panel pnlSolde = null!;
    private Label lblSoldeTitre = null!;
    private Panel pnlPaiementsHeader = null!;
    private Label lblPaiements = null!;
    private FlowLayoutPanel pnlAddPayment = null!;
    private Button btnAddPaiement = null!;
    private FlowLayoutPanel bottomBar = null!;
    private Button btnClose = null!;
    private Button btnPrint = null!;
    private Button btnSave = null!;

    private void InitializeComponent()
    {
        Text = "Gestion de l'inscription";
        Size = new Size(820, 620);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        titleBar = UITheme.CreateTitleBar(this, "7. Inscriptions & Paiements", "IIT", isDialog: true);
        Controls.Add(titleBar);

        content = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16), BackColor = Color.Transparent };

        card = UITheme.CreateCard("Détails de l'inscription", "");
        card.Dock = DockStyle.Fill;
        card.Padding = new Padding(16);

        // Header section: 2 columns (Form on left, Big Red Solde Box on right)
        topSection = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 150,
            ColumnCount = 2,
            RowCount = 1
        };
        topSection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
        topSection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));

        // Form fields on left
        pnlFields = new TableLayoutPanel
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

        pnlFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        pnlFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        pnlFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));

        pnlFields.Controls.Add(new Label { Text = "Année :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _cboAnnee = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboAnnee.SelectedIndexChanged += CboAnnee_SelectedIndexChanged;
        pnlFields.Controls.Add(_cboAnnee, 1, 2);

        topSection.Controls.Add(pnlFields, 0, 0);

        // Large Red Solde Box on right
        pnlSolde = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = UITheme.PrimaryRed,
            Padding = new Padding(12),
            Margin = new Padding(6)
        };
        pnlSolde.Paint += (s, e) =>
        {
            using var path = UITheme.GetRoundedRectangle(new Rectangle(0, 0, pnlSolde.Width - 1, pnlSolde.Height - 1), 12);
            pnlSolde.Region = new Region(path);
        };

        lblSoldeTitre = new Label
        {
            Text = "Solde actuel :",
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            ForeColor = Color.White,
            Dock = DockStyle.Top,
            Height = 30,
            TextAlign = ContentAlignment.MiddleCenter
        };

        _lblSoldeMontant = new Label
        {
            Text = "50 000 FCFA",
            Font = new Font("Segoe UI", 16f, FontStyle.Bold),
            ForeColor = Color.White,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };

        pnlSolde.Controls.Add(_lblSoldeMontant);
        pnlSolde.Controls.Add(lblSoldeTitre);
        topSection.Controls.Add(pnlSolde, 1, 0);

        // Section: Paiements
        pnlPaiementsHeader = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(4) };
        lblPaiements = new Label
        {
            Text = "Historique des paiements",
            Font = UITheme.FontBold(11f),
            ForeColor = UITheme.PrimaryRed,
            Dock = DockStyle.Left,
            AutoSize = true
        };
        pnlPaiementsHeader.Controls.Add(lblPaiements);

        // Quick add payment bar
        pnlAddPayment = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 45, FlowDirection = FlowDirection.LeftToRight };
        _txtMontantPaiement = new TextBox { PlaceholderText = "Montant FCFA (ex: 50000)", Width = 150, Font = UITheme.FontRegular(9.5f) };
        _cboModePaiement = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 120, Font = UITheme.FontRegular(9.5f) };
        _cboModePaiement.Items.AddRange(new object[] { "Virement", "Espèces", "Mobile Money", "Chèque" });
        _cboModePaiement.SelectedIndex = 0;
        _txtRefPaiement = new TextBox { PlaceholderText = "Référence (ex: REF2026001)", Width = 160, Font = UITheme.FontRegular(9.5f) };
        btnAddPaiement = UITheme.CreatePrimaryButton("Ajouter paiement");
        btnAddPaiement.Click += BtnAddPaiement_Click;

        pnlAddPayment.Controls.Add(_txtMontantPaiement);
        pnlAddPayment.Controls.Add(_cboModePaiement);
        pnlAddPayment.Controls.Add(_txtRefPaiement);
        pnlAddPayment.Controls.Add(btnAddPaiement);

        // Table des paiements
        _dgvPaiements = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvPaiements);

        _dgvPaiements.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColDate", HeaderText = "Date", FillWeight = 100, MinimumWidth = 85 });
        _dgvPaiements.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColMontant", HeaderText = "Montant (FCFA)", FillWeight = 120, MinimumWidth = 110 });
        _dgvPaiements.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColMode", HeaderText = "Mode", FillWeight = 100, MinimumWidth = 85 });
        _dgvPaiements.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColReference", HeaderText = "Référence", FillWeight = 130, MinimumWidth = 100 });
        _dgvPaiements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        // Bottom action buttons in FlowLayoutPanel with RightToLeft
        bottomBar = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 48,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(4)
        };

        btnClose = UITheme.CreateSecondaryButton("Fermer");
        btnClose.Click += (s, e) => Close();

        btnPrint = UITheme.CreateSecondaryButton("Imprimer reçu", "");
        btnPrint.Click += BtnPrint_Click;

        btnSave = UITheme.CreatePrimaryButton("Enregistrer");
        btnSave.Click += BtnSave_Click;

        bottomBar.Controls.Add(btnClose);
        bottomBar.Controls.Add(btnPrint);
        bottomBar.Controls.Add(btnSave);

        card.Controls.Add(_dgvPaiements);
        card.Controls.Add(pnlAddPayment);
        card.Controls.Add(pnlPaiementsHeader);
        card.Controls.Add(topSection);
        card.Controls.Add(bottomBar);

        content.Controls.Add(card);
        Controls.Add(content);
        Controls.Add(titleBar);
    }
}
