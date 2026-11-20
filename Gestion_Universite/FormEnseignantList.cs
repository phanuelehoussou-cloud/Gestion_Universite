using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormEnseignantList : Form
{
    private readonly EnseignantDAO _enseignantDao = new();


    public FormEnseignantList()
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
        var list = _enseignantDao.GetAll();
        _dgvEnseignants.Rows.Clear();
        foreach (var ens in list)
        {
            _dgvEnseignants.Rows.Add(ens.Id, ens.NomComplet, ens.Specialite, ens.TypeEnseignant, ens.Statut);
        }
    }

    private void DgvEnseignants_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvEnseignants.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvEnseignants.Columns[e.ColumnIndex].Name == "ColEdit")
        {
            var dlg = new FormEnseignantDetail(id);
            dlg.DataSaved += (s, ev) => LoadData();
            dlg.ShowDialog();
        }
        else if (_dgvEnseignants.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Confirmez-vous la suppression de cet enseignant ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _enseignantDao.Delete(id);
                LoadData();
            }
        }
    }
}
