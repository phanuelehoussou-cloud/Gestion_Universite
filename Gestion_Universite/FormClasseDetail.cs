using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormClasseDetail : Form
{
    private readonly ClasseDAO _classeDao = new();
    private readonly FiliereDAO _filiereDao = new();
    private readonly NiveauDAO _niveauDao = new();
    private readonly AnneeAcademiqueDAO _anneeDao = new();

    private ComboBox _cboFiliere = null!;
    private ComboBox _cboNiveau = null!;
    private ComboBox _cboAnnee = null!;
    private ComboBox _cboGroupe = null!;
    private int _classeId = 0;

    public event EventHandler? DataSaved;

    public FormClasseDetail(int classeId = 0)
    {
        _classeId = classeId;
        InitializeCustomComponents();
        LoadLookups();
        if (_classeId > 0)
            LoadClasseData();
    }

    private void InitializeCustomComponents()
    {
        Text = _classeId == 0 ? "Ajouter une classe" : "Modifier une classe";
        Size = new Size(520, 420);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;
        AutoScroll = true;

        var titleBar = UITheme.CreateTitleBar(this, _classeId == 0 ? "5. Classes - Nouvelle Classe" : "5. Classes - Modification", "👥", isDialog: true);

        var card = UITheme.CreateCard("Informations de la classe", "👥");
        card.Dock = DockStyle.Fill;
        card.Padding = new Padding(16);

        var pnlForm = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 5,
            Padding = new Padding(8)
        };
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95));
        pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        pnlForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

        pnlForm.Controls.Add(new Label { Text = "Filière :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
        _cboFiliere = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_cboFiliere, 1, 0);

        pnlForm.Controls.Add(new Label { Text = "Niveau :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
        _cboNiveau = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_cboNiveau, 1, 1);

        pnlForm.Controls.Add(new Label { Text = "Année :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
        _cboAnnee = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        pnlForm.Controls.Add(_cboAnnee, 1, 2);

        pnlForm.Controls.Add(new Label { Text = "Groupe :", Font = UITheme.FontSemiBold(), AutoSize = true, Anchor = AnchorStyles.Left }, 0, 3);
        _cboGroupe = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Dock = DockStyle.Fill };
        _cboGroupe.Items.AddRange(new object[] { "A", "B", "C", "D" });
        _cboGroupe.SelectedIndex = 0;
        pnlForm.Controls.Add(_cboGroupe, 1, 3);

        var pnlBtns = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        var btnSave = UITheme.CreatePrimaryButton("Enregistrer");
        btnSave.Click += BtnSave_Click;
        var btnCancel = UITheme.CreateSecondaryButton("Annuler");
        btnCancel.Click += (s, e) => Close();
        pnlBtns.Controls.Add(btnSave);
        pnlBtns.Controls.Add(btnCancel);
        pnlForm.Controls.Add(pnlBtns, 1, 4);

        card.Controls.Add(pnlForm);
        Controls.Add(card);
        Controls.Add(titleBar);
    }

    private void LoadLookups()
    {
        _cboFiliere.DataSource = _filiereDao.GetAll();
        _cboNiveau.DataSource = _niveauDao.GetAll();
        _cboAnnee.DataSource = _anneeDao.GetAll();
    }

    private void LoadClasseData()
    {
        var c = _classeDao.GetById(_classeId);
        if (c != null)
        {
            var filieres = (List<Filiere>)_cboFiliere.DataSource;
            var f = filieres.FirstOrDefault(x => x.Id == c.FiliereId);
            if (f != null) _cboFiliere.SelectedItem = f;

            var niveaux = (List<Niveau>)_cboNiveau.DataSource;
            var n = niveaux.FirstOrDefault(x => x.Id == c.NiveauId);
            if (n != null) _cboNiveau.SelectedItem = n;

            var annees = (List<AnneeAcademique>)_cboAnnee.DataSource;
            var a = annees.FirstOrDefault(x => x.Id == c.AnneeAcademiqueId);
            if (a != null) _cboAnnee.SelectedItem = a;

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
