using DAO.Models;
using DAO.Repositories;
#nullable enable
namespace Gestion_Universite;

partial class FormCours
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private TextBox _txtCode = null!;
    private TextBox _txtIntitule = null!;
    private TextBox _txtDescription = null!;
    private TextBox _txtVolume = null!;
    private DataGridView _dgvCours = null!;

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
        Text = "Cours";
        Size = new Size(1000, 560);
        MinimumSize = new Size(800, 480);
        AutoScroll = false;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        titleBar = UITheme.CreateTitleBar(this, "9. Cours & Matières", "IIT");

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
        cardForm = UITheme.CreateCard("Cours", "");
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

        pnlForm.Controls.Add(new Label { Text = "Code :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _txtCode = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtCode, 1, 0);

        pnlForm.Controls.Add(new Label { Text = "Intitulé :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _txtIntitule = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtIntitule, 1, 1);

        pnlForm.Controls.Add(new Label { Text = "Description :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _txtDescription = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtDescription, 1, 2);

        pnlForm.Controls.Add(new Label { Text = "Volume horaire :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 3);
        _txtVolume = new TextBox { Font = UITheme.FontRegular(10), Text = "45 h", Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtVolume, 1, 3);

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

        // RIGHT CARD: Liste des cours
        cardList = UITheme.CreateCard("Liste des cours", "");
        cardList.Dock = DockStyle.Fill;

        _dgvCours = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvCours);

        _dgvCours.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvCours.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCode", HeaderText = "Code", FillWeight = 90, MinimumWidth = 70 });
        _dgvCours.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColIntitule", HeaderText = "Intitulé", FillWeight = 160, MinimumWidth = 100 });
        _dgvCours.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColVH", HeaderText = "VH", FillWeight = 60, MinimumWidth = 50 });

        colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Action", Text = "Modifier", UseColumnTextForButtonValue = true, FillWeight = 75 };
        colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "", Text = "Supprimer", UseColumnTextForButtonValue = true, FillWeight = 85 };
        _dgvCours.Columns.AddRange(colEdit, colDelete);
        _dgvCours.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvCours.CellContentClick += DgvCours_CellContentClick;

        cardList.Controls.Add(_dgvCours);
        mainPanel.Controls.Add(cardList, 1, 0);

        Controls.Add(mainPanel);
        Controls.Add(titleBar);
    }
}
