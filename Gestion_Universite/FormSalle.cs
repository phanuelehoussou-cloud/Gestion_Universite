using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormSalle : Form
{
    private readonly SalleDAO _salleDao = new();
    private TextBox _txtNom = null!;
    private TextBox _txtCapacite = null!;
    private ComboBox _cboLocalisation = null!;
    private ComboBox _cboStatut = null!;
    private DataGridView _dgvSalles = null!;
    private int _editingId = 0;

    public FormSalle()
    {
        InitializeCustomComponents();
        LoadData();
    }

    private void InitializeCustomComponents()
    {
        Text = "Salles";
        Size = new Size(1000, 560);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        var titleBar = UITheme.CreateTitleBar(this, "10. Salles de Cours", "🏛️");
        Controls.Add(titleBar);

        var mainPanel = new TableLayoutPanel
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
        var cardForm = UITheme.CreateCard("Salles", "🏛️");
        cardForm.Dock = DockStyle.Fill;

        var pnlForm = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 5,
            Padding = new Padding(8)
        };
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        pnlForm.Controls.Add(new Label { Text = "Nom :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _txtNom = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtNom, 1, 0);

        pnlForm.Controls.Add(new Label { Text = "Capacité :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _txtCapacite = new TextBox { Font = UITheme.FontRegular(10), Text = "80", Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtCapacite, 1, 1);

        pnlForm.Controls.Add(new Label { Text = "Localisation :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _cboLocalisation = new ComboBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboLocalisation.Items.AddRange(new object[] { "Bâtiment A", "Bâtiment B", "Bâtiment C", "Amphithéâtre" });
        _cboLocalisation.SelectedIndex = 0;
        pnlForm.Controls.Add(_cboLocalisation, 1, 2);

        pnlForm.Controls.Add(new Label { Text = "Statut :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 3);
        _cboStatut = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboStatut.Items.AddRange(new object[] { "Disponible", "En maintenance", "Occupée" });
        _cboStatut.SelectedIndex = 0;
        pnlForm.Controls.Add(_cboStatut, 1, 3);

        var pnlBtns = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        var btnSave = UITheme.CreatePrimaryButton("Enregistrer");
        btnSave.Click += BtnSave_Click;
        var btnReset = UITheme.CreateSecondaryButton("Réinitialiser");
        btnReset.Click += (s, e) => ResetForm();
        pnlBtns.Controls.Add(btnSave);
        pnlBtns.Controls.Add(btnReset);
        pnlForm.Controls.Add(pnlBtns, 1, 4);

        cardForm.Controls.Add(pnlForm);
        mainPanel.Controls.Add(cardForm, 0, 0);

        // RIGHT CARD: Liste des salles
        var cardList = UITheme.CreateCard("Liste des salles", "📋");
        cardList.Dock = DockStyle.Fill;

        _dgvSalles = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvSalles);

        _dgvSalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvSalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColNom", HeaderText = "Nom", FillWeight = 90 });
        _dgvSalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCapacite", HeaderText = "Capacité", FillWeight = 80 });
        _dgvSalles.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColStatut", HeaderText = "Statut", FillWeight = 110 });

        var colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Action", Text = "✏", UseColumnTextForButtonValue = true, FillWeight = 35 };
        var colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "", Text = "🗑", UseColumnTextForButtonValue = true, FillWeight = 35 };
        _dgvSalles.Columns.AddRange(colEdit, colDelete);
        _dgvSalles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvSalles.CellContentClick += DgvSalles_CellContentClick;

        cardList.Controls.Add(_dgvSalles);
        mainPanel.Controls.Add(cardList, 1, 0);

        Controls.Add(mainPanel);
    }

    private void LoadData()
    {
        var list = _salleDao.GetAll();
        _dgvSalles.Rows.Clear();
        foreach (var s in list)
        {
            _dgvSalles.Rows.Add(s.Id, s.Nom, s.Capacite, s.Statut);
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtNom.Text))
        {
            MessageBox.Show("Veuillez saisir le nom de la salle.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int.TryParse(_txtCapacite.Text.Trim(), out var cap);
        if (cap <= 0) cap = 50;

        if (_editingId == 0)
        {
            var salle = new Salle
            {
                Nom = _txtNom.Text.Trim(),
                Capacite = cap,
                Localisation = _cboLocalisation.Text.Trim(),
                Statut = _cboStatut.SelectedItem?.ToString() ?? "Disponible"
            };
            _salleDao.Add(salle);
            MessageBox.Show("Salle enregistrée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            var salle = _salleDao.GetById(_editingId);
            if (salle != null)
            {
                salle.Nom = _txtNom.Text.Trim();
                salle.Capacite = cap;
                salle.Localisation = _cboLocalisation.Text.Trim();
                salle.Statut = _cboStatut.SelectedItem?.ToString() ?? "Disponible";
                _salleDao.Update(salle);
                MessageBox.Show("Salle mise à jour avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        ResetForm();
        LoadData();
    }

    private void DgvSalles_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvSalles.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvSalles.Columns[e.ColumnIndex].Name == "ColEdit")
        {
            var salle = _salleDao.GetById(id);
            if (salle != null)
            {
                _editingId = salle.Id;
                _txtNom.Text = salle.Nom;
                _txtCapacite.Text = salle.Capacite.ToString();
                _cboLocalisation.Text = salle.Localisation;
                _cboStatut.SelectedItem = salle.Statut;
            }
        }
        else if (_dgvSalles.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Confirmez-vous la suppression de cette salle ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _salleDao.Delete(id);
                LoadData();
            }
        }
    }

    private void ResetForm()
    {
        _editingId = 0;
        _txtNom.Text = "";
        _txtCapacite.Text = "80";
        _cboLocalisation.SelectedIndex = 0;
        _cboStatut.SelectedIndex = 0;
    }
}
