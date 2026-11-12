using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormEtudiantDetail : Form
{
    private readonly EtudiantDAO _etudiantDao = new();
    private readonly ClasseDAO _classeDao = new();

    private TextBox _txtMatricule = null!;
    private TextBox _txtNom = null!;
    private TextBox _txtPrenoms = null!;
    private TextBox _txtTelephone = null!;
    private TextBox _txtAdresse = null!;
    private TextBox _txtEmail = null!;
    private ComboBox _cboClasse = null!;
    private PictureBox _picPhoto = null!;
    private int _etudiantId = 0;
    private string? _selectedPhotoPath;

    public event EventHandler? DataSaved;

    public FormEtudiantDetail(int etudiantId = 0)
    {
        _etudiantId = etudiantId;
        InitializeCustomComponents();
        LoadLookups();
        if (_etudiantId > 0)
            LoadEtudiantData();
        else
            GenerateMatricule();
    }

    private void InitializeCustomComponents()
    {
        Text = _etudiantId == 0 ? "Nouveau dossier étudiant" : "Dossier étudiant";
        Size = new Size(640, 620);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;
        AutoScroll = true;

        var titleBar = UITheme.CreateTitleBar(this, _etudiantId == 0 ? "6. Étudiants - Nouveau Dossier" : "6. Étudiants - Dossier Détail", "🧑‍🎓", isDialog: true);

        var card = UITheme.CreateCard("Dossier de l'étudiant", "🧑‍🎓");
        card.Dock = DockStyle.Fill;
        card.Padding = new Padding(14);

        // Top section with Photo and Buttons
        var topSection = new Panel { Dock = DockStyle.Top, Height = 135 };

        _picPhoto = new PictureBox
        {
            Size = new Size(100, 115),
            Location = new Point(12, 10),
            BorderStyle = BorderStyle.FixedSingle,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.FromArgb(240, 240, 240)
        };
        CreateDefaultAvatar();

        var btnPhoto = UITheme.CreateSecondaryButton("Photo", "📷");
        btnPhoto.Location = new Point(125, 20);
        btnPhoto.Click += BtnPhoto_Click;

        var btnPieces = UITheme.CreateSecondaryButton("Pièces administratives", "📁");
        btnPieces.Location = new Point(125, 65);
        btnPieces.Click += BtnPieces_Click;

        topSection.Controls.Add(_picPhoto);
        topSection.Controls.Add(btnPhoto);
        topSection.Controls.Add(btnPieces);
        card.Controls.Add(topSection);

        // Fields table
        var pnlForm = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 8,
            Padding = new Padding(6)
        };
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        for (int i = 0; i < 7; i++)
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

        pnlForm.Controls.Add(new Label { Text = "Téléphone :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 3);
        _txtTelephone = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtTelephone, 1, 3);

        pnlForm.Controls.Add(new Label { Text = "Adresse :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 4);
        _txtAdresse = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtAdresse, 1, 4);

        pnlForm.Controls.Add(new Label { Text = "Email :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 5);
        _txtEmail = new TextBox { Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_txtEmail, 1, 5);

        pnlForm.Controls.Add(new Label { Text = "Classe :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 6);
        _cboClasse = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_cboClasse, 1, 6);

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
        Controls.Add(titleBar);
    }

    private void CreateDefaultAvatar()
    {
        var bmp = new Bitmap(100, 115);
        using (var g = Graphics.FromImage(bmp))
        {
            g.Clear(Color.FromArgb(230, 235, 245));
            using var brush = new SolidBrush(UITheme.SidebarRed);
            g.FillEllipse(brush, 28, 20, 44, 44);
            g.FillPie(brush, 10, 65, 80, 80, 180, 180);
        }
        _picPhoto.Image = bmp;
    }

    private void GenerateMatricule()
    {
        var year = DateTime.Now.Year;
        var all = _etudiantDao.GetAll();
        var nextNum = all.Count > 0 ? all.Max(e => e.Id) + 1 : 1;
        _txtMatricule.Text = $"IIT{year}{nextNum:D3}";
    }

    private void LoadLookups()
    {
        _cboClasse.Items.Clear();
        _cboClasse.Items.Add("-- Aucune classe assignée --");
        foreach (var c in _classeDao.GetAll())
        {
            _cboClasse.Items.Add(c.Code);
        }
        _cboClasse.SelectedIndex = 0;
    }

    private void LoadEtudiantData()
    {
        var etu = _etudiantDao.GetById(_etudiantId);
        if (etu != null)
        {
            _txtMatricule.Text = etu.Matricule;
            _txtNom.Text = etu.Nom;
            _txtPrenoms.Text = etu.Prenoms;
            _txtTelephone.Text = etu.Telephone;
            _txtAdresse.Text = etu.Adresse;
            _txtEmail.Text = etu.Email;

            if (!string.IsNullOrEmpty(etu.ClasseActuelle) && _cboClasse.Items.Contains(etu.ClasseActuelle))
            {
                _cboClasse.SelectedItem = etu.ClasseActuelle;
            }

            if (!string.IsNullOrEmpty(etu.PhotoPath) && File.Exists(etu.PhotoPath))
            {
                try
                {
                    _picPhoto.Image = Image.FromFile(etu.PhotoPath);
                }
                catch { }
            }
        }
    }

    private void BtnPhoto_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "Images (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
            Title = "Sélectionner la photo d'identité de l'étudiant"
        };
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            _selectedPhotoPath = ofd.FileName;
            _picPhoto.Image = Image.FromFile(ofd.FileName);
        }
    }

    private void BtnPieces_Click(object? sender, EventArgs e)
    {
        if (_etudiantId == 0)
        {
            MessageBox.Show("Veuillez d'abord enregistrer le dossier de l'étudiant avant de gérer ses pièces administratives.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var dlg = new FormPiecesEtudiant(_etudiantId);
        dlg.ShowDialog();
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtNom.Text) || string.IsNullOrWhiteSpace(_txtPrenoms.Text))
        {
            MessageBox.Show("Le nom et le prénom de l'étudiant sont obligatoires.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedClasse = _cboClasse.SelectedIndex > 0 ? _cboClasse.SelectedItem?.ToString() : null;

        if (_etudiantId == 0)
        {
            var etu = new Etudiant
            {
                Matricule = _txtMatricule.Text.Trim(),
                Nom = _txtNom.Text.Trim(),
                Prenoms = _txtPrenoms.Text.Trim(),
                Telephone = _txtTelephone.Text.Trim(),
                Adresse = _txtAdresse.Text.Trim(),
                Email = _txtEmail.Text.Trim(),
                ClasseActuelle = selectedClasse,
                PhotoPath = _selectedPhotoPath,
                Statut = "Actif"
            };
            _etudiantDao.Add(etu);
            _etudiantId = etu.Id;
            MessageBox.Show("Dossier étudiant créé avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            var etu = _etudiantDao.GetById(_etudiantId);
            if (etu != null)
            {
                etu.Nom = _txtNom.Text.Trim();
                etu.Prenoms = _txtPrenoms.Text.Trim();
                etu.Telephone = _txtTelephone.Text.Trim();
                etu.Adresse = _txtAdresse.Text.Trim();
                etu.Email = _txtEmail.Text.Trim();
                etu.ClasseActuelle = selectedClasse;
                if (!string.IsNullOrEmpty(_selectedPhotoPath))
                    etu.PhotoPath = _selectedPhotoPath;

                _etudiantDao.Update(etu);
                MessageBox.Show("Dossier étudiant mis à jour !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        DataSaved?.Invoke(this, EventArgs.Empty);
        Close();
    }
}
