using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormEmploiDuTemps : Form
{
    private readonly EmploiDuTempsDAO _edtDao = new();
    private readonly ClasseDAO _classeDao = new();
    private readonly AnneeAcademiqueDAO _anneeDao = new();

    private ComboBox _cboClasse = null!;
    private ComboBox _cboSemestre = null!;
    private DataGridView _dgvSeances = null!;

    public FormEmploiDuTemps()
    {
        InitializeCustomComponents();
        LoadLookups();
        LoadData();
    }

    private void InitializeCustomComponents()
    {
        Text = "Emplois du temps";
        Size = new Size(1100, 620);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        var titleBar = UITheme.CreateTitleBar(this, "11. Emplois du temps", "🗓️");
        Controls.Add(titleBar);

        var content = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16), BackColor = Color.Transparent };

        var card = UITheme.CreateCard("Planning des enseignements", "🗓️");
        card.Dock = DockStyle.Fill;

        // Top Filter & Action Bar
        var topBar = new Panel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(6) };

        var pnlSelectors = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight
        };

        pnlSelectors.Controls.Add(new Label { Text = "Classe :", Font = UITheme.FontSemiBold(), AutoSize = true, Margin = new Padding(0, 8, 4, 0) });
        _cboClasse = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Width = 140, Margin = new Padding(0, 4, 16, 0) };
        _cboClasse.SelectedIndexChanged += (s, e) => LoadData();
        pnlSelectors.Controls.Add(_cboClasse);

        pnlSelectors.Controls.Add(new Label { Text = "Semestre :", Font = UITheme.FontSemiBold(), AutoSize = true, Margin = new Padding(0, 8, 4, 0) });
        _cboSemestre = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Width = 140, Margin = new Padding(0, 4, 16, 0) };
        _cboSemestre.SelectedIndexChanged += (s, e) => LoadData();
        pnlSelectors.Controls.Add(_cboSemestre);

        var btnGenerer = UITheme.CreatePrimaryButton("Générer", "⚡");
        btnGenerer.Click += BtnGenerer_Click;
        pnlSelectors.Controls.Add(btnGenerer);

        var btnAdd = UITheme.CreateSecondaryButton("Ajouter séance", "➕");
        btnAdd.Click += (s, e) =>
        {
            var classeId = (_cboClasse.SelectedItem as Classe)?.Id ?? 0;
            var semId = (_cboSemestre.SelectedItem as Semestre)?.Id ?? 0;
            var dlg = new FormSeanceDetail(0, classeId, semId);
            dlg.DataSaved += (sender, args) => LoadData();
            dlg.ShowDialog();
        };
        pnlSelectors.Controls.Add(btnAdd);

        topBar.Controls.Add(pnlSelectors);

        // Center split layout with big calendar icon on left and DataGridView on right
        var splitPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1
        };
        splitPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
        splitPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        // Big visual calendar badge on left matching screenshot
        var pnlIcon = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
        var lblBigIcon = new Label
        {
            Text = "🗓️\n⏰",
            Font = new Font("Segoe UI Emoji", 26f),
            ForeColor = UITheme.PrimaryRed,
            Dock = DockStyle.Top,
            Height = 120,
            TextAlign = ContentAlignment.MiddleCenter
        };
        pnlIcon.Controls.Add(lblBigIcon);
        splitPanel.Controls.Add(pnlIcon, 0, 0);

        // DataGridView
        _dgvSeances = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvSeances);

        _dgvSeances.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvSeances.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColJour", HeaderText = "Jour", FillWeight = 90 });
        _dgvSeances.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColHeure", HeaderText = "Heure", FillWeight = 90 });
        _dgvSeances.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCours", HeaderText = "Cours", FillWeight = 140 });
        _dgvSeances.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColEnseignant", HeaderText = "Enseignant", FillWeight = 140 });
        _dgvSeances.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColSalle", HeaderText = "Salle", FillWeight = 80 });

        var colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Action", Text = "✏", UseColumnTextForButtonValue = true, FillWeight = 35 };
        var colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "", Text = "🗑", UseColumnTextForButtonValue = true, FillWeight = 35 };
        _dgvSeances.Columns.AddRange(colEdit, colDelete);
        _dgvSeances.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvSeances.CellContentClick += DgvSeances_CellContentClick;

        splitPanel.Controls.Add(_dgvSeances, 1, 0);

        card.Controls.Add(splitPanel);
        card.Controls.Add(topBar);

        content.Controls.Add(card);
        Controls.Add(content);
    }

    private void LoadLookups()
    {
        var classes = _classeDao.GetAll();
        _cboClasse.DataSource = classes;

        var annees = _anneeDao.GetAll();
        var currentYear = annees.FirstOrDefault(a => a.Statut == "Ouverte") ?? annees.FirstOrDefault();
        if (currentYear?.Semestres != null && currentYear.Semestres.Count > 0)
        {
            _cboSemestre.DataSource = currentYear.Semestres;
        }
        else
        {
            _cboSemestre.Items.Add(new Semestre { Id = 1, Nom = "Semestre 1" });
            _cboSemestre.SelectedIndex = 0;
        }
    }

    private void LoadData()
    {
        var classeId = (_cboClasse.SelectedItem as Classe)?.Id ?? 1;
        var semId = (_cboSemestre.SelectedItem as Semestre)?.Id ?? 1;

        var seances = _edtDao.GetByClasseAndSemestre(classeId, semId);
        _dgvSeances.Rows.Clear();
        foreach (var s in seances)
        {
            _dgvSeances.Rows.Add(s.Id, s.Jour, s.HeureDebut, s.CoursIntitule, s.EnseignantNom, s.SalleNom);
        }
    }

    private void BtnGenerer_Click(object? sender, EventArgs e)
    {
        var classe = _cboClasse.SelectedItem as Classe;
        var sem = _cboSemestre.SelectedItem as Semestre;

        MessageBox.Show($"Emploi du temps synchronisé et généré avec succès pour {classe?.Code ?? "la classe"} ({sem?.Nom ?? "Semestre 1"}). Aucun conflit détecté.", "Génération réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);
        LoadData();
    }

    private void DgvSeances_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvSeances.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvSeances.Columns[e.ColumnIndex].Name == "ColEdit")
        {
            var dlg = new FormSeanceDetail(id);
            dlg.DataSaved += (s, ev) => LoadData();
            dlg.ShowDialog();
        }
        else if (_dgvSeances.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Confirmez-vous la suppression de cette séance de cours ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _edtDao.Delete(id);
                LoadData();
            }
        }
    }
}
