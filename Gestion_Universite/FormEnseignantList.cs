using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormEnseignantList : Form
{
    private readonly EnseignantDAO _enseignantDao = new();
    private DataGridView _dgvEnseignants = null!;

    public FormEnseignantList()
    {
        InitializeCustomComponents();
        LoadData();
    }

    private void InitializeCustomComponents()
    {
        Text = "Enseignants";
        Size = new Size(1000, 560);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        var titleBar = UITheme.CreateTitleBar(this, "8. Enseignants - Corps Professoral", "👨‍🏫");
        Controls.Add(titleBar);

        var content = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16), BackColor = Color.Transparent };

        var card = UITheme.CreateCard("Liste des enseignants", "📋");
        card.Dock = DockStyle.Fill;

        var topBar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 45, FlowDirection = FlowDirection.LeftToRight };
        var btnAdd = UITheme.CreatePrimaryButton("Nouvel enseignant", "➕");
        btnAdd.Click += (s, e) =>
        {
            var dlg = new FormEnseignantDetail();
            dlg.DataSaved += (sender, args) => LoadData();
            dlg.ShowDialog();
        };

        var btnRefresh = UITheme.CreateSecondaryButton("Actualiser", "🔄");
        btnRefresh.Click += (s, e) => LoadData();

        topBar.Controls.Add(btnAdd);
        topBar.Controls.Add(btnRefresh);

        _dgvEnseignants = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvEnseignants);

        _dgvEnseignants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvEnseignants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColNom", HeaderText = "Nom complet", FillWeight = 160 });
        _dgvEnseignants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColSpecialite", HeaderText = "Spécialité", FillWeight = 110 });
        _dgvEnseignants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColType", HeaderText = "Type", FillWeight = 90 });
        _dgvEnseignants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColStatut", HeaderText = "Statut", FillWeight = 80 });

        var colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Action", Text = "✏", UseColumnTextForButtonValue = true, FillWeight = 40 };
        var colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "", Text = "🗑", UseColumnTextForButtonValue = true, FillWeight = 40 };
        _dgvEnseignants.Columns.AddRange(colEdit, colDelete);
        _dgvEnseignants.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvEnseignants.CellContentClick += DgvEnseignants_CellContentClick;

        card.Controls.Add(_dgvEnseignants);
        card.Controls.Add(topBar);

        content.Controls.Add(card);
        Controls.Add(content);
    }

    private void LoadData()
    {
        var list = _enseignantDao.GetAll();
        _dgvEnseignants.Rows.Clear();
        foreach (var ens in list)
        {
            _dgvEnseignants.Rows.Add(ens.Id, ens.NomComplet, ens.Specialite, ens.TypeEnseignant, ens.Statut);
        }
    }

    private void DgvEnseignants_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvEnseignants.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvEnseignants.Columns[e.ColumnIndex].Name == "ColEdit")
        {
            var dlg = new FormEnseignantDetail(id);
            dlg.DataSaved += (s, ev) => LoadData();
            dlg.ShowDialog();
        }
        else if (_dgvEnseignants.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Confirmez-vous la suppression de cet enseignant ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _enseignantDao.Delete(id);
                LoadData();
            }
        }
    }
}
