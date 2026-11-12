using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormInscriptionList : Form
{
    private readonly InscriptionDAO _inscriptionDao = new();
    private DataGridView _dgvInscriptions = null!;

    public FormInscriptionList()
    {
        InitializeCustomComponents();
        LoadData();
    }

    private void InitializeCustomComponents()
    {
        Text = "Inscriptions";
        Size = new Size(1100, 600);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        var titleBar = UITheme.CreateTitleBar(this, "7. Inscriptions & Paiements - Liste", "📝");
        Controls.Add(titleBar);

        var content = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16), BackColor = Color.Transparent };

        var card = UITheme.CreateCard("Registre des inscriptions", "📋");
        card.Dock = DockStyle.Fill;

        var topBar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 45, FlowDirection = FlowDirection.LeftToRight };
        var btnAdd = UITheme.CreatePrimaryButton("Nouvelle inscription", "➕");
        btnAdd.Click += (s, e) =>
        {
            var dlg = new FormInscriptionDetail();
            dlg.DataSaved += (sender, args) => LoadData();
            dlg.ShowDialog();
        };

        var btnRefresh = UITheme.CreateSecondaryButton("Actualiser", "🔄");
        btnRefresh.Click += (s, e) => LoadData();

        topBar.Controls.Add(btnAdd);
        topBar.Controls.Add(btnRefresh);

        _dgvInscriptions = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvInscriptions);

        _dgvInscriptions.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvInscriptions.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColMatricule", HeaderText = "Matricule", FillWeight = 90 });
        _dgvInscriptions.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColEtudiant", HeaderText = "Étudiant", FillWeight = 140 });
        _dgvInscriptions.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColClasse", HeaderText = "Classe", FillWeight = 85 });
        _dgvInscriptions.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColAnnee", HeaderText = "Année", FillWeight = 85 });
        _dgvInscriptions.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPaye", HeaderText = "Payé (FCFA)", FillWeight = 95 });
        _dgvInscriptions.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColSolde", HeaderText = "Solde (FCFA)", FillWeight = 95 });
        _dgvInscriptions.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColStatut", HeaderText = "Statut", FillWeight = 75 });

        var colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Détail/Paiement", Text = "💳", UseColumnTextForButtonValue = true, FillWeight = 60 };
        var colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "Suppr", Text = "🗑", UseColumnTextForButtonValue = true, FillWeight = 40 };
        _dgvInscriptions.Columns.AddRange(colEdit, colDelete);
        _dgvInscriptions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvInscriptions.CellContentClick += DgvInscriptions_CellContentClick;

        card.Controls.Add(_dgvInscriptions);
        card.Controls.Add(topBar);

        content.Controls.Add(card);
        Controls.Add(content);
    }

    private void LoadData()
    {
        var list = _inscriptionDao.GetAll();
        _dgvInscriptions.Rows.Clear();
        foreach (var i in list)
        {
            _dgvInscriptions.Rows.Add(
                i.Id,
                i.EtudiantMatricule ?? "N/A",
                i.EtudiantNomComplet ?? "N/A",
                i.ClasseCode ?? "N/A",
                i.AnneeCode ?? "N/A",
                $"{i.MontantPaye:N0} FCFA",
                $"{i.SoldeActuel:N0} FCFA",
                i.Statut
            );
        }
    }

    private void DgvInscriptions_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvInscriptions.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvInscriptions.Columns[e.ColumnIndex].Name == "ColEdit")
        {
            var dlg = new FormInscriptionDetail(id);
            dlg.DataSaved += (s, ev) => LoadData();
            dlg.ShowDialog();
        }
        else if (_dgvInscriptions.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Confirmez-vous l'annulation ou la suppression de cette inscription ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _inscriptionDao.Delete(id);
                LoadData();
            }
        }
    }
}
