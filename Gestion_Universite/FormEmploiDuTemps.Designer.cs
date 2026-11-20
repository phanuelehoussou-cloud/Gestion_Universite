#nullable enable
namespace Gestion_Universite;

partial class FormEmploiDuTemps
{
    private System.ComponentModel.IContainer? components = null;
    private Panel pnlHeader = null!;
    private Label lblTitle = null!;
    private Label lblSubtitle = null!;
    private GroupBox grpPlanning = null!;
    private Label lblClasse = null!;
    private Label lblSemestre = null!;
    private FlowLayoutPanel pnlActions = null!;
    private Button btnActualiser = null!;
    private Button btnAdd = null!;
    private DataGridViewTextBoxColumn colId = null!;
    private DataGridViewTextBoxColumn colJour = null!;
    private DataGridViewTextBoxColumn colHeure = null!;
    private DataGridViewTextBoxColumn colCours = null!;
    private DataGridViewTextBoxColumn colEnseignant = null!;
    private DataGridViewTextBoxColumn colSalle = null!;
    private DataGridViewButtonColumn colEdit = null!;
    private DataGridViewButtonColumn colDelete = null!;
    private ComboBox _cboClasse = null!;
    private ComboBox _cboSemestre = null!;
    private DataGridView _dgvSeances = null!;

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
        grpPlanning = new GroupBox();
        lblClasse = new Label();
        lblSemestre = new Label();
        _cboClasse = new ComboBox();
        _cboSemestre = new ComboBox();
        pnlActions = new FlowLayoutPanel();
        btnActualiser = new Button();
        btnAdd = new Button();
        _dgvSeances = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colJour = new DataGridViewTextBoxColumn();
        colHeure = new DataGridViewTextBoxColumn();
        colCours = new DataGridViewTextBoxColumn();
        colEnseignant = new DataGridViewTextBoxColumn();
        colSalle = new DataGridViewTextBoxColumn();
        colEdit = new DataGridViewButtonColumn();
        colDelete = new DataGridViewButtonColumn();
        ((System.ComponentModel.ISupportInitialize)_dgvSeances).BeginInit();
        SuspendLayout();

        pnlHeader.BackColor = Color.FromArgb(139, 0, 0);
        pnlHeader.Controls.Add(lblSubtitle);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Height = 92;
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblTitle.ForeColor = Color.White;
        lblTitle.Location = new Point(24, 12);
        lblTitle.Text = "Emploi du temps";
        lblSubtitle.AutoSize = true;
        lblSubtitle.Font = new Font("Segoe UI", 10F);
        lblSubtitle.ForeColor = Color.FromArgb(245, 225, 225);
        lblSubtitle.Location = new Point(28, 56);
        lblSubtitle.Text = "Organiser les séances par classe et par semestre";

        grpPlanning.BackColor = Color.White;
        grpPlanning.Controls.Add(lblClasse);
        grpPlanning.Controls.Add(_cboClasse);
        grpPlanning.Controls.Add(lblSemestre);
        grpPlanning.Controls.Add(_cboSemestre);
        grpPlanning.Controls.Add(pnlActions);
        grpPlanning.Controls.Add(_dgvSeances);
        grpPlanning.Dock = DockStyle.Fill;
        grpPlanning.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        grpPlanning.ForeColor = Color.FromArgb(139, 0, 0);
        grpPlanning.Padding = new Padding(16);
        grpPlanning.Text = "Planification des cours";

        lblClasse.AutoSize = true;
        lblClasse.Font = new Font("Segoe UI", 9.5F);
        lblClasse.ForeColor = Color.FromArgb(45, 50, 58);
        lblClasse.Location = new Point(20, 42);
        lblClasse.Text = "Classe";
        _cboClasse.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboClasse.Font = new Font("Segoe UI", 10F);
        _cboClasse.Location = new Point(20, 66);
        _cboClasse.Size = new Size(210, 28);
        _cboClasse.Name = "_cboClasse";
        _cboClasse.SelectedIndexChanged += CboClasse_SelectedIndexChanged;

        lblSemestre.AutoSize = true;
        lblSemestre.Font = new Font("Segoe UI", 9.5F);
        lblSemestre.ForeColor = Color.FromArgb(45, 50, 58);
        lblSemestre.Location = new Point(250, 42);
        lblSemestre.Text = "Semestre";
        _cboSemestre.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboSemestre.Font = new Font("Segoe UI", 10F);
        _cboSemestre.Location = new Point(250, 66);
        _cboSemestre.Size = new Size(210, 28);
        _cboSemestre.Name = "_cboSemestre";
        _cboSemestre.SelectedIndexChanged += CboSemestre_SelectedIndexChanged;

        pnlActions.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        pnlActions.FlowDirection = FlowDirection.LeftToRight;
        pnlActions.Location = new Point(620, 60);
        pnlActions.Size = new Size(430, 40);
        pnlActions.WrapContents = false;
        btnActualiser.BackColor = Color.White;
        btnActualiser.FlatAppearance.BorderColor = Color.FromArgb(210, 214, 220);
        btnActualiser.FlatStyle = FlatStyle.Flat;
        btnActualiser.Font = new Font("Segoe UI", 9.5F);
        btnActualiser.ForeColor = Color.FromArgb(45, 50, 58);
        btnActualiser.Size = new Size(135, 36);
        btnActualiser.Text = "Actualiser";
        btnActualiser.Click += BtnActualiser_Click;
        btnAdd.BackColor = Color.FromArgb(139, 0, 0);
        btnAdd.FlatAppearance.BorderSize = 0;
        btnAdd.FlatStyle = FlatStyle.Flat;
        btnAdd.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnAdd.ForeColor = Color.White;
        btnAdd.Margin = new Padding(8, 0, 0, 0);
        btnAdd.Size = new Size(165, 36);
        btnAdd.Text = "Ajouter une séance";
        btnAdd.UseVisualStyleBackColor = false;
        btnAdd.Click += BtnAdd_Click;
        pnlActions.Controls.Add(btnActualiser);
        pnlActions.Controls.Add(btnAdd);

        _dgvSeances.AllowUserToAddRows = false;
        _dgvSeances.AllowUserToDeleteRows = false;
        _dgvSeances.AllowUserToResizeRows = false;
        _dgvSeances.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _dgvSeances.BackgroundColor = Color.White;
        _dgvSeances.BorderStyle = BorderStyle.None;
        _dgvSeances.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        _dgvSeances.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        _dgvSeances.ColumnHeadersHeight = 42;
        colId.HeaderText = "ID";
        colId.Name = "ColId";
        colId.Visible = false;
        colJour.HeaderText = "Jour";
        colJour.Name = "ColJour";
        colHeure.HeaderText = "Heure";
        colHeure.Name = "ColHeure";
        colCours.HeaderText = "Cours";
        colCours.Name = "ColCours";
        colEnseignant.HeaderText = "Enseignant";
        colEnseignant.Name = "ColEnseignant";
        colSalle.HeaderText = "Salle";
        colSalle.Name = "ColSalle";
        colEdit.HeaderText = "Modifier";
        colEdit.Name = "ColEdit";
        colEdit.Text = "Modifier";
        colEdit.UseColumnTextForButtonValue = true;
        colDelete.HeaderText = "Supprimer";
        colDelete.Name = "ColDelete";
        colDelete.Text = "Supprimer";
        colDelete.UseColumnTextForButtonValue = true;
        _dgvSeances.Columns.AddRange(new DataGridViewColumn[] { colId, colJour, colHeure, colCours, colEnseignant, colSalle, colEdit, colDelete });
        _dgvSeances.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _dgvSeances.EnableHeadersVisualStyles = false;
        _dgvSeances.Font = new Font("Segoe UI", 9.5F);
        _dgvSeances.GridColor = Color.FromArgb(232, 235, 239);
        _dgvSeances.Location = new Point(20, 118);
        _dgvSeances.MultiSelect = false;
        _dgvSeances.Name = "_dgvSeances";
        _dgvSeances.Size = new Size(1035, 470);
        _dgvSeances.RowHeadersVisible = false;
        _dgvSeances.RowTemplate.Height = 36;
        _dgvSeances.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _dgvSeances.CellContentClick += DgvSeances_CellContentClick;
        _dgvSeances.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 251);
        _dgvSeances.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _dgvSeances.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(45, 50, 58);
        _dgvSeances.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
        _dgvSeances.DefaultCellStyle.SelectionBackColor = Color.FromArgb(252, 232, 232);
        _dgvSeances.DefaultCellStyle.SelectionForeColor = Color.FromArgb(45, 50, 58);
        _dgvSeances.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 252);

        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(246, 247, 249);
        ClientSize = new Size(1120, 680);
        Controls.Add(grpPlanning);
        Controls.Add(pnlHeader);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(980, 600);
        Name = "FormEmploiDuTemps";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Emploi du temps - Gestion universitaire";
        ((System.ComponentModel.ISupportInitialize)_dgvSeances).EndInit();
        ResumeLayout(false);
    }
}
