using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormEnseignantDetail : Form
{
    private readonly EnseignantDAO _enseignantDao = new();

    private int _enseignantId = 0;

    public event EventHandler? DataSaved;

    public FormEnseignantDetail() : this(0) { }

    public FormEnseignantDetail(int enseignantId)
    {
        _enseignantId = enseignantId;
        InitializeComponent();
        Load += (_, _) =>
        {
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime && _enseignantId > 0)
                LoadEnseignantData();
        };
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
