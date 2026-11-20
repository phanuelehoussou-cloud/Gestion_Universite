using DAO.Models;
using DAO.Repositories;
#nullable enable
namespace Gestion_Universite;

partial class FormPiecesEtudiant
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private DataGridView _dgvPieces = null!;
    private ComboBox _cboTypePiece = null!;

    private Panel titleBar = null!;
    private Panel card = null!;
    private FlowLayoutPanel topPanel = null!;
    private Button btnAdd = null!;
    private DataGridViewButtonColumn colValider = null!;
    private FlowLayoutPanel bottomPanel = null!;
    private Button btnClose = null!;

    private void InitializeComponent()
    {
        Text = "Pièces administratives du dossier";
        Size = new Size(650, 430);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        titleBar = UITheme.CreateTitleBar(this, "Pièces administratives - Dossier Étudiant", "IIT", isDialog: true);
        Controls.Add(titleBar);

        card = UITheme.CreateCard("Pièces administratives fournies", "");
        card.Dock = DockStyle.Fill;
        card.Padding = new Padding(12);

        topPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 45,
            FlowDirection = FlowDirection.LeftToRight
        };

        _cboTypePiece = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Width = 220 };
        _cboTypePiece.Items.AddRange(new object[] { "Extrait de naissance", "Diplôme du Baccalauréat", "Photos d'identité", "Certificat de nationalité", "Relevé de notes" });
        _cboTypePiece.SelectedIndex = 0;

        btnAdd = UITheme.CreatePrimaryButton("Ajouter la pièce");
        btnAdd.Click += BtnAdd_Click;

        topPanel.Controls.Add(_cboTypePiece);
        topPanel.Controls.Add(btnAdd);

        _dgvPieces = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvPieces);

        _dgvPieces.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvPieces.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColType", HeaderText = "Type de Pièce", FillWeight = 160 });
        _dgvPieces.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColStatut", HeaderText = "Statut", FillWeight = 90 });

        colValider = new DataGridViewButtonColumn { Name = "ColValider", HeaderText = "Action", Text = "Valider / Réfuter", UseColumnTextForButtonValue = true, FillWeight = 90 };
        _dgvPieces.Columns.Add(colValider);
        _dgvPieces.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvPieces.CellContentClick += DgvPieces_CellContentClick;

        bottomPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 45,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(4)
        };
        btnClose = UITheme.CreateSecondaryButton("Fermer");
        btnClose.Click += (s, e) => Close();
        bottomPanel.Controls.Add(btnClose);

        card.Controls.Add(_dgvPieces);
        card.Controls.Add(topPanel);
        card.Controls.Add(bottomPanel);

        Controls.Add(card);
        Controls.Add(titleBar);
    }
}
