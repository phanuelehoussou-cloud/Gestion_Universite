using DAO.Models;
using DAO.Repositories;
#nullable enable
namespace Gestion_Universite;

partial class FormAnneeSemestre
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private DataGridView _dgvAnnees = null!;
    private TextBox _txtAnnee = null!;
    private DateTimePicker _dtpDebut = null!;
    private DateTimePicker _dtpFin = null!;
    private ComboBox _cboSemestre = null!;
    private ComboBox _cboAnneePourSemestre = null!;
    private DateTimePicker _dtpSemDebut = null!;

    private Panel titleBar = null!;
    private TableLayoutPanel mainPanel = null!;
    private Panel leftPanel = null!;
    private Panel cardAnnee = null!;
    private TableLayoutPanel pnlFormAnnee = null!;
    private FlowLayoutPanel pnlBtnAnnee = null!;
    private Button btnSaveAnnee = null!;
    private Button btnResetAnnee = null!;
    private Panel cardSemestre = null!;
    private TableLayoutPanel pnlFormSem = null!;
    private Button btnAddSem = null!;
    private Panel cardListe = null!;
    private DataGridViewButtonColumn colView = null!;
    private DataGridViewButtonColumn colEdit = null!;
    private DataGridViewButtonColumn colDelete = null!;

    private void InitializeComponent()
    {
        Text = "Années académiques et semestres";
        Size = new Size(1050, 620);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        AutoScroll = true;

        titleBar = UITheme.CreateTitleBar(this, "2. Années académiques et semestres", "IIT");

        mainPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(12),
            BackColor = Color.Transparent,
            AutoScroll = true
        };
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));

        // LEFT COLUMN: Nouvelle année académique & Ajouter un semestre
        leftPanel = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true
        };

        // --- Card: Nouvelle année académique ---
        cardAnnee = UITheme.CreateCard("Nouvelle année académique", "");
        cardAnnee.Dock = DockStyle.Top;
        cardAnnee.Height = 255;
        cardAnnee.Margin = new Padding(0, 0, 0, 10);

        pnlFormAnnee = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            Padding = new Padding(6)
        };
        pnlFormAnnee.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95));
        pnlFormAnnee.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        pnlFormAnnee.Controls.Add(new Label { Text = "Année :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _txtAnnee = new TextBox { Text = "2026 - 2027", Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlFormAnnee.Controls.Add(_txtAnnee, 1, 0);

        pnlFormAnnee.Controls.Add(new Label { Text = "Date début :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _dtpDebut = new DateTimePicker { Value = new DateTime(2026, 10, 1), Format = DateTimePickerFormat.Short, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlFormAnnee.Controls.Add(_dtpDebut, 1, 1);

        pnlFormAnnee.Controls.Add(new Label { Text = "Date fin :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _dtpFin = new DateTimePicker { Value = new DateTime(2027, 9, 30), Format = DateTimePickerFormat.Short, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlFormAnnee.Controls.Add(_dtpFin, 1, 2);

        pnlBtnAnnee = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        btnSaveAnnee = UITheme.CreatePrimaryButton("Enregistrer");
        btnSaveAnnee.Click += BtnSaveAnnee_Click;
        btnResetAnnee = UITheme.CreateSecondaryButton("Réinitialiser");
        btnResetAnnee.Click += (s, e) => ResetAnneeForm();
        pnlBtnAnnee.Controls.Add(btnSaveAnnee);
        pnlBtnAnnee.Controls.Add(btnResetAnnee);
        pnlFormAnnee.Controls.Add(pnlBtnAnnee, 1, 3);

        cardAnnee.Controls.Add(pnlFormAnnee);

        // --- Card: Ajouter un semestre ---
        cardSemestre = UITheme.CreateCard("Ajouter un semestre", "");
        cardSemestre.Dock = DockStyle.Top;
        cardSemestre.Height = 225;

        pnlFormSem = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            Padding = new Padding(6)
        };
        pnlFormSem.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95));
        pnlFormSem.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        pnlFormSem.Controls.Add(new Label { Text = "Année :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _cboAnneePourSemestre = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlFormSem.Controls.Add(_cboAnneePourSemestre, 1, 0);

        pnlFormSem.Controls.Add(new Label { Text = "Semestre :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _cboSemestre = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboSemestre.Items.AddRange(new object[] { "Semestre 1", "Semestre 2", "Semestre 3", "Semestre 4" });
        _cboSemestre.SelectedIndex = 0;
        pnlFormSem.Controls.Add(_cboSemestre, 1, 1);

        pnlFormSem.Controls.Add(new Label { Text = "Date début :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _dtpSemDebut = new DateTimePicker { Value = new DateTime(2026, 10, 1), Format = DateTimePickerFormat.Short, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlFormSem.Controls.Add(_dtpSemDebut, 1, 2);

        btnAddSem = UITheme.CreatePrimaryButton("Ajouter");
        btnAddSem.Click += BtnAddSem_Click;
        pnlFormSem.Controls.Add(btnAddSem, 1, 3);

        cardSemestre.Controls.Add(pnlFormSem);

        leftPanel.Controls.Add(cardSemestre);
        leftPanel.Controls.Add(cardAnnee);

        mainPanel.Controls.Add(leftPanel, 0, 0);

        // RIGHT COLUMN: Liste des années académiques
        cardListe = UITheme.CreateCard("Liste des années académiques", "");
        cardListe.Dock = DockStyle.Fill;

        _dgvAnnees = new DataGridView
        {
            Dock = DockStyle.Fill
        };
        UITheme.StyleDataGridView(_dgvAnnees);

        _dgvAnnees.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvAnnees.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColAnnee", HeaderText = "Année", FillWeight = 110, MinimumWidth = 90 });
        _dgvAnnees.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColDebut", HeaderText = "Début", FillWeight = 90, MinimumWidth = 80 });
        _dgvAnnees.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColFin", HeaderText = "Fin", FillWeight = 90, MinimumWidth = 80 });
        _dgvAnnees.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColStatut", HeaderText = "Statut", FillWeight = 90, MinimumWidth = 80 });

        colView = new DataGridViewButtonColumn { Name = "ColView", HeaderText = "Voir", Text = "Voir", UseColumnTextForButtonValue = true, FillWeight = 75, MinimumWidth = 75 };
        colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Éditer", Text = "Modifier", UseColumnTextForButtonValue = true, FillWeight = 75, MinimumWidth = 92 };
        colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "Suppr", Text = "Supprimer", UseColumnTextForButtonValue = true, FillWeight = 85, MinimumWidth = 105 };
        _dgvAnnees.Columns.AddRange(colView, colEdit, colDelete);
        _dgvAnnees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvAnnees.CellContentClick += DgvAnnees_CellContentClick;

        cardListe.Controls.Add(_dgvAnnees);
        mainPanel.Controls.Add(cardListe, 1, 0);

        Controls.Add(mainPanel);
        Controls.Add(titleBar);
    }
}
