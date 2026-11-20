using DAO.Models;
using DAO.Repositories;
#nullable enable
namespace Gestion_Universite;

partial class FormNiveau
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private ComboBox _cboNiveau = null!;
    private ComboBox _cboCycle = null!;
    private DataGridView _dgvNiveaux = null!;

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
        Text = "Niveaux";
        Size = new Size(950, 520);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        AutoScroll = true;

        titleBar = UITheme.CreateTitleBar(this, "4. Niveaux", "IIT");

        mainPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(12),
            BackColor = Color.Transparent,
            AutoScroll = true
        };
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));

        // LEFT CARD: Saisie
        cardForm = UITheme.CreateCard("Niveaux", "");
        cardForm.Dock = DockStyle.Fill;

        pnlForm = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            Padding = new Padding(6)
        };
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        pnlForm.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        pnlForm.Controls.Add(new Label { Text = "Niveau :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _cboNiveau = new ComboBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboNiveau.Items.AddRange(new object[] { "L1", "L2", "L3", "M1", "M2", "Doctorat" });
        _cboNiveau.SelectedIndex = 0;
        pnlForm.Controls.Add(_cboNiveau, 1, 0);

        pnlForm.Controls.Add(new Label { Text = "Cycle :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _cboCycle = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboCycle.Items.AddRange(new object[] { "Licence", "Master", "Doctorat" });
        _cboCycle.SelectedIndex = 0;
        pnlForm.Controls.Add(_cboCycle, 1, 1);

        pnlBtns = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        btnSave = UITheme.CreatePrimaryButton("Enregistrer");
        btnSave.Click += BtnSave_Click;
        btnReset = UITheme.CreateSecondaryButton("Réinitialiser");
        btnReset.Click += (s, e) => ResetForm();
        pnlBtns.Controls.Add(btnSave);
        pnlBtns.Controls.Add(btnReset);
        pnlForm.Controls.Add(pnlBtns, 1, 2);

        cardForm.Controls.Add(pnlForm);
        mainPanel.Controls.Add(cardForm, 0, 0);

        // RIGHT CARD: Liste des niveaux
        cardList = UITheme.CreateCard("Liste des niveaux", "");
        cardList.Dock = DockStyle.Fill;

        _dgvNiveaux = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvNiveaux);

        _dgvNiveaux.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvNiveaux.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColNiveau", HeaderText = "Niveau", FillWeight = 120, MinimumWidth = 100 });
        _dgvNiveaux.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCycle", HeaderText = "Cycle", FillWeight = 140, MinimumWidth = 110 });

        colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Action", Text = "Modifier", UseColumnTextForButtonValue = true, FillWeight = 75, MinimumWidth = 92 };
        colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "", Text = "Supprimer", UseColumnTextForButtonValue = true, FillWeight = 85, MinimumWidth = 105 };
        _dgvNiveaux.Columns.AddRange(colEdit, colDelete);
        _dgvNiveaux.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvNiveaux.CellContentClick += DgvNiveaux_CellContentClick;

        cardList.Controls.Add(_dgvNiveaux);
        mainPanel.Controls.Add(cardList, 1, 0);

        Controls.Add(mainPanel);
        Controls.Add(titleBar);
    }
}
