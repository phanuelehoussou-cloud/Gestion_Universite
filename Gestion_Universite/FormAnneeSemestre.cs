using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormAnneeSemestre : Form
{
    private readonly AnneeAcademiqueDAO _anneeDao = new();

    private int _editingAnneeId = 0;

    public FormAnneeSemestre()
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
        var annees = _anneeDao.GetAll();
        _dgvAnnees.Rows.Clear();
        _cboAnneePourSemestre.Items.Clear();

        foreach (var a in annees)
        {
            _dgvAnnees.Rows.Add(a.Id, a.Code, a.DateDebut.ToString("dd/MM/yyyy"), a.DateFin.ToString("dd/MM/yyyy"), a.Statut);
            _cboAnneePourSemestre.Items.Add(a);
        }

        if (_cboAnneePourSemestre.Items.Count > 0)
            _cboAnneePourSemestre.SelectedIndex = 0;
    }

    private void BtnSaveAnnee_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtAnnee.Text))
        {
            MessageBox.Show("Veuillez saisir le libellé de l'année académique.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_editingAnneeId == 0)
        {
            var annee = new AnneeAcademique
            {
                Code = _txtAnnee.Text.Trim(),
                DateDebut = _dtpDebut.Value,
                DateFin = _dtpFin.Value,
                Statut = "Ouverte"
            };
            _anneeDao.Add(annee);
            MessageBox.Show("Année académique créée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            var annee = _anneeDao.GetById(_editingAnneeId);
            if (annee != null)
            {
                annee.Code = _txtAnnee.Text.Trim();
                annee.DateDebut = _dtpDebut.Value;
                annee.DateFin = _dtpFin.Value;
                _anneeDao.Update(annee);
                MessageBox.Show("Année académique mise à jour.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        ResetAnneeForm();
        LoadData();
    }

    private void BtnAddSem_Click(object? sender, EventArgs e)
    {
        if (_cboAnneePourSemestre.SelectedItem is not AnneeAcademique selectedAnnee)
        {
            MessageBox.Show("Veuillez sélectionner une année académique.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var sem = new Semestre
        {
            Nom = _cboSemestre.SelectedItem?.ToString() ?? "Semestre 1",
            DateDebut = _dtpSemDebut.Value,
            DateFin = _dtpSemDebut.Value.AddMonths(5)
        };

        _anneeDao.AddSemestre(selectedAnnee.Id, sem);
        MessageBox.Show($"Semestre '{sem.Nom}' rattaché à l'année {selectedAnnee.Code}.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void DgvAnnees_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvAnnees.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvAnnees.Columns[e.ColumnIndex].Name == "ColEdit")
        {
            var annee = _anneeDao.GetById(id);
            if (annee != null)
            {
                _editingAnneeId = annee.Id;
                _txtAnnee.Text = annee.Code;
                _dtpDebut.Value = annee.DateDebut;
                _dtpFin.Value = annee.DateFin;
            }
        }
        else if (_dgvAnnees.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Voulez-vous vraiment supprimer cette année académique ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _anneeDao.Delete(id);
                LoadData();
            }
        }
        else if (_dgvAnnees.Columns[e.ColumnIndex].Name == "ColView")
        {
            var annee = _anneeDao.GetById(id);
            if (annee != null)
            {
                var semestres = string.Join("\n - ", annee.Semestres.Select(s => $"{s.Nom} ({s.DateDebut:dd/MM/yyyy} - {s.DateFin:dd/MM/yyyy})"));
                MessageBox.Show($"Année: {annee.Code}\nStatut: {annee.Statut}\nDébut: {annee.DateDebut:dd/MM/yyyy}\nFin: {annee.DateFin:dd/MM/yyyy}\n\nSemestres:\n - {(string.IsNullOrEmpty(semestres) ? "Aucun semestre rattaché" : semestres)}", "Détails de l'année", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    private void ResetAnneeForm()
    {
        _editingAnneeId = 0;
        _txtAnnee.Text = "";
        _dtpDebut.Value = DateTime.Now;
        _dtpFin.Value = DateTime.Now.AddYears(1);
    }
}
