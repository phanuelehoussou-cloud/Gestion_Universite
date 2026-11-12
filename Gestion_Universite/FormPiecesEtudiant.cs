using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormPiecesEtudiant : Form
{
    private readonly EtudiantDAO _etudiantDao = new();
    private readonly int _etudiantId;
    private DataGridView _dgvPieces = null!;
    private ComboBox _cboTypePiece = null!;

    public FormPiecesEtudiant(int etudiantId)
    {
        _etudiantId = etudiantId;
        InitializeCustomComponents();
        LoadPieces();
    }

    private void InitializeCustomComponents()
    {
        Text = "Pièces administratives du dossier";
        Size = new Size(650, 430);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        var titleBar = UITheme.CreateTitleBar(this, "Pièces administratives - Dossier Étudiant", "📄", isDialog: true);
        Controls.Add(titleBar);

        var card = UITheme.CreateCard("Pièces administratives fournies", "📁");
        card.Dock = DockStyle.Fill;
        card.Padding = new Padding(12);

        var topPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 45,
            FlowDirection = FlowDirection.LeftToRight
        };

        _cboTypePiece = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.FontRegular(10), Width = 220 };
        _cboTypePiece.Items.AddRange(new object[] { "Extrait de naissance", "Diplôme du Baccalauréat", "Photos d'identité", "Certificat de nationalité", "Relevé de notes" });
        _cboTypePiece.SelectedIndex = 0;

        var btnAdd = UITheme.CreatePrimaryButton("Ajouter la pièce");
        btnAdd.Click += BtnAdd_Click;

        topPanel.Controls.Add(_cboTypePiece);
        topPanel.Controls.Add(btnAdd);

        _dgvPieces = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvPieces);

        _dgvPieces.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvPieces.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColType", HeaderText = "Type de Pièce", FillWeight = 160 });
        _dgvPieces.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColStatut", HeaderText = "Statut", FillWeight = 90 });

        var colValider = new DataGridViewButtonColumn { Name = "ColValider", HeaderText = "Action", Text = "Valider / Réfuter", UseColumnTextForButtonValue = true, FillWeight = 90 };
        _dgvPieces.Columns.Add(colValider);
        _dgvPieces.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvPieces.CellContentClick += DgvPieces_CellContentClick;

        card.Controls.Add(_dgvPieces);
        card.Controls.Add(topPanel);

        var bottomPanel = new Panel { Dock = DockStyle.Bottom, Height = 45 };
        var btnClose = UITheme.CreateSecondaryButton("Fermer");
        btnClose.Location = new Point(card.Width - 110, 5);
        btnClose.Click += (s, e) => Close();
        bottomPanel.Controls.Add(btnClose);
        card.Controls.Add(bottomPanel);

        Controls.Add(card);
    }

    private void LoadPieces()
    {
        var etu = _etudiantDao.GetById(_etudiantId);
        _dgvPieces.Rows.Clear();
        if (etu?.Pieces != null)
        {
            foreach (var p in etu.Pieces)
            {
                _dgvPieces.Rows.Add(p.Id, p.TypePiece, p.EstValidee ? "Active" : "En attente");
            }
        }
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        var type = _cboTypePiece.SelectedItem?.ToString() ?? "Extrait de naissance";
        var piece = new PieceAdministrative
        {
            TypePiece = type,
            EstValidee = false,
            DateDepot = DateTime.Now
        };

        _etudiantDao.AddOrUpdatePiece(_etudiantId, piece);
        LoadPieces();
    }

    private void DgvPieces_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvPieces.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvPieces.Columns[e.ColumnIndex].Name == "ColValider")
        {
            var etu = _etudiantDao.GetById(_etudiantId);
            var piece = etu?.Pieces.FirstOrDefault(p => p.Id == id);
            if (piece != null)
            {
                piece.EstValidee = !piece.EstValidee;
                _etudiantDao.AddOrUpdatePiece(_etudiantId, piece);
                LoadPieces();
            }
        }
    }
}
