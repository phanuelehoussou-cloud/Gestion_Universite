using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormEtudiantList : Form
{
    private readonly EtudiantDAO _etudiantDao = new();
    private DataGridView _dgvEtudiants = null!;
    private TextBox _txtSearch = null!;

    public FormEtudiantList()
    {
        InitializeCustomComponents();
        LoadData();
    }

    private void InitializeCustomComponents()
    {
        Text = "Étudiants";
        Size = new Size(1050, 600);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        var titleBar = UITheme.CreateTitleBar(this, "6. Étudiants - Liste & Dossiers", "🧑‍🎓");
        Controls.Add(titleBar);

        var contentPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(16),
            BackColor = Color.Transparent
        };

        var card = UITheme.CreateCard("Répertoire des étudiants", "📋");
        card.Dock = DockStyle.Fill;

        // Top actions bar with Search and Add
        var topBar = new Panel { Dock = DockStyle.Top, Height = 50, Padding = new Padding(6) };

        var btnAdd = UITheme.CreatePrimaryButton("Nouveau dossier", "➕");
        btnAdd.Location = new Point(6, 6);
        btnAdd.Click += (s, e) =>
        {
            var dlg = new FormEtudiantDetail();
            dlg.DataSaved += (sender, args) => LoadData();
            dlg.ShowDialog();
        };

        var pnlSearch = new Panel { Size = new Size(340, 36), Location = new Point(card.Width - 370, 6), Anchor = AnchorStyles.Top | AnchorStyles.Right };
        _txtSearch = new TextBox
        {
            Text = "",
            Font = UITheme.FontRegular(10),
            Size = new Size(240, 32),
            Location = new Point(0, 4)
        };
        _txtSearch.PlaceholderText = "Rechercher par nom ou matricule...";
        _txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadData(); };

        var btnSearch = UITheme.CreatePrimaryButton("🔍");
        btnSearch.Location = new Point(245, 1);
        btnSearch.Size = new Size(45, 34);
        btnSearch.Click += (s, e) => LoadData();

        var btnReset = UITheme.CreateSecondaryButton("✕");
        btnReset.Location = new Point(295, 1);
        btnReset.Size = new Size(40, 34);
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
        _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColMatricule", HeaderText = "Matricule", FillWeight = 110 });
        _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColNom", HeaderText = "Nom", FillWeight = 110 });
        _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPrenom", HeaderText = "Prénom", FillWeight = 120 });
        _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColClasse", HeaderText = "Classe", FillWeight = 90 });
        _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColStatut", HeaderText = "Statut", FillWeight = 80 });

        var colView = new DataGridViewButtonColumn { Name = "ColView", HeaderText = "Dossier", Text = "👁", UseColumnTextForButtonValue = true, FillWeight = 40 };
        var colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Éditer", Text = "✏", UseColumnTextForButtonValue = true, FillWeight = 40 };
        var colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "Suppr", Text = "🗑", UseColumnTextForButtonValue = true, FillWeight = 40 };
        _dgvEtudiants.Columns.AddRange(colView, colEdit, colDelete);
        _dgvEtudiants.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvEtudiants.CellContentClick += DgvEtudiants_CellContentClick;

        card.Controls.Add(_dgvEtudiants);
        card.Controls.Add(topBar);

        contentPanel.Controls.Add(card);
        Controls.Add(contentPanel);
    }

    private void LoadData()
    {
        var list = string.IsNullOrWhiteSpace(_txtSearch.Text) ? _etudiantDao.GetAll() : _etudiantDao.Search(_txtSearch.Text);
        _dgvEtudiants.Rows.Clear();
        foreach (var e in list)
        {
            _dgvEtudiants.Rows.Add(e.Id, e.Matricule, e.Nom, e.Prenoms, e.ClasseActuelle ?? "Non assigné", e.Statut);
        }
    }

    private void DgvEtudiants_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvEtudiants.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvEtudiants.Columns[e.ColumnIndex].Name == "ColEdit" || _dgvEtudiants.Columns[e.ColumnIndex].Name == "ColView")
        {
            var dlg = new FormEtudiantDetail(id);
            dlg.DataSaved += (s, ev) => LoadData();
            dlg.ShowDialog();
        }
        else if (_dgvEtudiants.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Confirmez-vous la suppression de cet étudiant ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _etudiantDao.Delete(id);
                LoadData();
            }
        }
    }
}
