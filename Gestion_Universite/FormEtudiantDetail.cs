using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormEtudiantDetail : Form
{
    private readonly EtudiantDAO _etudiantDao = new();
    private readonly ClasseDAO _classeDao = new();


    private int _etudiantId = 0;
    private string? _selectedPhotoPath;

    public event EventHandler? DataSaved;

    public FormEtudiantDetail() : this(0) { }

    public FormEtudiantDetail(int etudiantId)
    {
        _etudiantId = etudiantId;
        InitializeComponent();
        Load += (_, _) =>
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            LoadLookups();
            if (_etudiantId > 0)
                LoadEtudiantData();
            else
                GenerateMatricule();
        };
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
            _dateNaissance.Value = etu.DateNaissance == DateTime.MinValue ? DateTime.Today.AddYears(-18) : etu.DateNaissance;
            _cboSexe.SelectedItem = etu.Sexe;
            _txtNationalite.Text = etu.Nationalite;
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
        if (string.IsNullOrWhiteSpace(_txtNom.Text) || string.IsNullOrWhiteSpace(_txtPrenoms.Text) || string.IsNullOrWhiteSpace(_txtNationalite.Text))
        {
            MessageBox.Show("Le nom, les prénoms et la nationalité de l'étudiant sont obligatoires.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                DateNaissance = _dateNaissance.Value.Date,
                Sexe = _cboSexe.SelectedItem?.ToString() ?? "M",
                Nationalite = _txtNationalite.Text.Trim(),
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
                etu.DateNaissance = _dateNaissance.Value.Date;
                etu.Sexe = _cboSexe.SelectedItem?.ToString() ?? "M";
                etu.Nationalite = _txtNationalite.Text.Trim();
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
