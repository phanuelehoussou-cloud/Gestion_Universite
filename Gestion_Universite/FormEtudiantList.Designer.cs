using DAO.Models;
using DAO.Repositories;
#nullable enable
namespace Gestion_Universite;

partial class FormEtudiantList
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private DataGridView _dgvEtudiants = null!;
    private TextBox _txtSearch = null!;

    private Panel titleBar = null!;
    private Panel contentPanel = null!;
    private Panel card = null!;
    private Panel topBar = null!;
    private Button btnAdd = null!;
    private FlowLayoutPanel pnlSearch = null!;
    private Button btnSearch = null!;
    private Button btnReset = null!;
    private DataGridViewButtonColumn colView = null!;
    private DataGridViewButtonColumn colEdit = null!;
    private DataGridViewButtonColumn colDelete = null!;

    private void InitializeComponent()
    {
        Text = "Étudiants";
        Size = new Size(1050, 600);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        AutoScroll = true;

        titleBar = UITheme.CreateTitleBar(this, "6. Étudiants - Liste & Dossiers", "IIT");

        contentPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            BackColor = Color.Transparent,
            AutoScroll = true
        };

        card = UITheme.CreateCard("Répertoire des étudiants", "");
        card.Dock = DockStyle.Fill;

        // Top actions bar with Search and Add
        topBar = new Panel { Dock = DockStyle.Top, Height = 48, Padding = new Padding(4) };

        btnAdd = UITheme.CreatePrimaryButton("Nouveau dossier", "");
        btnAdd.Dock = DockStyle.Left;
        btnAdd.Click += (s, e) =>
        {
            var dlg = new FormEtudiantDetail();
            dlg.DataSaved += (sender, args) => LoadData();
            dlg.ShowDialog();
        };

        pnlSearch = new FlowLayoutPanel
        {
            Dock = DockStyle.Right,
            Width = 350,
            Height = 42,
            FlowDirection = FlowDirection.LeftToRight
        };

        _txtSearch = new TextBox
        {
            Text = "",
            Font = UITheme.FontRegular(10),
            Width = 230,
            Margin = new Padding(0, 4, 4, 0)
        };
        _txtSearch.PlaceholderText = "Rechercher par nom ou matricule...";
        _txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadData(); };

        btnSearch = UITheme.CreatePrimaryButton("Rechercher");
        btnSearch.Size = new Size(42, 32);
        btnSearch.Margin = new Padding(0, 2, 4, 0);
        btnSearch.Click += (s, e) => LoadData();

        btnReset = UITheme.CreateSecondaryButton("Réinitialiser");
        btnReset.Size = new Size(38, 32);
        btnReset.Margin = new Padding(0, 2, 0, 0);
        btnReset.Click += (s, e) => { _txtSearch.Text = ""; LoadData(); };

        pnlSearch.Controls.Add(_txtSearch);
        pnlSearch.Controls.Add(btnSearch);
        pnlSearch.Controls.Add(btnReset);

        topBar.Controls.Add(btnAdd);
        topBar.Controls.Add(pnlSearch);

        // DataGridView
        _dgvEtudiants = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvEtudiants);

        _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColMatricule", HeaderText = "Matricule", FillWeight = 110, MinimumWidth = 95 });
        _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColNom", HeaderText = "Nom", FillWeight = 110, MinimumWidth = 90 });
        _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPrenom", HeaderText = "Prénom", FillWeight = 120, MinimumWidth = 100 });
        _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColClasse", HeaderText = "Classe", FillWeight = 90, MinimumWidth = 85 });
        _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColStatut", HeaderText = "Statut", FillWeight = 80, MinimumWidth = 80 });

        colView = new DataGridViewButtonColumn { Name = "ColView", HeaderText = "Dossier", Text = "Consulter", UseColumnTextForButtonValue = true, FillWeight = 80, MinimumWidth = 100 };
        colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Éditer", Text = "Modifier", UseColumnTextForButtonValue = true, FillWeight = 75, MinimumWidth = 92 };
        colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "Suppr", Text = "Supprimer", UseColumnTextForButtonValue = true, FillWeight = 85, MinimumWidth = 105 };
        _dgvEtudiants.Columns.AddRange(colView, colEdit, colDelete);
        _dgvEtudiants.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvEtudiants.CellContentClick += DgvEtudiants_CellContentClick;

        card.Controls.Add(_dgvEtudiants);
        card.Controls.Add(topBar);

        contentPanel.Controls.Add(card);
        Controls.Add(contentPanel);
        Controls.Add(titleBar);
    }
}
