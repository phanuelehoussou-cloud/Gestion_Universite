using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormClasseList : Form
{
    private readonly ClasseDAO _classeDao = new();

    public FormClasseList()
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
        var classes = _classeDao.GetAll();
        _dgvClasses.Rows.Clear();
        foreach (var classe in classes)
        {
            _dgvClasses.Rows.Add(classe.Id, classe.Code, classe.Details);
        }
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        using var dialog = new FormClasseDetail();
        dialog.DataSaved += (_, _) => LoadData();
        dialog.ShowDialog(this);
    }

    private void BtnRefresh_Click(object? sender, EventArgs e) => LoadData();

    private void DgvClasses_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

        var id = Convert.ToInt32(_dgvClasses.Rows[e.RowIndex].Cells["ColId"].Value);
        var action = _dgvClasses.Columns[e.ColumnIndex].Name;

        if (action == "ColEdit")
        {
            using var dialog = new FormClasseDetail(id);
            dialog.DataSaved += (_, _) => LoadData();
            dialog.ShowDialog(this);
        }
        else if (action == "ColDelete" && MessageBox.Show(
            this,
            "Confirmez-vous la suppression de cette classe ?",
            "Confirmation",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question) == DialogResult.Yes)
        {
            _classeDao.Delete(id);
            LoadData();
        }
    }
}
