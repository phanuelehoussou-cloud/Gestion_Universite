using DAO.Models;
using DAO.Repositories;
#nullable enable
namespace Gestion_Universite;

partial class FormInscriptionList
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private DataGridView _dgvInscriptions = null!;

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
        Text = "Inscriptions";
        Size = new Size(1100, 600);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        AutoScroll = true;

        titleBar = UITheme.CreateTitleBar(this, "7. Inscriptions & Paiements - Liste", "IIT");

        content = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12), BackColor = Color.Transparent, AutoScroll = true };

        card = UITheme.CreateCard("Registre des inscriptions", "");
        card.Dock = DockStyle.Fill;

        topBar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 45, FlowDirection = FlowDirection.LeftToRight };
        btnAdd = UITheme.CreatePrimaryButton("Nouvelle inscription", "");
        btnAdd.Click += (s, e) =>
        {
            var dlg = new FormInscriptionDetail();
            dlg.DataSaved += (sender, args) => LoadData();
            dlg.ShowDialog();
        };

        btnRefresh = UITheme.CreateSecondaryButton("Actualiser", "");
        btnRefresh.Click += (s, e) => LoadData();

        topBar.Controls.Add(btnAdd);
        topBar.Controls.Add(btnRefresh);

        _dgvInscriptions = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvInscriptions);

        _dgvInscriptions.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvInscriptions.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColMatricule", HeaderText = "Matricule", FillWeight = 90, MinimumWidth = 85 });
        _dgvInscriptions.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColEtudiant", HeaderText = "Étudiant", FillWeight = 140, MinimumWidth = 120 });
        _dgvInscriptions.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColClasse", HeaderText = "Classe", FillWeight = 85, MinimumWidth = 80 });
        _dgvInscriptions.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColAnnee", HeaderText = "Année", FillWeight = 85, MinimumWidth = 80 });
        _dgvInscriptions.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPaye", HeaderText = "Payé (FCFA)", FillWeight = 95, MinimumWidth = 90 });
        _dgvInscriptions.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColSolde", HeaderText = "Solde (FCFA)", FillWeight = 95, MinimumWidth = 90 });
        _dgvInscriptions.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColStatut", HeaderText = "Statut", FillWeight = 75, MinimumWidth = 75 });

        colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Détail/Paiement", Text = "Ouvrir", UseColumnTextForButtonValue = true, FillWeight = 60, MinimumWidth = 55 };
        colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "Suppr", Text = "Supprimer", UseColumnTextForButtonValue = true, FillWeight = 85, MinimumWidth = 105 };
        _dgvInscriptions.Columns.AddRange(colEdit, colDelete);
        _dgvInscriptions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvInscriptions.CellContentClick += DgvInscriptions_CellContentClick;

        card.Controls.Add(_dgvInscriptions);
        card.Controls.Add(topBar);

        content.Controls.Add(card);
        Controls.Add(content);
        Controls.Add(titleBar);
    }
}
