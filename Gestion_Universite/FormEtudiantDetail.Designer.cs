using DAO.Models;
using DAO.Repositories;
#nullable enable
namespace Gestion_Universite;

partial class FormEtudiantDetail
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private TextBox _txtMatricule = null!;
    private TextBox _txtNom = null!;
    private TextBox _txtPrenoms = null!;
    private DateTimePicker _dateNaissance = null!;
    private ComboBox _cboSexe = null!;
    private TextBox _txtNationalite = null!;
    private TextBox _txtTelephone = null!;
    private TextBox _txtAdresse = null!;
    private TextBox _txtEmail = null!;
    private ComboBox _cboClasse = null!;
    private PictureBox _picPhoto = null!;

    private Panel titleBar = null!;
    private Panel card = null!;
    private Panel topSection = null!;
    private Button btnPhoto = null!;
    private Button btnPieces = null!;
    private TableLayoutPanel pnlForm = null!;
    private FlowLayoutPanel pnlBtns = null!;
    private Button btnSave = null!;
    private Button btnCancel = null!;

    private void InitializeComponent()
    {
        Text = _etudiantId == 0 ? "Nouveau dossier étudiant" : "Dossier étudiant";
        Size = new Size(640, 730);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;
        AutoScroll = true;

        titleBar = UITheme.CreateTitleBar(this, _etudiantId == 0 ? "6. Étudiants - Nouveau Dossier" : "6. Étudiants - Dossier Détail", "IIT", isDialog: true);

        card = UITheme.CreateCard("Dossier de l'étudiant", "");
        card.Dock = DockStyle.Fill;
        card.Padding = new Padding(14);

        // Top section with Photo and Buttons
        topSection = new Panel { Dock = DockStyle.Top, Height = 135 };

        _picPhoto = new PictureBox
        {
            Size = new Size(100, 115),
            Location = new Point(12, 10),
            BorderStyle = BorderStyle.FixedSingle,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.FromArgb(240, 240, 240)
        };
        CreateDefaultAvatar();

        btnPhoto = UITheme.CreateSecondaryButton("Photo", "");
        btnPhoto.Location = new Point(125, 20);
        btnPhoto.Click += BtnPhoto_Click;

        btnPieces = UITheme.CreateSecondaryButton("Pièces administratives", "");
        btnPieces.Location = new Point(125, 65);
        btnPieces.Click += BtnPieces_Click;

        topSection.Controls.Add(_picPhoto);
        topSection.Controls.Add(btnPhoto);
        topSection.Controls.Add(btnPieces);
        card.Controls.Add(topSection);

        // Fields table
        pnlForm = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 11,
            Padding = new Padding(6)
        };
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        for (int i = 0; i < 10; i++)
        {
            pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        }
        pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

        pnlForm.Controls.Add(new Label { Text = "Matricule :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _txtMatricule = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill, ReadOnly = true, BackColor = Color.FromArgb(248, 249, 250) };
        pnlForm.Controls.Add(_txtMatricule, 1, 0);

        pnlForm.Controls.Add(new Label { Text = "Nom :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _txtNom = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtNom, 1, 1);

        pnlForm.Controls.Add(new Label { Text = "Prénoms :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _txtPrenoms = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtPrenoms, 1, 2);

        pnlForm.Controls.Add(new Label { Text = "Naissance :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 3);
        _dateNaissance = new DateTimePicker { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, MaxDate = DateTime.Today };
        pnlForm.Controls.Add(_dateNaissance, 1, 3);

        pnlForm.Controls.Add(new Label { Text = "Sexe :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 4);
        _cboSexe = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboSexe.Items.AddRange(new object[] { "M", "F" });
        _cboSexe.SelectedIndex = 0;
        pnlForm.Controls.Add(_cboSexe, 1, 4);

        pnlForm.Controls.Add(new Label { Text = "Nationalité :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 5);
        _txtNationalite = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill, Text = "Ivoirienne" };
        pnlForm.Controls.Add(_txtNationalite, 1, 5);

        pnlForm.Controls.Add(new Label { Text = "Téléphone :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 6);
        _txtTelephone = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtTelephone, 1, 6);

        pnlForm.Controls.Add(new Label { Text = "Adresse :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 7);
        _txtAdresse = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtAdresse, 1, 7);

        pnlForm.Controls.Add(new Label { Text = "Email :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 8);
        _txtEmail = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtEmail, 1, 8);

        pnlForm.Controls.Add(new Label { Text = "Classe actuelle :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 9);
        _cboClasse = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill, Enabled = false };
        pnlForm.Controls.Add(_cboClasse, 1, 9);

        pnlBtns = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        btnSave = UITheme.CreatePrimaryButton("Enregistrer");
        btnSave.Click += BtnSave_Click;
        btnCancel = UITheme.CreateSecondaryButton("Fermer");
        btnCancel.Click += (s, e) => Close();
        pnlBtns.Controls.Add(btnSave);
        pnlBtns.Controls.Add(btnCancel);
        pnlForm.Controls.Add(pnlBtns, 1, 10);

        card.Controls.Add(pnlForm);
        Controls.Add(card);
        Controls.Add(titleBar);
    }
}
