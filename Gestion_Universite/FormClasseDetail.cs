using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormClasseDetail : Form
{
    private readonly ClasseDAO _classeDao = new();
    private readonly FiliereDAO _filiereDao = new();
    private readonly NiveauDAO _niveauDao = new();
    private readonly AnneeAcademiqueDAO _anneeDao = new();

    private int _classeId = 0;

    public event EventHandler? DataSaved;

    public FormClasseDetail() : this(0) { }

    public FormClasseDetail(int classeId)
    {
        _classeId = classeId;
        InitializeComponent();
        Text = _classeId == 0 ? "Nouvelle classe" : "Modifier la classe";
        lblTitle.Text = Text;
        Load += (_, _) =>
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            LoadLookups();
            if (_classeId > 0) LoadClasseData();
        };
    }

    private void BtnCancel_Click(object? sender, EventArgs e) => Close();

    private void LoadLookups()
    {
        _cboFiliere.DisplayMember = nameof(Filiere.Nom);
        _cboFiliere.ValueMember = nameof(Filiere.Id);
        _cboFiliere.DataSource = _filiereDao.GetAll();
        _cboNiveau.DisplayMember = nameof(Niveau.Code);
        _cboNiveau.ValueMember = nameof(Niveau.Id);
        _cboNiveau.DataSource = _niveauDao.GetAll();
        _cboAnnee.DisplayMember = nameof(AnneeAcademique.Code);
        _cboAnnee.ValueMember = nameof(AnneeAcademique.Id);
        _cboAnnee.DataSource = _anneeDao.GetAll();
    }

    private void LoadClasseData()
    {
        var c = _classeDao.GetById(_classeId);
        if (c != null)
        {
            if (_cboFiliere.DataSource is List<Filiere> filieres)
            {
                var f = filieres.FirstOrDefault(x => x.Id == c.FiliereId);
                if (f != null) _cboFiliere.SelectedItem = f;
            }

            if (_cboNiveau.DataSource is List<Niveau> niveaux)
            {
                var n = niveaux.FirstOrDefault(x => x.Id == c.NiveauId);
                if (n != null) _cboNiveau.SelectedItem = n;
            }

            if (_cboAnnee.DataSource is List<AnneeAcademique> annees)
            {
                var a = annees.FirstOrDefault(x => x.Id == c.AnneeAcademiqueId);
                if (a != null) _cboAnnee.SelectedItem = a;
            }

            _cboGroupe.SelectedItem = c.Groupe;
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (_cboFiliere.SelectedItem is not Filiere filiere ||
            _cboNiveau.SelectedItem is not Niveau niveau ||
            _cboAnnee.SelectedItem is not AnneeAcademique annee)
        {
            MessageBox.Show("Veuillez sélectionner la filière, le niveau et l'année académique.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var groupe = _cboGroupe.SelectedItem?.ToString() ?? "A";
        var filierePrefix = filiere.Nom.Split(' ')[0];
        if (filierePrefix.Length > 4)
            filierePrefix = filierePrefix[..4];

        var code = $"{filierePrefix} {niveau.Code} {groupe}";
        var details = $"{filiere.Nom} - {niveau.Code} - {groupe}";

        if (_classeId == 0)
        {
            var c = new Classe
            {
                Code = code,
                FiliereId = filiere.Id,
                NiveauId = niveau.Id,
                AnneeAcademiqueId = annee.Id,
                Groupe = groupe,
                Details = details,
                FiliereNom = filiere.Nom,
                NiveauCode = niveau.Code,
                AnneeCode = annee.Code
            };
            _classeDao.Add(c);
            MessageBox.Show("Classe créée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            var c = _classeDao.GetById(_classeId);
            if (c != null)
            {
                c.Code = code;
                c.FiliereId = filiere.Id;
                c.NiveauId = niveau.Id;
                c.AnneeAcademiqueId = annee.Id;
                c.Groupe = groupe;
                c.Details = details;
                c.FiliereNom = filiere.Nom;
                c.NiveauCode = niveau.Code;
                c.AnneeCode = annee.Code;
                _classeDao.Update(c);
                MessageBox.Show("Classe modifiée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        DataSaved?.Invoke(this, EventArgs.Empty);
        Close();
    }
}
