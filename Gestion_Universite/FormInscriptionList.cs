using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormInscriptionList : Form
{
    private readonly InscriptionDAO _inscriptionDao = new();


    public FormInscriptionList()
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
        var list = _inscriptionDao.GetAll();
        _dgvInscriptions.Rows.Clear();
        foreach (var i in list)
        {
            _dgvInscriptions.Rows.Add(
                i.Id,
                i.EtudiantMatricule ?? "N/A",
                i.EtudiantNomComplet ?? "N/A",
                i.ClasseCode ?? "N/A",
                i.AnneeCode ?? "N/A",
                $"{i.MontantPaye:N0} FCFA",
                $"{i.SoldeActuel:N0} FCFA",
                i.Statut
            );
        }
    }

    private void DgvInscriptions_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvInscriptions.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvInscriptions.Columns[e.ColumnIndex].Name == "ColEdit")
        {
            var dlg = new FormInscriptionDetail(id);
            dlg.DataSaved += (s, ev) => LoadData();
            dlg.ShowDialog();
        }
        else if (_dgvInscriptions.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Confirmez-vous l'annulation ou la suppression de cette inscription ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _inscriptionDao.Delete(id);
                LoadData();
            }
        }
    }
}
