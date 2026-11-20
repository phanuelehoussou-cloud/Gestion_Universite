using DAO.Models;
using DAO.Repositories;
#nullable enable
namespace Gestion_Universite;

partial class FormSalle
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private TextBox _txtNom = null!;
    private TextBox _txtCapacite = null!;
    private ComboBox _cboLocalisation = null!;
    private ComboBox _cboStatut = null!;
    private DataGridView _dgvSalles = null!;

    private Panel titleBar = null!;
    private TableLayoutPanel mainPanel = null!;
    private Panel cardForm = null!;
    private TableLayoutPanel pnlForm = null!;
    private FlowLayoutPanel pnlBtns = null!;
    private Button btnSave = null!;
    private Button btnReset = null!;
    private Panel cardList = null!;
    private DataGridViewButtonColumn colEdit = null!;
    private DataGridViewButtonColumn colDelete = null!;

    private void InitializeComponent()
    {
        Text = "Salles";
        Size = new Size(1000, 560);
        MinimumSize = new Size(800, 480);
        AutoScroll = false;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        titleBar = UITheme.CreateTitleBar(this, "10. Salles de Cours", "IIT");

        mainPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(16),
            BackColor = Color.Transparent
        };
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));

        // LEFT CARD: Saisie / Modification
        cardForm = UITheme.CreateCard("Salles", "");
        cardForm.Dock = DockStyle.Fill;

        pnlForm = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 5,
            Padding = new Padding(8)
        };
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        for (int i = 0; i < 4; i++)
            pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

        pnlForm.Controls.Add(new Label { Text = "Nom :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _txtNom = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtNom, 1, 0);

        pnlForm.Controls.Add(new Label { Text = "Capacité :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _txtCapacite = new TextBox { Font = UITheme.FontRegular(10), Text = "80", Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtCapacite, 1, 1);

        pnlForm.Controls.Add(new Label { Text = "Localisation :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _cboLocalisation = new ComboBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboLocalisation.Items.AddRange(new object[] { "Bâtiment A", "Bâtiment B", "Bâtiment C", "Amphithéâtre" });
        _cboLocalisation.SelectedIndex = 0;
        pnlForm.Controls.Add(_cboLocalisation, 1, 2);

        pnlForm.Controls.Add(new Label { Text = "Statut :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 3);
        _cboStatut = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboStatut.Items.AddRange(new object[] { "Disponible", "En maintenance", "Occupée" });
        _cboStatut.SelectedIndex = 0;
        pnlForm.Controls.Add(_cboStatut, 1, 3);

        pnlBtns = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        btnSave = UITheme.CreatePrimaryButton("Enregistrer");
        btnSave.Click += BtnSave_Click;
        btnReset = UITheme.CreateSecondaryButton("Réinitialiser");
        btnReset.Click += (s, e) => ResetForm();
        pnlBtns.Controls.Add(btnSave);
        pnlBtns.Controls.Add(btnReset);
        pnlForm.Controls.Add(pnlBtns, 1, 4);

        cardForm.Controls.Add(pnlForm);
        mainPanel.Controls.Add(cardForm, 0, 0);

        // RIGHT CARD: Liste des salles
        cardList = UITheme.CreateCard("Liste des salles", "");
        cardList.Dock = DockStyle.Fill;

        _dgvSalles = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvSalles);

        _dgvSalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvSalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColNom", HeaderText = "Nom", FillWeight = 90, MinimumWidth = 70 });
        _dgvSalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCapacite", HeaderText = "Capacité", FillWeight = 80, MinimumWidth = 60 });
        _dgvSalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColStatut", HeaderText = "Statut", FillWeight = 110, MinimumWidth = 80 });

        colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Action", Text = "Modifier", UseColumnTextForButtonValue = true, FillWeight = 75 };
        colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "", Text = "Supprimer", UseColumnTextForButtonValue = true, FillWeight = 85 };
        _dgvSalles.Columns.AddRange(colEdit, colDelete);
        _dgvSalles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvSalles.CellContentClick += DgvSalles_CellContentClick;

        cardList.Controls.Add(_dgvSalles);
        mainPanel.Controls.Add(cardList, 1, 0);

        Controls.Add(mainPanel);
    }
}
