using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormClasseList : Form
{
    private readonly ClasseDAO _classeDao = new();
    private DataGridView _dgvClasses = null!;

    public FormClasseList()
    {
        InitializeCustomComponents();
        LoadData();
    }

    private void InitializeCustomComponents()
    {
        Text = "Classes";
        Size = new Size(1000, 560);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        AutoScroll = true;

        var titleBar = UITheme.CreateTitleBar(this, "5. Classes - Liste & Gestion", "👥");

        var contentPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            BackColor = Color.Transparent,
            AutoScroll = true
        };

        var cardList = UITheme.CreateCard("Liste des classes", "📋");
        cardList.Dock = DockStyle.Fill;

        // Top actions bar
        var pnlActions = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 45,
            FlowDirection = FlowDirection.LeftToRight
        };

        var btnAdd = UITheme.CreatePrimaryButton("Nouvelle classe", "➕");
        btnAdd.Click += (s, e) =>
        {
            var dlg = new FormClasseDetail();
            dlg.DataSaved += (sender, args) => LoadData();
            dlg.ShowDialog();
        };

        var btnRefresh = UITheme.CreateSecondaryButton("Actualiser", "🔄");
        btnRefresh.Click += (s, e) => LoadData();

        pnlActions.Controls.Add(btnAdd);
        pnlActions.Controls.Add(btnRefresh);

        _dgvClasses = new DataGridView { Dock = DockStyle.Fill };
        UITheme.StyleDataGridView(_dgvClasses);

        _dgvClasses.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false });
        _dgvClasses.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColClasse", HeaderText = "Classe", FillWeight = 110, MinimumWidth = 100 });
        _dgvClasses.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColDetails", HeaderText = "Détails", FillWeight = 180, MinimumWidth = 160 });

        var colEdit = new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Action", Text = "✏", UseColumnTextForButtonValue = true, FillWeight = 40, MinimumWidth = 45 };
        var colDelete = new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "", Text = "🗑", UseColumnTextForButtonValue = true, FillWeight = 40, MinimumWidth = 45 };
        _dgvClasses.Columns.AddRange(colEdit, colDelete);
        _dgvClasses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _dgvClasses.CellContentClick += DgvClasses_CellContentClick;

        cardList.Controls.Add(_dgvClasses);
        cardList.Controls.Add(pnlActions);
        contentPanel.Controls.Add(cardList);
        Controls.Add(contentPanel);
        Controls.Add(titleBar);
    }

    private void LoadData()
    {
        var classes = _classeDao.GetAll();
        _dgvClasses.Rows.Clear();
        foreach (var c in classes)
        {
            _dgvClasses.Rows.Add(c.Id, c.Code, c.Details);
        }
    }

    private void DgvClasses_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var id = Convert.ToInt32(_dgvClasses.Rows[e.RowIndex].Cells["ColId"].Value);

        if (_dgvClasses.Columns[e.ColumnIndex].Name == "ColEdit")
        {
            var dlg = new FormClasseDetail(id);
            dlg.DataSaved += (s, ev) => LoadData();
            dlg.ShowDialog();
        }
        else if (_dgvClasses.Columns[e.ColumnIndex].Name == "ColDelete")
        {
            if (MessageBox.Show("Confirmez-vous la suppression de cette classe ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _classeDao.Delete(id);
                LoadData();
            }
        }
    }
}
