using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormCours : Form
{
    private readonly CoursDAO _coursDao = new();

    private int _editingId = 0;

    public FormCours()
    {
        InitializeComponent();
        Load += (_, _) =>
        {
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
                LoadData();
        };
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
