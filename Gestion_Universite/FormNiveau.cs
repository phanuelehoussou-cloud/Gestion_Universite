using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormNiveau : Form
{
    private readonly NiveauDAO _niveauDao = new();
    private ComboBox _cboNiveau = null!;
    private ComboBox _cboCycle = null!;
    private DataGridView _dgvNiveaux = null!;
    private int _editingId = 0;

    public FormNiveau()
    {
        InitializeCustomComponents();
        LoadData();
    }

    private void InitializeCustomComponents()
    {
        Text = "Niveaux";
        Size = new Size(950, 520);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        AutoScroll = true;

        var titleBar = UITheme.CreateTitleBar(this, "4. Niveaux", "📊");

        var mainPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(12),
            BackColor = Color.Transparent,
            AutoScroll = true
        };
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));

        // LEFT CARD: Saisie
        var cardForm = UITheme.CreateCard("Niveaux", "📊");
        cardForm.Dock = DockStyle.Fill;

        var pnlForm = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            Padding = new Padding(6)
        };
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        pnlForm.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        pnlForm.Controls.Add(new Label { Text = "Niveau :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _cboNiveau = new ComboBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboNiveau.Items.AddRange(new object[] { "L1", "L2", "L3", "M1", "M2", "Doctorat" });
        _cboNiveau.SelectedIndex = 0;
        pnlForm.Controls.Add(_cboNiveau, 1, 0);

        pnlForm.Controls.Add(new Label { Text = "Cycle :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _cboCycle = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboCycle.Items.AddRange(new object[] { "Licence", "Master", "Doctorat" });
        _cboCycle.SelectedIndex = 0;
        pnlForm.Controls.Add(_cboCycle, 1, 1);

        var pnlBtns = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        var btnSave = UITheme.CreatePrimaryButton("Enregistrer");
        btnSave.Click += BtnSave_Click;
        var btnReset = UITheme.CreateSecondaryButton("Réinitialiser");
        btnReset.Click += (s, e) => ResetForm();
        pnlBtns.Controls.Add(btnSave);
        pnlBtns.Controls.Add(btnReset);
        pnlForm.Controls.Add(pnlBtns, 1, 2);

        cardForm.Controls.Add(pnlForm);
        mainPanel.Controls.Add(cardForm, 0, 0);

        // RIGHT CARD: Liste des niveaux
        var cardList = UITheme.CreateCard("Liste des niveaux", "📋");
        cardList.Dock = DockStyle.Fill;

        _dgvNiveaux = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvNiveaux);

        _dgvNiveaux.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvNiveaux.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColNiveau", HeaderText = "Niveau", FillWeight = 120, MinimumWidth = 100 });
        _dgvNiveaux.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCycle", HeaderText = "Cycle", FillWeight = 140, MinimumWidth = 110 });

        var colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Action", Text = "✏", UseColumnTextForButtonValue = true, FillWeight = 40, MinimumWidth = 45 };
        var colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "", Text = "🗑", UseColumnTextForButtonValue = true, FillWeight = 40, MinimumWidth = 45 };
        _dgvNiveaux.Columns.AddRange(colEdit, colDelete);
        _dgvNiveaux.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvNiveaux.CellContentClick += DgvNiveaux_CellContentClick;

        cardList.Controls.Add(_dgvNiveaux);
        mainPanel.Controls.Add(cardList, 1, 0);

        Controls.Add(mainPanel);
        Controls.Add(titleBar);
    }

    private void LoadData()
    {
        var niveaux = _niveauDao.GetAll();
        _dgvNiveaux.Rows.Clear();
        foreach (var n in niveaux)
        {
            _dgvNiveaux.Rows.Add(n.Id, n.Code, n.Cycle);
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        var code = _cboNiveau.Text.Trim();
        if (string.IsNullOrWhiteSpace(code))
        {
            MessageBox.Show("Veuillez indiquer le code du niveau.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_editingId == 0)
        {
            var niv = new Niveau
            {
                Code = code,
                Cycle = _cboCycle.SelectedItem?.ToString() ?? "Licence"
            };
            _niveauDao.Add(niv);
            MessageBox.Show("Niveau enregistré avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            var niv = _niveauDao.GetById(_editingId);
            if (niv != null)
            {
                niv.Code = code;
                niv.Cycle = _cboCycle.SelectedItem?.ToString() ?? "Licence";
                _niveauDao.Update(niv);
                MessageBox.Show("Niveau mis à jour !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        ResetForm();
        LoadData();
    }

    private void DgvNiveaux_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvNiveaux.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvNiveaux.Columns[e.ColumnIndex].Name == "ColEdit")
        {
            var niv = _niveauDao.GetById(id);
            if (niv != null)
            {
                _editingId = niv.Id;
                _cboNiveau.Text = niv.Code;
                _cboCycle.SelectedItem = niv.Cycle;
            }
        }
        else if (_dgvNiveaux.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Confirmez-vous la suppression de ce niveau ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _niveauDao.Delete(id);
                LoadData();
            }
        }
    }

    private void ResetForm()
    {
        _editingId = 0;
        _cboNiveau.SelectedIndex = 0;
        _cboCycle.SelectedIndex = 0;
    }
}
