using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormPiecesEtudiant : Form
{
    private readonly EtudiantDAO _etudiantDao = new();
    private readonly int _etudiantId;


    public FormPiecesEtudiant() : this(0) { }

    public FormPiecesEtudiant(int etudiantId)
    {
        _etudiantId = etudiantId;
        InitializeComponent();
        Load += (_, _) =>
        {
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
                LoadPieces();
        };
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
