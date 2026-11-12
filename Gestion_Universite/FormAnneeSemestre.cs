using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormAnneeSemestre : Form
{
    private readonly AnneeAcademiqueDAO _anneeDao = new();
    private DataGridView _dgvAnnees = null!;
    private TextBox _txtAnnee = null!;
    private DateTimePicker _dtpDebut = null!;
    private DateTimePicker _dtpFin = null!;
    private ComboBox _cboSemestre = null!;
    private ComboBox _cboAnneePourSemestre = null!;
    private DateTimePicker _dtpSemDebut = null!;
    private int _editingAnneeId = 0;

    public FormAnneeSemestre()
    {
        InitializeCustomComponents();
        LoadData();
    }

    private void InitializeCustomComponents()
    {
        Text = "Années académiques et semestres";
        Size = new Size(1050, 620);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        var titleBar = UITheme.CreateTitleBar(this, "2. Années académiques et semestres", "📅");
        Controls.Add(titleBar);

        var mainPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(16),
            BackColor = Color.Transparent
        };
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));

        // LEFT COLUMN: Nouvelle année académique & Ajouter un semestre
        var leftPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true
        };

        // --- Card: Nouvelle année académique ---
        var cardAnnee = UITheme.CreateCard("Nouvelle année académique", "📅");
        cardAnnee.Size = new Size(390, 260);

        var pnlFormAnnee = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            Padding = new Padding(8)
        };
        pnlFormAnnee.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
        pnlFormAnnee.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        pnlFormAnnee.Controls.Add(new Label { Text = "Année :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _txtAnnee = new TextBox { Text = "2026 - 2027", Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlFormAnnee.Controls.Add(_txtAnnee, 1, 0);

        pnlFormAnnee.Controls.Add(new Label { Text = "Date début :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _dtpDebut = new DateTimePicker { Value = new DateTime(2026, 10, 1), Format = DateTimePickerFormat.Short, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlFormAnnee.Controls.Add(_dtpDebut, 1, 1);

        pnlFormAnnee.Controls.Add(new Label { Text = "Date fin :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _dtpFin = new DateTimePicker { Value = new DateTime(2027, 9, 30), Format = DateTimePickerFormat.Short, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlFormAnnee.Controls.Add(_dtpFin, 1, 2);

        var pnlBtnAnnee = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        var btnSaveAnnee = UITheme.CreatePrimaryButton("Enregistrer");
        btnSaveAnnee.Click += BtnSaveAnnee_Click;
        var btnResetAnnee = UITheme.CreateSecondaryButton("Réinitialiser");
        btnResetAnnee.Click += (s, e) => ResetAnneeForm();
        pnlBtnAnnee.Controls.Add(btnSaveAnnee);
        pnlBtnAnnee.Controls.Add(btnResetAnnee);
        pnlFormAnnee.Controls.Add(pnlBtnAnnee, 1, 3);

        cardAnnee.Controls.Add(pnlFormAnnee);
        leftPanel.Controls.Add(cardAnnee);

        // --- Card: Ajouter un semestre ---
        var cardSemestre = UITheme.CreateCard("Ajouter un semestre", "📆");
        cardSemestre.Size = new Size(390, 230);

        var pnlFormSem = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            Padding = new Padding(8)
        };
        pnlFormSem.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
        pnlFormSem.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        pnlFormSem.Controls.Add(new Label { Text = "Année :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _cboAnneePourSemestre = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlFormSem.Controls.Add(_cboAnneePourSemestre, 1, 0);

        pnlFormSem.Controls.Add(new Label { Text = "Semestre :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _cboSemestre = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboSemestre.Items.AddRange(new object[] { "Semestre 1", "Semestre 2", "Semestre 3", "Semestre 4" });
        _cboSemestre.SelectedIndex = 0;
        pnlFormSem.Controls.Add(_cboSemestre, 1, 1);

        pnlFormSem.Controls.Add(new Label { Text = "Date début :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _dtpSemDebut = new DateTimePicker { Value = new DateTime(2026, 10, 1), Format = DateTimePickerFormat.Short, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlFormSem.Controls.Add(_dtpSemDebut, 1, 2);

        var btnAddSem = UITheme.CreatePrimaryButton("Ajouter");
        btnAddSem.Click += BtnAddSem_Click;
        pnlFormSem.Controls.Add(btnAddSem, 1, 3);

        cardSemestre.Controls.Add(pnlFormSem);
        leftPanel.Controls.Add(cardSemestre);

        mainPanel.Controls.Add(leftPanel, 0, 0);

        // RIGHT COLUMN: Liste des années académiques
        var cardListe = UITheme.CreateCard("Liste des années académiques", "📋");
        cardListe.Dock = DockStyle.Fill;

        _dgvAnnees = new DataGridView
        {
            Dock = DockStyle.Fill
        };
        UITheme.StyleDataGridView(_dgvAnnees);

        _dgvAnnees.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvAnnees.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColAnnee", HeaderText = "Année", FillWeight = 110 });
        _dgvAnnees.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColDebut", HeaderText = "Début", FillWeight = 90 });
        _dgvAnnees.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColFin", HeaderText = "Fin", FillWeight = 90 });
        _dgvAnnees.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColStatut", HeaderText = "Statut", FillWeight = 90 });

        var colView = new DataGridViewButtonColumn { Name = "ColView", HeaderText = "Voir", Text = "👁", UseColumnTextForButtonValue = true, FillWeight = 45 };
        var colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Éditer", Text = "✏", UseColumnTextForButtonValue = true, FillWeight = 45 };
        var colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "Suppr", Text = "🗑", UseColumnTextForButtonValue = true, FillWeight = 45 };
        _dgvAnnees.Columns.AddRange(colView, colEdit, colDelete);
        _dgvAnnees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvAnnees.CellContentClick += DgvAnnees_CellContentClick;

        cardListe.Controls.Add(_dgvAnnees);
        mainPanel.Controls.Add(cardListe, 1, 0);

        Controls.Add(mainPanel);
    }

    private void LoadData()
    {
        var annees = _anneeDao.GetAll();
        _dgvAnnees.Rows.Clear();
        _cboAnneePourSemestre.Items.Clear();

        foreach (var a in annees)
        {
            _dgvAnnees.Rows.Add(a.Id, a.Code, a.DateDebut.ToString("dd/MM/yyyy"), a.DateFin.ToString("dd/MM/yyyy"), a.Statut);
            _cboAnneePourSemestre.Items.Add(a);
        }

        if (_cboAnneePourSemestre.Items.Count > 0)
            _cboAnneePourSemestre.SelectedIndex = 0;
    }

    private void BtnSaveAnnee_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtAnnee.Text))
        {
            MessageBox.Show("Veuillez saisir le libellé de l'année académique.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_editingAnneeId == 0)
        {
            var annee = new AnneeAcademique
            {
                Code = _txtAnnee.Text.Trim(),
                DateDebut = _dtpDebut.Value,
                DateFin = _dtpFin.Value,
                Statut = "Ouverte"
            };
            _anneeDao.Add(annee);
            MessageBox.Show("Année académique créée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            var annee = _anneeDao.GetById(_editingAnneeId);
            if (annee != null)
            {
                annee.Code = _txtAnnee.Text.Trim();
                annee.DateDebut = _dtpDebut.Value;
                annee.DateFin = _dtpFin.Value;
                _anneeDao.Update(annee);
                MessageBox.Show("Année académique mise à jour.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        ResetAnneeForm();
        LoadData();
    }

    private void BtnAddSem_Click(object? sender, EventArgs e)
    {
        if (_cboAnneePourSemestre.SelectedItem is not AnneeAcademique selectedAnnee)
        {
            MessageBox.Show("Veuillez sélectionner une année académique.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var sem = new Semestre
        {
            Nom = _cboSemestre.SelectedItem?.ToString() ?? "Semestre 1",
            DateDebut = _dtpSemDebut.Value,
            DateFin = _dtpSemDebut.Value.AddMonths(5)
        };

        _anneeDao.AddSemestre(selectedAnnee.Id, sem);
        MessageBox.Show($"Semestre '{sem.Nom}' rattaché à l'année {selectedAnnee.Code}.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void DgvAnnees_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvAnnees.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvAnnees.Columns[e.ColumnIndex].Name == "ColEdit")
        {
            var annee = _anneeDao.GetById(id);
            if (annee != null)
            {
                _editingAnneeId = annee.Id;
                _txtAnnee.Text = annee.Code;
                _dtpDebut.Value = annee.DateDebut;
                _dtpFin.Value = annee.DateFin;
            }
        }
        else if (_dgvAnnees.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Voulez-vous vraiment supprimer cette année académique ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _anneeDao.Delete(id);
                LoadData();
            }
        }
        else if (_dgvAnnees.Columns[e.ColumnIndex].Name == "ColView")
        {
            var annee = _anneeDao.GetById(id);
            if (annee != null)
            {
                var semestres = string.Join("\n - ", annee.Semestres.Select(s => $"{s.Nom} ({s.DateDebut:dd/MM/yyyy} - {s.DateFin:dd/MM/yyyy})"));
                MessageBox.Show($"Année: {annee.Code}\nStatut: {annee.Statut}\nDébut: {annee.DateDebut:dd/MM/yyyy}\nFin: {annee.DateFin:dd/MM/yyyy}\n\nSemestres:\n - {(string.IsNullOrEmpty(semestres) ? "Aucun semestre rattaché" : semestres)}", "Détails de l'année", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    private void ResetAnneeForm()
    {
        _editingAnneeId = 0;
        _txtAnnee.Text = "";
        _dtpDebut.Value = DateTime.Now;
        _dtpFin.Value = DateTime.Now.AddYears(1);
    }
}
