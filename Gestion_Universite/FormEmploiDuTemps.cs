using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormEmploiDuTemps : Form
{
    private readonly EmploiDuTempsDAO _edtDao = new();
    private readonly ClasseDAO _classeDao = new();
    private readonly AnneeAcademiqueDAO _anneeDao = new();



    public FormEmploiDuTemps()
    {
        InitializeComponent();
        Load += (_, _) =>
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            LoadLookups();
            LoadData();
        };
    }



    private void LoadLookups()
    {
        var annees = _anneeDao.GetAll();
        var currentYear = annees.FirstOrDefault(a => a.Statut == "Ouverte") ?? annees.FirstOrDefault();
        var classes = _classeDao.GetAll();
        if (currentYear != null)
            classes = classes.Where(classe => classe.AnneeAcademiqueId == currentYear.Id).ToList();

        _cboClasse.DisplayMember = nameof(Classe.Code);
        _cboClasse.ValueMember = nameof(Classe.Id);
        _cboClasse.DataSource = classes;

        if (currentYear?.Semestres is { Count: > 0 } semestres)
        {
            _cboSemestre.DisplayMember = nameof(Semestre.Nom);
            _cboSemestre.ValueMember = nameof(Semestre.Id);
            _cboSemestre.DataSource = semestres;
        }
        else
        {
            _cboSemestre.DataSource = null;
            _cboSemestre.Items.Clear();
        }
    }

    private void LoadData()
    {
        var classeId = (_cboClasse.SelectedItem as Classe)?.Id ?? 0;
        var semId = (_cboSemestre.SelectedItem as Semestre)?.Id ?? 0;

        if (classeId <= 0 || semId <= 0)
        {
            _dgvSeances.Rows.Clear();
            return;
        }

        var seances = _edtDao.GetByClasseAndSemestre(classeId, semId);
        _dgvSeances.Rows.Clear();
        foreach (var s in seances)
        {
            _dgvSeances.Rows.Add(
                s.Id,
                s.Jour,
                s.HeureDebut,
                s.CoursIntitule ?? "Cours inconnu",
                s.EnseignantNom ?? "Enseignant inconnu",
                s.SalleNom ?? "Salle inconnue");
        }
    }

    private void CboClasse_SelectedIndexChanged(object? sender, EventArgs e) => LoadData();

    private void CboSemestre_SelectedIndexChanged(object? sender, EventArgs e) => LoadData();

    private void BtnActualiser_Click(object? sender, EventArgs e) => LoadData();

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        var classeId = (_cboClasse.SelectedItem as Classe)?.Id ?? 0;
        var semId = (_cboSemestre.SelectedItem as Semestre)?.Id ?? 0;
        using var dialog = new FormSeanceDetail(0, classeId, semId);
        dialog.DataSaved += (_, _) => LoadData();
        dialog.ShowDialog(this);
    }

    private void DgvSeances_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvSeances.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvSeances.Columns[e.ColumnIndex].Name == "ColEdit")
        {
            var dlg = new FormSeanceDetail(id);
            dlg.DataSaved += (s, ev) => LoadData();
            dlg.ShowDialog();
        }
        else if (_dgvSeances.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Confirmez-vous la suppression de cette séance de cours ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _edtDao.Delete(id);
                LoadData();
            }
        }
    }
}
