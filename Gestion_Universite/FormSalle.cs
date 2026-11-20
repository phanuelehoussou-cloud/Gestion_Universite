using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormSalle : Form
{
    private readonly SalleDAO _salleDao = new();

    private int _editingId = 0;

    public FormSalle()
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
        var list = _salleDao.GetAll();
        _dgvSalles.Rows.Clear();
        foreach (var s in list)
        {
            _dgvSalles.Rows.Add(s.Id, s.Nom, s.Capacite, s.Statut);
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtNom.Text))
        {
            MessageBox.Show("Veuillez saisir le nom de la salle.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int.TryParse(_txtCapacite.Text.Trim(), out var cap);
        if (cap <= 0) cap = 50;

        if (_editingId == 0)
        {
            var salle = new Salle
            {
                Nom = _txtNom.Text.Trim(),
                Capacite = cap,
                Localisation = _cboLocalisation.Text.Trim(),
                Statut = _cboStatut.SelectedItem?.ToString() ?? "Disponible"
            };
            _salleDao.Add(salle);
            MessageBox.Show("Salle enregistrée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            var salle = _salleDao.GetById(_editingId);
            if (salle != null)
            {
                salle.Nom = _txtNom.Text.Trim();
                salle.Capacite = cap;
                salle.Localisation = _cboLocalisation.Text.Trim();
                salle.Statut = _cboStatut.SelectedItem?.ToString() ?? "Disponible";
                _salleDao.Update(salle);
                MessageBox.Show("Salle mise à jour avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        ResetForm();
        LoadData();
    }

    private void DgvSalles_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvSalles.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvSalles.Columns[e.ColumnIndex].Name == "ColEdit")
        {
            var salle = _salleDao.GetById(id);
            if (salle != null)
            {
                _editingId = salle.Id;
                _txtNom.Text = salle.Nom;
                _txtCapacite.Text = salle.Capacite.ToString();
                _cboLocalisation.Text = salle.Localisation;
                _cboStatut.SelectedItem = salle.Statut;
            }
        }
        else if (_dgvSalles.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Confirmez-vous la suppression de cette salle ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _salleDao.Delete(id);
                LoadData();
            }
        }
    }

    private void ResetForm()
    {
        _editingId = 0;
        _txtNom.Text = "";
        _txtCapacite.Text = "80";
        _cboLocalisation.SelectedIndex = 0;
        _cboStatut.SelectedIndex = 0;
    }
}
