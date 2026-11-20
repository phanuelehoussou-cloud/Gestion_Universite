using DAO.Models;
using DAO.Repositories;
#nullable enable
namespace Gestion_Universite;

partial class FormEnseignantList
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private DataGridView _dgvEnseignants = null!;

    private Panel titleBar = null!;
    private Panel content = null!;
    private Panel card = null!;
    private FlowLayoutPanel topBar = null!;
    private Button btnAdd = null!;
    private Button btnRefresh = null!;
    private DataGridViewButtonColumn colEdit = null!;
    private DataGridViewButtonColumn colDelete = null!;

    private void InitializeComponent()
    {
        Text = "Enseignants";
        Size = new Size(1000, 560);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        AutoScroll = true;

        titleBar = UITheme.CreateTitleBar(this, "8. Enseignants - Corps Professoral", "IIT");

        content = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12), BackColor = Color.Transparent, AutoScroll = true };

        card = UITheme.CreateCard("Liste des enseignants", "");
        card.Dock = DockStyle.Fill;

        topBar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 45, FlowDirection = FlowDirection.LeftToRight };
        btnAdd = UITheme.CreatePrimaryButton("Nouvel enseignant", "");
        btnAdd.Click += (s, e) =>
        {
            var dlg = new FormEnseignantDetail();
            dlg.DataSaved += (sender, args) => LoadData();
            dlg.ShowDialog();
        };

        btnRefresh = UITheme.CreateSecondaryButton("Actualiser", "");
        btnRefresh.Click += (s, e) => LoadData();

        topBar.Controls.Add(btnAdd);
        topBar.Controls.Add(btnRefresh);

        _dgvEnseignants = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvEnseignants);

        _dgvEnseignants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvEnseignants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColNom", HeaderText = "Nom complet", FillWeight = 160, MinimumWidth = 140 });
        _dgvEnseignants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColSpecialite", HeaderText = "Spécialité", FillWeight = 110, MinimumWidth = 100 });
        _dgvEnseignants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColType", HeaderText = "Type", FillWeight = 90, MinimumWidth = 90 });
        _dgvEnseignants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColStatut", HeaderText = "Statut", FillWeight = 80, MinimumWidth = 80 });

        colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Action", Text = "Modifier", UseColumnTextForButtonValue = true, FillWeight = 75, MinimumWidth = 92 };
        colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "", Text = "Supprimer", UseColumnTextForButtonValue = true, FillWeight = 85, MinimumWidth = 105 };
        _dgvEnseignants.Columns.AddRange(colEdit, colDelete);
        _dgvEnseignants.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvEnseignants.CellContentClick += DgvEnseignants_CellContentClick;

        card.Controls.Add(_dgvEnseignants);
        card.Controls.Add(topBar);

        content.Controls.Add(card);
        Controls.Add(content);
        Controls.Add(titleBar);
    }
}
