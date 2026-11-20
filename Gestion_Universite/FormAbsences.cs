using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormAbsences : Form
{
    private readonly AbsenceDAO _absenceDao = new();
    private readonly ClasseDAO _classeDao = new();
    private readonly EtudiantDAO _etudiantDao = new();
    private readonly EmploiDuTempsDAO _seanceDao = new();

    private int _editingId;
    private bool _loadingChoices;

    public FormAbsences()
    {
        InitializeComponent();
        Load += FormAbsences_Load;
    }

    private void FormAbsences_Load(object? sender, EventArgs e)
    {
        if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
        LoadChoices();
        LoadData();
    }

    private void CboClasse_SelectedIndexChanged(object? sender, EventArgs e) => RefreshFilteredChoices();

    private void CboSeance_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cboSeance.SelectedItem is SeanceEmploiDuTemps selected && selected.DateSeance.Date <= DateTime.Today)
            _dtpDate.Value = selected.DateSeance.Date;
    }

    private void BtnReset_Click(object? sender, EventArgs e) => ResetForm();

    private void LoadChoices()
    {
        _loadingChoices = true;
        try
        {
            _cboClasse.DataSource = _classeDao.GetAll();
            _cboClasse.DisplayMember = nameof(Classe.Code);
            _cboClasse.ValueMember = nameof(Classe.Id);
            RefreshFilteredChoices();
        }
        finally
        {
            _loadingChoices = false;
        }
        RefreshFilteredChoices();
    }

    private void RefreshFilteredChoices()
    {
        if (_loadingChoices)
            return;

        var selectedClass = _cboClasse.SelectedItem as Classe;
        var students = _etudiantDao.GetAll();
        if (selectedClass != null)
            students = students.Where(e => string.Equals(e.ClasseActuelle, selectedClass.Code, StringComparison.OrdinalIgnoreCase)).ToList();
        else
            students.Clear();

        _cboEtudiant.DataSource = students;
        _cboEtudiant.DisplayMember = nameof(Etudiant.NomComplet);
        _cboEtudiant.ValueMember = nameof(Etudiant.Id);

        var sessions = _seanceDao.GetAll().Where(s => s.DateSeance.Date <= DateTime.Today).ToList();
        if (selectedClass != null)
            sessions = sessions.Where(s => s.ClasseId == selectedClass.Id).ToList();
        _cboSeance.DataSource = sessions;
        _cboSeance.DisplayMember = nameof(SeanceEmploiDuTemps.Libelle);
        _cboSeance.ValueMember = nameof(SeanceEmploiDuTemps.Id);
    }

    private void LoadData()
    {
        _grid.Rows.Clear();
        foreach (var absence in _absenceDao.GetAll())
        {
            _grid.Rows.Add(absence.Id, absence.DateAbsence.ToString("dd/MM/yyyy"), absence.EtudiantNom ?? string.Empty,
                absence.ClasseCode ?? string.Empty, absence.CoursIntitule ?? string.Empty, absence.Statut ?? string.Empty, absence.Motif ?? string.Empty);
        }
    }

    private void SaveAbsence(object? sender, EventArgs e)
    {
        if (_cboEtudiant.SelectedItem is not Etudiant etudiant || _cboSeance.SelectedItem is not SeanceEmploiDuTemps seance)
        {
            MessageBox.Show("Ajoutez d'abord des étudiants et des séances à l'emploi du temps.", "Données manquantes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var absence = new Absence
        {
            Id = _editingId,
            EtudiantId = etudiant.Id,
            SeanceId = seance.Id,
            DateAbsence = seance.DateSeance.Date,
            Justifiee = _chkJustifiee.Checked,
            Motif = _txtMotif.Text.Trim()
        };

        if (!_absenceDao.Save(absence, out var error))
        {
            MessageBox.Show(error, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ResetForm();
        LoadData();
    }

    private void Grid_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0)
            return;

        var id = Convert.ToInt32(_grid.Rows[e.RowIndex].Cells["Id"].Value);
        var action = _grid.Columns[e.ColumnIndex].Name;
        var absence = _absenceDao.GetById(id);
        if (absence == null)
            return;

        if (action == "Edit")
        {
            _editingId = absence.Id;
            var selectedSession = _seanceDao.GetAll().FirstOrDefault(s => s.Id == absence.SeanceId);
            if (selectedSession != null)
                _cboClasse.SelectedValue = selectedSession.ClasseId;
            _cboEtudiant.SelectedValue = absence.EtudiantId;
            _cboSeance.SelectedValue = absence.SeanceId;
            _dtpDate.Value = absence.DateAbsence.Date;
            _chkJustifiee.Checked = absence.Justifiee;
            _txtMotif.Text = absence.Motif;
        }
        else if (action == "Delete" && MessageBox.Show("Supprimer cet enregistrement d'absence ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            _absenceDao.Delete(id);
            LoadData();
        }
    }

    private void ResetForm()
    {
        _editingId = 0;
        if (_cboClasse.Items.Count > 0) _cboClasse.SelectedIndex = 0;
        if (_cboEtudiant.Items.Count > 0) _cboEtudiant.SelectedIndex = 0;
        if (_cboSeance.Items.Count > 0) _cboSeance.SelectedIndex = 0;
        _dtpDate.Value = DateTime.Today;
        _chkJustifiee.Checked = false;
        _txtMotif.Clear();
    }
}
