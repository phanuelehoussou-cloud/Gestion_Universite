using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormEtudiantList : Form
{
    private readonly EtudiantDAO _etudiantDao = new();


    public FormEtudiantList()
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
        var list = string.IsNullOrWhiteSpace(_txtSearch.Text) ? _etudiantDao.GetAll() : _etudiantDao.Search(_txtSearch.Text);
        _dgvEtudiants.Rows.Clear();
        foreach (var e in list)
        {
            _dgvEtudiants.Rows.Add(e.Id, e.Matricule, e.Nom, e.Prenoms, e.ClasseActuelle ?? "Non assigné", e.Statut);
        }
    }

    private void DgvEtudiants_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvEtudiants.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvEtudiants.Columns[e.ColumnIndex].Name == "ColEdit" || _dgvEtudiants.Columns[e.ColumnIndex].Name == "ColView")
        {
            var dlg = new FormEtudiantDetail(id);
            dlg.DataSaved += (s, ev) => LoadData();
            dlg.ShowDialog();
        }
        else if (_dgvEtudiants.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Confirmez-vous la suppression de cet étudiant ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _etudiantDao.Delete(id);
                LoadData();
            }
        }
    }
}
