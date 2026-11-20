#nullable enable
namespace Gestion_Universite;

partial class FormFiliere
{
    private System.ComponentModel.IContainer? components = null;
    private Panel pnlHeader = null!;
    private Label lblTitle = null!;
    private Label lblSubtitle = null!;
    private TableLayoutPanel mainLayout = null!;
    private GroupBox grpFiliere = null!;
    private Label lblNom = null!;
    private Label lblDescription = null!;
    private Label lblStatut = null!;
    private FlowLayoutPanel pnlActions = null!;
    private Button btnSave = null!;
    private Button btnReset = null!;
    private GroupBox grpListe = null!;
    private DataGridViewTextBoxColumn colId = null!;
    private DataGridViewTextBoxColumn colNom = null!;
    private DataGridViewTextBoxColumn colStatut = null!;
    private DataGridViewButtonColumn colEdit = null!;
    private DataGridViewButtonColumn colDelete = null!;
    private TextBox _txtNom = null!;
    private TextBox _txtDescription = null!;
    private ComboBox _cboStatut = null!;
    private DataGridView _dgvFilieres = null!;

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
        mainLayout = new TableLayoutPanel();
        grpFiliere = new GroupBox();
        lblNom = new Label();
        lblDescription = new Label();
        lblStatut = new Label();
        _txtNom = new TextBox();
        _txtDescription = new TextBox();
        _cboStatut = new ComboBox();
        pnlActions = new FlowLayoutPanel();
        btnSave = new Button();
        btnReset = new Button();
        grpListe = new GroupBox();
        _dgvFilieres = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colNom = new DataGridViewTextBoxColumn();
        colStatut = new DataGridViewTextBoxColumn();
        colEdit = new DataGridViewButtonColumn();
        colDelete = new DataGridViewButtonColumn();

        ((System.ComponentModel.ISupportInitialize)_dgvFilieres).BeginInit();
        SuspendLayout();

        pnlHeader.BackColor = Color.FromArgb(139, 0, 0);
        pnlHeader.Controls.Add(lblSubtitle);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Height = 92;
        pnlHeader.Padding = new Padding(24, 14, 24, 10);
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblTitle.ForeColor = Color.White;
        lblTitle.Location = new Point(24, 10);
        lblTitle.Text = "Gestion des filières";
        lblSubtitle.AutoSize = true;
        lblSubtitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        lblSubtitle.ForeColor = Color.FromArgb(245, 225, 225);
        lblSubtitle.Location = new Point(28, 53);
        lblSubtitle.Text = "Créer et administrer les filières de formation";

        mainLayout.ColumnCount = 2;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
        mainLayout.Controls.Add(grpFiliere, 0, 0);
        mainLayout.Controls.Add(grpListe, 1, 0);
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Padding = new Padding(20);
        mainLayout.RowCount = 1;
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        grpFiliere.BackColor = Color.White;
        grpFiliere.Controls.Add(lblNom);
        grpFiliere.Controls.Add(_txtNom);
        grpFiliere.Controls.Add(lblDescription);
        grpFiliere.Controls.Add(_txtDescription);
        grpFiliere.Controls.Add(lblStatut);
        grpFiliere.Controls.Add(_cboStatut);
        grpFiliere.Controls.Add(pnlActions);
        grpFiliere.Dock = DockStyle.Fill;
        grpFiliere.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        grpFiliere.ForeColor = Color.FromArgb(139, 0, 0);
        grpFiliere.Padding = new Padding(18);
        grpFiliere.Text = "Fiche filière";

        lblNom.AutoSize = true;
        lblNom.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        lblNom.ForeColor = Color.FromArgb(45, 50, 58);
        lblNom.Location = new Point(20, 52);
        lblNom.Text = "Nom de la filière";
        _txtNom.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _txtNom.Font = new Font("Segoe UI", 10F);
        _txtNom.Location = new Point(20, 78);
        _txtNom.Size = new Size(330, 30);
        _txtNom.Name = "_txtNom";

        lblDescription.AutoSize = true;
        lblDescription.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        lblDescription.ForeColor = Color.FromArgb(45, 50, 58);
        lblDescription.Location = new Point(20, 127);
        lblDescription.Text = "Description";
        _txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _txtDescription.Font = new Font("Segoe UI", 10F);
        _txtDescription.Location = new Point(20, 153);
        _txtDescription.Multiline = true;
        _txtDescription.ScrollBars = ScrollBars.Vertical;
        _txtDescription.Size = new Size(330, 110);
        _txtDescription.Name = "_txtDescription";

        lblStatut.AutoSize = true;
        lblStatut.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        lblStatut.ForeColor = Color.FromArgb(45, 50, 58);
        lblStatut.Location = new Point(20, 280);
        lblStatut.Text = "Statut";
        _cboStatut.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _cboStatut.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboStatut.Font = new Font("Segoe UI", 10F);
        _cboStatut.Items.AddRange(new object[] { "Active", "Inactive" });
        _cboStatut.Location = new Point(20, 306);
        _cboStatut.Size = new Size(330, 28);
        _cboStatut.Name = "_cboStatut";

        pnlActions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlActions.FlowDirection = FlowDirection.LeftToRight;
        pnlActions.Location = new Point(20, 358);
        pnlActions.Size = new Size(330, 46);
        pnlActions.WrapContents = false;
        btnSave.BackColor = Color.FromArgb(139, 0, 0);
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Size = new Size(150, 38);
        btnSave.Text = "Enregistrer";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += BtnSave_Click;
        btnReset.BackColor = Color.White;
        btnReset.FlatAppearance.BorderColor = Color.FromArgb(210, 214, 220);
        btnReset.FlatStyle = FlatStyle.Flat;
        btnReset.Font = new Font("Segoe UI", 10F);
        btnReset.ForeColor = Color.FromArgb(45, 50, 58);
        btnReset.Margin = new Padding(10, 0, 0, 0);
        btnReset.Size = new Size(150, 38);
        btnReset.Text = "Réinitialiser";
        btnReset.Click += BtnReset_Click;
        pnlActions.Controls.Add(btnSave);
        pnlActions.Controls.Add(btnReset);

        grpListe.BackColor = Color.White;
        grpListe.Controls.Add(_dgvFilieres);
        grpListe.Dock = DockStyle.Fill;
        grpListe.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        grpListe.ForeColor = Color.FromArgb(139, 0, 0);
        grpListe.Padding = new Padding(12, 22, 12, 12);
        grpListe.Text = "Répertoire des filières";

        _dgvFilieres.AllowUserToAddRows = false;
        _dgvFilieres.AllowUserToDeleteRows = false;
        _dgvFilieres.AllowUserToResizeRows = false;
        _dgvFilieres.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _dgvFilieres.BackgroundColor = Color.White;
        _dgvFilieres.BorderStyle = BorderStyle.None;
        _dgvFilieres.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        _dgvFilieres.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        _dgvFilieres.ColumnHeadersHeight = 42;
        colId.HeaderText = "ID";
        colId.Name = "ColId";
        colId.Visible = false;
        colNom.HeaderText = "Filière";
        colNom.MinimumWidth = 120;
        colNom.Name = "ColNom";
        colNom.FillWeight = 150F;
        colStatut.HeaderText = "Statut";
        colStatut.MinimumWidth = 80;
        colStatut.Name = "ColStatut";
        colEdit.HeaderText = "Action";
        colEdit.MinimumWidth = 90;
        colEdit.Name = "ColEdit";
        colEdit.Text = "Modifier";
        colEdit.UseColumnTextForButtonValue = true;
        colDelete.HeaderText = "Action";
        colDelete.MinimumWidth = 90;
        colDelete.Name = "ColDelete";
        colDelete.Text = "Supprimer";
        colDelete.UseColumnTextForButtonValue = true;
        _dgvFilieres.Columns.AddRange(new DataGridViewColumn[] { colId, colNom, colStatut, colEdit, colDelete });
        _dgvFilieres.Dock = DockStyle.Fill;
        _dgvFilieres.EnableHeadersVisualStyles = false;
        _dgvFilieres.Font = new Font("Segoe UI", 10F);
        _dgvFilieres.GridColor = Color.FromArgb(232, 235, 239);
        _dgvFilieres.Location = new Point(15, 42);
        _dgvFilieres.MultiSelect = false;
        _dgvFilieres.Name = "_dgvFilieres";
        _dgvFilieres.RowHeadersVisible = false;
        _dgvFilieres.RowTemplate.Height = 38;
        _dgvFilieres.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _dgvFilieres.CellContentClick += DgvFilieres_CellContentClick;
        _dgvFilieres.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 251);
        _dgvFilieres.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        _dgvFilieres.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(45, 50, 58);
        _dgvFilieres.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
        _dgvFilieres.DefaultCellStyle.SelectionBackColor = Color.FromArgb(252, 232, 232);
        _dgvFilieres.DefaultCellStyle.SelectionForeColor = Color.FromArgb(45, 50, 58);
        _dgvFilieres.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 252);

        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(246, 247, 249);
        ClientSize = new Size(1120, 680);
        Controls.Add(mainLayout);
        Controls.Add(pnlHeader);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(940, 620);
        Name = "FormFiliere";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Filières - Gestion universitaire";
        ((System.ComponentModel.ISupportInitialize)_dgvFilieres).EndInit();
        ResumeLayout(false);
    }
}
