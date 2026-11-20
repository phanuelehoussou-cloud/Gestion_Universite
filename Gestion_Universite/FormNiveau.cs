using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormNiveau : Form
{
    private readonly NiveauDAO _niveauDao = new();

    private int _editingId = 0;

    public FormNiveau()
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
        var niveaux = _niveauDao.GetAll();
        _dgvNiveaux.Rows.Clear();
        foreach (var n in niveaux)
        {
            _dgvNiveaux.Rows.Add(n.Id, n.Code, n.Cycle);
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        var code = _cboNiveau.Text.Trim();
        if (string.IsNullOrWhiteSpace(code))
        {
            MessageBox.Show("Veuillez indiquer le code du niveau.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_editingId == 0)
        {
            var niv = new Niveau
            {
                Code = code,
                Cycle = _cboCycle.SelectedItem?.ToString() ?? "Licence"
            };
            _niveauDao.Add(niv);
            MessageBox.Show("Niveau enregistré avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            var niv = _niveauDao.GetById(_editingId);
            if (niv != null)
            {
                niv.Code = code;
                niv.Cycle = _cboCycle.SelectedItem?.ToString() ?? "Licence";
                _niveauDao.Update(niv);
                MessageBox.Show("Niveau mis à jour !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        ResetForm();
        LoadData();
    }

    private void DgvNiveaux_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvNiveaux.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvNiveaux.Columns[e.ColumnIndex].Name == "ColEdit")
        {
            var niv = _niveauDao.GetById(id);
            if (niv != null)
            {
                _editingId = niv.Id;
                _cboNiveau.Text = niv.Code;
                _cboCycle.SelectedItem = niv.Cycle;
            }
        }
        else if (_dgvNiveaux.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Confirmez-vous la suppression de ce niveau ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _niveauDao.Delete(id);
                LoadData();
            }
        }
    }

    private void ResetForm()
    {
        _editingId = 0;
        _cboNiveau.SelectedIndex = 0;
        _cboCycle.SelectedIndex = 0;
    }
}
