using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormEnseignantDetail : Form
{
    private readonly EnseignantDAO _enseignantDao = new();
    private TextBox _txtNom = null!;
    private TextBox _txtPrenoms = null!;
    private ComboBox _cboSpecialite = null!;
    private ComboBox _cboType = null!;
    private TextBox _txtTelephone = null!;
    private TextBox _txtEmail = null!;
    private ComboBox _cboStatut = null!;
    private int _enseignantId = 0;

    public event EventHandler? DataSaved;

    public FormEnseignantDetail(int enseignantId = 0)
    {
        _enseignantId = enseignantId;
        InitializeCustomComponents();
        if (_enseignantId > 0)
            LoadEnseignantData();
    }

    private void InitializeCustomComponents()
    {
        Text = _enseignantId == 0 ? "Ajouter un enseignant" : "Modifier un enseignant";
        Size = new Size(540, 480);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        var titleBar = UITheme.CreateTitleBar(this, _enseignantId == 0 ? "8. Enseignants - Nouvel Enseignant" : "8. Enseignants - Édition", "👨‍🏫", isDialog: true);
        Controls.Add(titleBar);

        var card = UITheme.CreateCard("Fiche enseignant", "👨‍🏫");
        card.Dock = DockStyle.Fill;
        card.Padding = new Padding(16);

        var pnlForm = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 8,
            Padding = new Padding(10)
        };
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        pnlForm.Controls.Add(new Label { Text = "Nom :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _txtNom = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtNom, 1, 0);

        pnlForm.Controls.Add(new Label { Text = "Prénoms :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _txtPrenoms = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtPrenoms, 1, 1);

        pnlForm.Controls.Add(new Label { Text = "Spécialité :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _cboSpecialite = new ComboBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboSpecialite.Items.AddRange(new object[] { "Informatique", "Maths", "Physique", "Gestion", "Comptabilité", "Droit", "Anglais" });
        _cboSpecialite.SelectedIndex = 0;
        pnlForm.Controls.Add(_cboSpecialite, 1, 2);

        pnlForm.Controls.Add(new Label { Text = "Type :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 3);
        _cboType = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboType.Items.AddRange(new object[] { "Permanent", "Vacataire" });
        _cboType.SelectedIndex = 0;
        pnlForm.Controls.Add(_cboType, 1, 3);

        pnlForm.Controls.Add(new Label { Text = "Téléphone :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 4);
        _txtTelephone = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtTelephone, 1, 4);

        pnlForm.Controls.Add(new Label { Text = "Email :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 5);
        _txtEmail = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtEmail, 1, 5);

        pnlForm.Controls.Add(new Label { Text = "Statut :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 6);
        _cboStatut = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboStatut.Items.AddRange(new object[] { "Actif", "Inactif" });
        _cboStatut.SelectedIndex = 0;
        pnlForm.Controls.Add(_cboStatut, 1, 6);

        var pnlBtns = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        var btnSave = UITheme.CreatePrimaryButton("Enregistrer");
        btnSave.Click += BtnSave_Click;
        var btnCancel = UITheme.CreateSecondaryButton("Fermer");
        btnCancel.Click += (s, e) => Close();
        pnlBtns.Controls.Add(btnSave);
        pnlBtns.Controls.Add(btnCancel);
        pnlForm.Controls.Add(pnlBtns, 1, 7);

        card.Controls.Add(pnlForm);
        Controls.Add(card);
    }

    private void LoadEnseignantData()
    {
        var ens = _enseignantDao.GetById(_enseignantId);
        if (ens != null)
        {
            _txtNom.Text = ens.Nom;
            _txtPrenoms.Text = ens.Prenoms;
            _cboSpecialite.Text = ens.Specialite;
            _cboType.SelectedItem = ens.TypeEnseignant;
            _txtTelephone.Text = ens.Telephone;
            _txtEmail.Text = ens.Email;
            _cboStatut.SelectedItem = ens.Statut;
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtNom.Text) || string.IsNullOrWhiteSpace(_txtPrenoms.Text))
        {
            MessageBox.Show("Le nom et le prénom de l'enseignant sont obligatoires.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_enseignantId == 0)
        {
            var ens = new Enseignant
            {
                Nom = _txtNom.Text.Trim(),
                Prenoms = _txtPrenoms.Text.Trim(),
                Specialite = _cboSpecialite.Text.Trim(),
                TypeEnseignant = _cboType.SelectedItem?.ToString() ?? "Permanent",
                Telephone = _txtTelephone.Text.Trim(),
                Email = _txtEmail.Text.Trim(),
                Statut = _cboStatut.SelectedItem?.ToString() ?? "Actif"
            };
            _enseignantDao.Add(ens);
            MessageBox.Show("Enseignant enregistré avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            var ens = _enseignantDao.GetById(_enseignantId);
            if (ens != null)
            {
                ens.Nom = _txtNom.Text.Trim();
                ens.Prenoms = _txtPrenoms.Text.Trim();
                ens.Specialite = _cboSpecialite.Text.Trim();
                ens.TypeEnseignant = _cboType.SelectedItem?.ToString() ?? "Permanent";
                ens.Telephone = _txtTelephone.Text.Trim();
                ens.Email = _txtEmail.Text.Trim();
                ens.Statut = _cboStatut.SelectedItem?.ToString() ?? "Actif";
                _enseignantDao.Update(ens);
                MessageBox.Show("Enseignant mis à jour !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        DataSaved?.Invoke(this, EventArgs.Empty);
        Close();
    }
}
