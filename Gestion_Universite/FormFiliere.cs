using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormFiliere : Form
{
    private readonly FiliereDAO _filiereDao = new();

    private int _editingId = 0;

    public FormFiliere()
    {
        InitializeComponent();
        _cboStatut.SelectedIndex = 0;
        Load += FormFiliere_Load;
    }

    private void FormFiliere_Load(object? sender, EventArgs e)
    {
        if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
        LoadData();
    }

    private void BtnReset_Click(object? sender, EventArgs e) => ResetForm();


    private void LoadData()
    {
        var filieres = _filiereDao.GetAll();
        _dgvFilieres.Rows.Clear();
        foreach (var f in filieres)
        {
            _dgvFilieres.Rows.Add(f.Id, f.Nom, f.Statut);
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtNom.Text))
        {
            MessageBox.Show("Veuillez saisir le nom de la filière.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_editingId == 0)
        {
            var filiere = new Filiere
            {
                Nom = _txtNom.Text.Trim(),
                Description = _txtDescription.Text.Trim(),
                Statut = _cboStatut.SelectedItem?.ToString() ?? "Active"
            };
            _filiereDao.Add(filiere);
            MessageBox.Show("Filière enregistrée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            var filiere = _filiereDao.GetById(_editingId);
            if (filiere != null)
            {
                filiere.Nom = _txtNom.Text.Trim();
                filiere.Description = _txtDescription.Text.Trim();
                filiere.Statut = _cboStatut.SelectedItem?.ToString() ?? "Active";
                _filiereDao.Update(filiere);
                MessageBox.Show("Filière modifiée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        ResetForm();
        LoadData();
    }

    private void DgvFilieres_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvFilieres.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvFilieres.Columns[e.ColumnIndex].Name == "ColEdit")
        {
            var filiere = _filiereDao.GetById(id);
            if (filiere != null)
            {
                _editingId = filiere.Id;
                _txtNom.Text = filiere.Nom;
                _txtDescription.Text = filiere.Description;
                _cboStatut.SelectedItem = filiere.Statut;
            }
        }
        else if (_dgvFilieres.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Confirmez-vous la suppression de cette filière ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _filiereDao.Delete(id);
                LoadData();
            }
        }
    }

    private void ResetForm()
    {
        _editingId = 0;
        _txtNom.Text = "";
        _txtDescription.Text = "";
        _cboStatut.SelectedIndex = 0;
    }
}
