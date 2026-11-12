using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormFiliere : Form
{
    private readonly FiliereDAO _filiereDao = new();
    private TextBox _txtNom = null!;
    private TextBox _txtDescription = null!;
    private ComboBox _cboStatut = null!;
    private DataGridView _dgvFilieres = null!;
    private int _editingId = 0;

    public FormFiliere()
    {
        InitializeCustomComponents();
        LoadData();
    }

    private void InitializeCustomComponents()
    {
        Text = "Filières";
        Size = new Size(1000, 560);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        var titleBar = UITheme.CreateTitleBar(this, "3. Filières", "🎓");
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
        var cardForm = UITheme.CreateCard("Filières", "🎓");
        cardForm.Dock = DockStyle.Fill;

        var pnlForm = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 5,
            Padding = new Padding(8)
        };
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        pnlForm.Controls.Add(new Label { Text = "Nom de la filière :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _txtNom = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtNom, 1, 0);

        pnlForm.Controls.Add(new Label { Text = "Description :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Left }, 0, 1);
        _txtDescription = new TextBox { Font = UITheme.FontRegular(10), Multiline = true, Height = 90, Dock = DockStyle.Fill, ScrollBars = ScrollBars.Vertical };
        pnlForm.Controls.Add(_txtDescription, 1, 1);

        pnlForm.Controls.Add(new Label { Text = "Statut :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _cboStatut = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboStatut.Items.AddRange(new object[] { "Active", "Inactive" });
        _cboStatut.SelectedIndex = 0;
        pnlForm.Controls.Add(_cboStatut, 1, 2);

        var pnlBtns = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        var btnSave = UITheme.CreatePrimaryButton("Enregistrer");
        btnSave.Click += BtnSave_Click;
        var btnReset = UITheme.CreateSecondaryButton("Réinitialiser");
        btnReset.Click += (s, e) => ResetForm();
        pnlBtns.Controls.Add(btnSave);
        pnlBtns.Controls.Add(btnReset);
        pnlForm.Controls.Add(pnlBtns, 1, 3);

        cardForm.Controls.Add(pnlForm);
        mainPanel.Controls.Add(cardForm, 0, 0);

        // RIGHT CARD: Liste des filières
        var cardList = UITheme.CreateCard("Liste des filières", "📋");
        cardList.Dock = DockStyle.Fill;

        _dgvFilieres = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvFilieres);

        _dgvFilieres.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvFilieres.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColNom", HeaderText = "Filière", FillWeight = 160 });
        _dgvFilieres.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColStatut", HeaderText = "Statut", FillWeight = 80 });

        var colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Action", Text = "✏", UseColumnTextForButtonValue = true, FillWeight = 40 };
        var colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "", Text = "🗑", UseColumnTextForButtonValue = true, FillWeight = 40 };
        _dgvFilieres.Columns.AddRange(colEdit, colDelete);
        _dgvFilieres.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvFilieres.CellContentClick += DgvFilieres_CellContentClick;

        cardList.Controls.Add(_dgvFilieres);
        mainPanel.Controls.Add(cardList, 1, 0);

        Controls.Add(mainPanel);
    }

    private void LoadData()
    {
        var filieres = _filiereDao.GetAll();
        _dgvFilieres.Rows.Clear();
        foreach (var f in filieres)
        {
            _dgvFilieres.Rows.Add(f.Id, f.Nom, f.Statut);
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtNom.Text))
        {
            MessageBox.Show("Veuillez saisir le nom de la filière.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_editingId == 0)
        {
            var filiere = new Filiere
            {
                Nom = _txtNom.Text.Trim(),
                Description = _txtDescription.Text.Trim(),
                Statut = _cboStatut.SelectedItem?.ToString() ?? "Active"
            };
            _filiereDao.Add(filiere);
            MessageBox.Show("Filière enregistrée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            var filiere = _filiereDao.GetById(_editingId);
            if (filiere != null)
            {
                filiere.Nom = _txtNom.Text.Trim();
                filiere.Description = _txtDescription.Text.Trim();
                filiere.Statut = _cboStatut.SelectedItem?.ToString() ?? "Active";
                _filiereDao.Update(filiere);
                MessageBox.Show("Filière modifiée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        ResetForm();
        LoadData();
    }

    private void DgvFilieres_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvFilieres.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvFilieres.Columns[e.ColumnIndex].Name == "ColEdit")
        {
            var filiere = _filiereDao.GetById(id);
            if (filiere != null)
            {
                _editingId = filiere.Id;
                _txtNom.Text = filiere.Nom;
                _txtDescription.Text = filiere.Description;
                _cboStatut.SelectedItem = filiere.Statut;
            }
        }
        else if (_dgvFilieres.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Confirmez-vous la suppression de cette filière ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _filiereDao.Delete(id);
                LoadData();
            }
        }
    }

    private void ResetForm()
    {
        _editingId = 0;
        _txtNom.Text = "";
        _txtDescription.Text = "";
        _cboStatut.SelectedIndex = 0;
    }
}
