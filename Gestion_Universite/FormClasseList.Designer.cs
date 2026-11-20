#nullable enable
namespace Gestion_Universite;

partial class FormClasseList
{
    private System.ComponentModel.IContainer? components = null;
    private Panel pnlHeader = null!;
    private Label lblTitle = null!;
    private Label lblSubtitle = null!;
    private Panel pnlContent = null!;
    private FlowLayoutPanel pnlActions = null!;
    private Button btnAdd = null!;
    private Button btnRefresh = null!;
    private DataGridView _dgvClasses = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        pnlHeader = new Panel();
        lblTitle = new Label();
        lblSubtitle = new Label();
        pnlContent = new Panel();
        pnlActions = new FlowLayoutPanel();
        btnAdd = new Button();
        btnRefresh = new Button();
        _dgvClasses = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)_dgvClasses).BeginInit();
        SuspendLayout();

        // Header
        pnlHeader.BackColor = Color.FromArgb(139, 0, 0);
        pnlHeader.Controls.Add(lblSubtitle);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Height = 88;
        pnlHeader.Padding = new Padding(24, 14, 24, 10);
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblTitle.ForeColor = Color.White;
        lblTitle.Location = new Point(24, 10);
        lblTitle.Text = "Gestion des classes";
        lblSubtitle.AutoSize = true;
        lblSubtitle.Font = new Font("Segoe UI", 10F);
        lblSubtitle.ForeColor = Color.FromArgb(245, 225, 225);
        lblSubtitle.Location = new Point(27, 51);
        lblSubtitle.Text = "Consulter, ajouter, modifier ou supprimer une classe";

        // Main content
        pnlContent.BackColor = Color.FromArgb(246, 247, 249);
        pnlContent.Controls.Add(_dgvClasses);
        pnlContent.Controls.Add(pnlActions);
        pnlContent.Dock = DockStyle.Fill;
        pnlContent.Padding = new Padding(24);
        pnlActions.Dock = DockStyle.Top;
        pnlActions.Height = 52;
        pnlActions.FlowDirection = FlowDirection.LeftToRight;
        pnlActions.Padding = new Padding(0, 0, 0, 10);
        pnlActions.WrapContents = false;
        pnlActions.Controls.Add(btnAdd);
        pnlActions.Controls.Add(btnRefresh);
        btnAdd.BackColor = Color.FromArgb(139, 0, 0);
        btnAdd.FlatAppearance.BorderSize = 0;
        btnAdd.FlatStyle = FlatStyle.Flat;
        btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnAdd.ForeColor = Color.White;
        btnAdd.Size = new Size(170, 38);
        btnAdd.Text = "Nouvelle classe";
        btnAdd.UseVisualStyleBackColor = false;
        btnAdd.Click += BtnAdd_Click;
        btnRefresh.BackColor = Color.White;
        btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(210, 214, 220);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Segoe UI", 10F);
        btnRefresh.ForeColor = Color.FromArgb(45, 50, 58);
        btnRefresh.Margin = new Padding(10, 0, 0, 0);
        btnRefresh.Size = new Size(130, 38);
        btnRefresh.Text = "Actualiser";
        btnRefresh.Click += BtnRefresh_Click;

        // Grid
        _dgvClasses.AllowUserToAddRows = false;
        _dgvClasses.AllowUserToDeleteRows = false;
        _dgvClasses.AllowUserToResizeRows = false;
        _dgvClasses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _dgvClasses.BackgroundColor = Color.White;
        _dgvClasses.BorderStyle = BorderStyle.None;
        _dgvClasses.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        _dgvClasses.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        _dgvClasses.ColumnHeadersHeight = 44;
        _dgvClasses.Columns.AddRange(
            new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "ID", Visible = false },
            new DataGridViewTextBoxColumn { Name = "ColClasse", HeaderText = "Classe", MinimumWidth = 120, FillWeight = 35 },
            new DataGridViewTextBoxColumn { Name = "ColDetails", HeaderText = "Détails", MinimumWidth = 180, FillWeight = 45 },
            new DataGridViewButtonColumn { Name = "ColEdit", HeaderText = "Modifier", Text = "Modifier", UseColumnTextForButtonValue = true, MinimumWidth = 100, FillWeight = 10 },
            new DataGridViewButtonColumn { Name = "ColDelete", HeaderText = "Supprimer", Text = "Supprimer", UseColumnTextForButtonValue = true, MinimumWidth = 105, FillWeight = 10 });
        _dgvClasses.Dock = DockStyle.Fill;
        _dgvClasses.EnableHeadersVisualStyles = false;
        _dgvClasses.Font = new Font("Segoe UI", 10F);
        _dgvClasses.GridColor = Color.FromArgb(232, 235, 239);
        _dgvClasses.RowHeadersVisible = false;
        _dgvClasses.RowTemplate.Height = 38;
        _dgvClasses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _dgvClasses.MultiSelect = false;
        _dgvClasses.CellContentClick += DgvClasses_CellContentClick;
        _dgvClasses.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 251);
        _dgvClasses.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        _dgvClasses.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(45, 50, 58);
        _dgvClasses.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
        _dgvClasses.DefaultCellStyle.SelectionBackColor = Color.FromArgb(252, 232, 232);
        _dgvClasses.DefaultCellStyle.SelectionForeColor = Color.FromArgb(45, 50, 58);
        _dgvClasses.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 252);

        // Form
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(246, 247, 249);
        ClientSize = new Size(1100, 680);
        Controls.Add(pnlContent);
        Controls.Add(pnlHeader);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(900, 560);
        Name = "FormClasseList";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Classes - Gestion universitaire";
        ((System.ComponentModel.ISupportInitialize)_dgvClasses).EndInit();
        ResumeLayout(false);
    }
}

