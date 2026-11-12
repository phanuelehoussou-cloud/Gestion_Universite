using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormCours : Form
{
    private readonly CoursDAO _coursDao = new();
    private TextBox _txtCode = null!;
    private TextBox _txtIntitule = null!;
    private TextBox _txtDescription = null!;
    private TextBox _txtVolume = null!;
    private DataGridView _dgvCours = null!;
    private int _editingId = 0;

    public FormCours()
    {
        InitializeCustomComponents();
        LoadData();
    }

    private void InitializeCustomComponents()
    {
        Text = "Cours";
        Size = new Size(1000, 560);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        var titleBar = UITheme.CreateTitleBar(this, "9. Cours & Matières", "📚");
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
        var cardForm = UITheme.CreateCard("Cours", "📚");
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

        pnlForm.Controls.Add(new Label { Text = "Code :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _txtCode = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtCode, 1, 0);

        pnlForm.Controls.Add(new Label { Text = "Intitulé :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _txtIntitule = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtIntitule, 1, 1);

        pnlForm.Controls.Add(new Label { Text = "Description :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _txtDescription = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtDescription, 1, 2);

        pnlForm.Controls.Add(new Label { Text = "Volume horaire :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 3);
        _txtVolume = new TextBox { Font = UITheme.FontRegular(10), Text = "45 h", Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtVolume, 1, 3);

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

        // RIGHT CARD: Liste des cours
        var cardList = UITheme.CreateCard("Liste des cours", "📋");
        cardList.Dock = DockStyle.Fill;

        _dgvCours = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvCours);

        _dgvCours.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvCours.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCode", HeaderText = "Code", FillWeight = 90 });
        _dgvCours.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColIntitule", HeaderText = "Intitulé", FillWeight = 160 });
        _dgvCours.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColVH", HeaderText = "VH", FillWeight = 60 });

        var colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Action", Text = "✏", UseColumnTextForButtonValue = true, FillWeight = 35 };
        var colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "", Text = "🗑", UseColumnTextForButtonValue = true, FillWeight = 35 };
        _dgvCours.Columns.AddRange(colEdit, colDelete);
        _dgvCours.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvCours.CellContentClick += DgvCours_CellContentClick;

        cardList.Controls.Add(_dgvCours);
        mainPanel.Controls.Add(cardList, 1, 0);

        Controls.Add(mainPanel);
    }

    private void LoadData()
    {
        var list = _coursDao.GetAll();
        _dgvCours.Rows.Clear();
        foreach (var c in list)
        {
            _dgvCours.Rows.Add(c.Id, c.Code, c.Intitule, $"{c.VolumeHoraire} h");
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtCode.Text) || string.IsNullOrWhiteSpace(_txtIntitule.Text))
        {
            MessageBox.Show("Le code et l'intitulé du cours sont obligatoires.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int.TryParse(new string(_txtVolume.Text.Where(char.IsDigit).ToArray()), out var vh);
        if (vh <= 0) vh = 45;

        if (_editingId == 0)
        {
            var cours = new Cours
            {
                Code = _txtCode.Text.Trim(),
                Intitule = _txtIntitule.Text.Trim(),
                Description = _txtDescription.Text.Trim(),
                VolumeHoraire = vh
            };
            _coursDao.Add(cours);
            MessageBox.Show("Cours enregistré avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            var cours = _coursDao.GetById(_editingId);
            if (cours != null)
            {
                cours.Code = _txtCode.Text.Trim();
                cours.Intitule = _txtIntitule.Text.Trim();
                cours.Description = _txtDescription.Text.Trim();
                cours.VolumeHoraire = vh;
                _coursDao.Update(cours);
                MessageBox.Show("Cours mis à jour avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        ResetForm();
        LoadData();
    }

    private void DgvCours_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvCours.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvCours.Columns[e.ColumnIndex].Name == "ColEdit")
        {
            var cours = _coursDao.GetById(id);
            if (cours != null)
            {
                _editingId = cours.Id;
                _txtCode.Text = cours.Code;
                _txtIntitule.Text = cours.Intitule;
                _txtDescription.Text = cours.Description;
                _txtVolume.Text = $"{cours.VolumeHoraire} h";
            }
        }
        else if (_dgvCours.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Confirmez-vous la suppression de ce cours ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _coursDao.Delete(id);
                LoadData();
            }
        }
    }

    private void ResetForm()
    {
        _editingId = 0;
        _txtCode.Text = "";
        _txtIntitule.Text = "";
        _txtDescription.Text = "";
        _txtVolume.Text = "45 h";
    }
}
