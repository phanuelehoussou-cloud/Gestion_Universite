using DAO.Models;
using DAO.Repositories;
#nullable enable
namespace Gestion_Universite;

partial class FormEnseignantDetail
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private TextBox _txtNom = null!;
    private TextBox _txtPrenoms = null!;
    private ComboBox _cboSpecialite = null!;
    private ComboBox _cboType = null!;
    private TextBox _txtTelephone = null!;
    private TextBox _txtEmail = null!;
    private ComboBox _cboStatut = null!;

    private Panel titleBar = null!;
    private Panel card = null!;
    private TableLayoutPanel pnlForm = null!;
    private FlowLayoutPanel pnlBtns = null!;
    private Button btnSave = null!;
    private Button btnCancel = null!;

    private void InitializeComponent()
    {
        Text = _enseignantId == 0 ? "Ajouter un enseignant" : "Modifier un enseignant";
        Size = new Size(540, 520);
        MinimumSize = new Size(500, 480);
        AutoScroll = true;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        titleBar = UITheme.CreateTitleBar(this, _enseignantId == 0 ? "8. Enseignants - Nouvel Enseignant" : "8. Enseignants - Édition", "IIT", isDialog: true);

        card = UITheme.CreateCard("Fiche enseignant", "");
        card.Dock = DockStyle.Fill;
        card.Padding = new Padding(16);

        pnlForm = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 8,
            Padding = new Padding(10)
        };
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        for (int i = 0; i < 7; i++)
            pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

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

        pnlBtns = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        btnSave = UITheme.CreatePrimaryButton("Enregistrer");
        btnSave.Click += BtnSave_Click;
        btnCancel = UITheme.CreateSecondaryButton("Fermer");
        btnCancel.Click += (s, e) => Close();
        pnlBtns.Controls.Add(btnSave);
        pnlBtns.Controls.Add(btnCancel);
        pnlForm.Controls.Add(pnlBtns, 1, 7);

        card.Controls.Add(pnlForm);
        Controls.Add(card);
        Controls.Add(titleBar);
    }
}
