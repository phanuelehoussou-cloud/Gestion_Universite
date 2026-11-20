#nullable enable
namespace Gestion_Universite;

partial class FormSeanceDetail
{
    private System.ComponentModel.IContainer? components = null;
    private Panel pnlHeader = null!;
    private Label lblTitle = null!;
    private Label lblSubtitle = null!;
    private GroupBox grpSeance = null!;
    private TableLayoutPanel formLayout = null!;
    private Label lblClasse = null!;
    private Label lblSemestre = null!;
    private Label lblJour = null!;
    private Label lblCreneau = null!;
    private Label lblDate = null!;
    private Label lblCours = null!;
    private Label lblEnseignant = null!;
    private Label lblSalle = null!;
    private FlowLayoutPanel pnlButtons = null!;
    private Button btnSave = null!;
    private Button btnCancel = null!;
    private ComboBox _cboClasse = null!;
    private ComboBox _cboSemestre = null!;
    private ComboBox _cboCours = null!;
    private ComboBox _cboEnseignant = null!;
    private ComboBox _cboSalle = null!;
    private ComboBox _cboJour = null!;
    private ComboBox _cboCreneau = null!;
    private DateTimePicker _dateSeance = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlHeader = new Panel();
        lblSubtitle = new Label();
        lblTitle = new Label();
        grpSeance = new GroupBox();
        formLayout = new TableLayoutPanel();
        lblClasse = new Label();
        lblSemestre = new Label();
        lblJour = new Label();
        lblCreneau = new Label();
        lblDate = new Label();
        lblCours = new Label();
        lblEnseignant = new Label();
        lblSalle = new Label();
        _cboClasse = new ComboBox();
        _cboSemestre = new ComboBox();
        _cboJour = new ComboBox();
        _cboCreneau = new ComboBox();
        _dateSeance = new DateTimePicker();
        _cboCours = new ComboBox();
        _cboEnseignant = new ComboBox();
        _cboSalle = new ComboBox();
        pnlButtons = new FlowLayoutPanel();
        btnSave = new Button();
        btnCancel = new Button();
        pnlHeader.SuspendLayout();
        grpSeance.SuspendLayout();
        formLayout.SuspendLayout();
        pnlButtons.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(139, 0, 0);
        pnlHeader.Controls.Add(lblSubtitle);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(620, 88);
        pnlHeader.TabIndex = 1;
        // 
        // lblSubtitle
        // 
        lblSubtitle.AutoSize = true;
        lblSubtitle.Font = new Font("Segoe UI", 9.5F);
        lblSubtitle.ForeColor = Color.FromArgb(245, 225, 225);
        lblSubtitle.Location = new Point(25, 53);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(414, 25);
        lblSubtitle.TabIndex = 0;
        lblSubtitle.Text = "Définir le cours, l'enseignant, la salle et l'horaire";
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
        lblTitle.ForeColor = Color.White;
        lblTitle.Location = new Point(22, 9);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(339, 46);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "Planifier une séance";
        // 
        // grpSeance
        // 
        grpSeance.BackColor = Color.White;
        grpSeance.Controls.Add(formLayout);
        grpSeance.Dock = DockStyle.Fill;
        grpSeance.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        grpSeance.ForeColor = Color.FromArgb(139, 0, 0);
        grpSeance.Location = new Point(0, 88);
        grpSeance.Name = "grpSeance";
        grpSeance.Padding = new Padding(15);
        grpSeance.Size = new Size(620, 502);
        grpSeance.TabIndex = 0;
        grpSeance.TabStop = false;
        grpSeance.Text = "Informations de la séance";
        // 
        // formLayout
        // 
        formLayout.ColumnCount = 2;
        formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        formLayout.Controls.Add(lblClasse, 0, 0);
        formLayout.Controls.Add(lblSemestre, 0, 1);
        formLayout.Controls.Add(lblJour, 0, 2);
        formLayout.Controls.Add(lblCreneau, 0, 3);
        formLayout.Controls.Add(lblDate, 0, 4);
        formLayout.Controls.Add(lblCours, 0, 5);
        formLayout.Controls.Add(lblEnseignant, 0, 6);
        formLayout.Controls.Add(lblSalle, 0, 7);
        formLayout.Controls.Add(_cboClasse, 1, 0);
        formLayout.Controls.Add(_cboSemestre, 1, 1);
        formLayout.Controls.Add(_cboJour, 1, 2);
        formLayout.Controls.Add(_cboCreneau, 1, 3);
        formLayout.Controls.Add(_dateSeance, 1, 4);
        formLayout.Controls.Add(_cboCours, 1, 5);
        formLayout.Controls.Add(_cboEnseignant, 1, 6);
        formLayout.Controls.Add(_cboSalle, 1, 7);
        formLayout.Controls.Add(pnlButtons, 1, 8);
        formLayout.Dock = DockStyle.Fill;
        formLayout.Location = new Point(15, 43);
        formLayout.Name = "formLayout";
        formLayout.Padding = new Padding(12, 16, 12, 8);
        formLayout.RowCount = 9;
        formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        formLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        formLayout.Size = new Size(590, 444);
        formLayout.TabIndex = 0;
        // 
        // lblClasse
        // 
        lblClasse.Anchor = AnchorStyles.Left;
        lblClasse.AutoSize = true;
        lblClasse.Font = new Font("Segoe UI", 9.5F);
        lblClasse.ForeColor = Color.FromArgb(45, 50, 58);
        lblClasse.Location = new Point(15, 24);
        lblClasse.Name = "lblClasse";
        lblClasse.Size = new Size(65, 25);
        lblClasse.TabIndex = 0;
        lblClasse.Text = "Classe";
        // 
        // lblSemestre
        // 
        lblSemestre.Anchor = AnchorStyles.Left;
        lblSemestre.AutoSize = true;
        lblSemestre.Font = new Font("Segoe UI", 9.5F);
        lblSemestre.ForeColor = Color.FromArgb(45, 50, 58);
        lblSemestre.Location = new Point(15, 66);
        lblSemestre.Name = "lblSemestre";
        lblSemestre.Size = new Size(89, 25);
        lblSemestre.TabIndex = 1;
        lblSemestre.Text = "Semestre";
        // 
        // lblJour
        // 
        lblJour.Anchor = AnchorStyles.Left;
        lblJour.AutoSize = true;
        lblJour.Font = new Font("Segoe UI", 9.5F);
        lblJour.ForeColor = Color.FromArgb(45, 50, 58);
        lblJour.Location = new Point(15, 108);
        lblJour.Name = "lblJour";
        lblJour.Size = new Size(48, 25);
        lblJour.TabIndex = 2;
        lblJour.Text = "Jour";
        // 
        // lblCreneau
        // 
        lblCreneau.Anchor = AnchorStyles.Left;
        lblCreneau.AutoSize = true;
        lblCreneau.Font = new Font("Segoe UI", 9.5F);
        lblCreneau.ForeColor = Color.FromArgb(45, 50, 58);
        lblCreneau.Location = new Point(15, 150);
        lblCreneau.Name = "lblCreneau";
        lblCreneau.Size = new Size(83, 25);
        lblCreneau.TabIndex = 3;
        lblCreneau.Text = "Créneau";
        // 
        // lblDate
        // 
        lblDate.Anchor = AnchorStyles.Left;
        lblDate.AutoSize = true;
        lblDate.Font = new Font("Segoe UI", 9.5F);
        lblDate.ForeColor = Color.FromArgb(45, 50, 58);
        lblDate.Location = new Point(15, 192);
        lblDate.Name = "lblDate";
        lblDate.Size = new Size(51, 25);
        lblDate.TabIndex = 4;
        lblDate.Text = "Date";
        // 
        // lblCours
        // 
        lblCours.Anchor = AnchorStyles.Left;
        lblCours.AutoSize = true;
        lblCours.Font = new Font("Segoe UI", 9.5F);
        lblCours.ForeColor = Color.FromArgb(45, 50, 58);
        lblCours.Location = new Point(15, 234);
        lblCours.Name = "lblCours";
        lblCours.Size = new Size(61, 25);
        lblCours.TabIndex = 5;
        lblCours.Text = "Cours";
        // 
        // lblEnseignant
        // 
        lblEnseignant.Anchor = AnchorStyles.Left;
        lblEnseignant.AutoSize = true;
        lblEnseignant.Font = new Font("Segoe UI", 9.5F);
        lblEnseignant.ForeColor = Color.FromArgb(45, 50, 58);
        lblEnseignant.Location = new Point(15, 276);
        lblEnseignant.Name = "lblEnseignant";
        lblEnseignant.Size = new Size(105, 25);
        lblEnseignant.TabIndex = 6;
        lblEnseignant.Text = "Enseignant";
        // 
        // lblSalle
        // 
        lblSalle.Anchor = AnchorStyles.Left;
        lblSalle.AutoSize = true;
        lblSalle.Font = new Font("Segoe UI", 9.5F);
        lblSalle.ForeColor = Color.FromArgb(45, 50, 58);
        lblSalle.Location = new Point(15, 318);
        lblSalle.Name = "lblSalle";
        lblSalle.Size = new Size(52, 25);
        lblSalle.TabIndex = 7;
        lblSalle.Text = "Salle";
        // 
        // _cboClasse
        // 
        _cboClasse.Dock = DockStyle.Fill;
        _cboClasse.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboClasse.Font = new Font("Segoe UI", 10F);
        _cboClasse.Location = new Point(155, 19);
        _cboClasse.Name = "_cboClasse";
        _cboClasse.Size = new Size(420, 36);
        _cboClasse.TabIndex = 8;
        // 
        // _cboSemestre
        // 
        _cboSemestre.Dock = DockStyle.Fill;
        _cboSemestre.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboSemestre.Font = new Font("Segoe UI", 10F);
        _cboSemestre.Location = new Point(155, 61);
        _cboSemestre.Name = "_cboSemestre";
        _cboSemestre.Size = new Size(420, 36);
        _cboSemestre.TabIndex = 9;
        // 
        // _cboJour
        // 
        _cboJour.Dock = DockStyle.Fill;
        _cboJour.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboJour.Font = new Font("Segoe UI", 10F);
        _cboJour.Items.AddRange(new object[] { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi" });
        _cboJour.Location = new Point(155, 103);
        _cboJour.Name = "_cboJour";
        _cboJour.Size = new Size(420, 36);
        _cboJour.TabIndex = 10;
        // 
        // _cboCreneau
        // 
        _cboCreneau.Dock = DockStyle.Fill;
        _cboCreneau.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboCreneau.Font = new Font("Segoe UI", 10F);
        _cboCreneau.Items.AddRange(new object[] { "08h-10h", "10h-12h", "12h-14h", "14h-16h", "16h-18h", "18h-20h" });
        _cboCreneau.Location = new Point(155, 145);
        _cboCreneau.Name = "_cboCreneau";
        _cboCreneau.Size = new Size(420, 36);
        _cboCreneau.TabIndex = 11;
        // 
        // _dateSeance
        // 
        _dateSeance.Dock = DockStyle.Fill;
        _dateSeance.Font = new Font("Segoe UI", 10F);
        _dateSeance.Format = DateTimePickerFormat.Short;
        _dateSeance.Location = new Point(155, 187);
        _dateSeance.Name = "_dateSeance";
        _dateSeance.Size = new Size(420, 34);
        _dateSeance.TabIndex = 12;
        // 
        // _cboCours
        // 
        _cboCours.Dock = DockStyle.Fill;
        _cboCours.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboCours.Font = new Font("Segoe UI", 10F);
        _cboCours.Location = new Point(155, 229);
        _cboCours.Name = "_cboCours";
        _cboCours.Size = new Size(420, 36);
        _cboCours.TabIndex = 13;
        // 
        // _cboEnseignant
        // 
        _cboEnseignant.Dock = DockStyle.Fill;
        _cboEnseignant.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboEnseignant.Font = new Font("Segoe UI", 10F);
        _cboEnseignant.Location = new Point(155, 271);
        _cboEnseignant.Name = "_cboEnseignant";
        _cboEnseignant.Size = new Size(420, 36);
        _cboEnseignant.TabIndex = 14;
        // 
        // _cboSalle
        // 
        _cboSalle.Dock = DockStyle.Fill;
        _cboSalle.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboSalle.Font = new Font("Segoe UI", 10F);
        _cboSalle.Location = new Point(155, 313);
        _cboSalle.Name = "_cboSalle";
        _cboSalle.Size = new Size(420, 36);
        _cboSalle.TabIndex = 15;
        // 
        // pnlButtons
        // 
        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnCancel);
        pnlButtons.Dock = DockStyle.Fill;
        pnlButtons.Location = new Point(155, 355);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Padding = new Padding(0, 8, 0, 0);
        pnlButtons.Size = new Size(420, 78);
        pnlButtons.TabIndex = 16;
        pnlButtons.WrapContents = false;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(139, 0, 0);
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(3, 11);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(190, 38);
        btnSave.TabIndex = 0;
        btnSave.Text = "Enregistrer la séance";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += BtnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.BackColor = Color.White;
        btnCancel.FlatAppearance.BorderColor = Color.FromArgb(210, 214, 220);
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Segoe UI", 9.5F);
        btnCancel.ForeColor = Color.FromArgb(45, 50, 58);
        btnCancel.Location = new Point(206, 8);
        btnCancel.Margin = new Padding(10, 0, 0, 0);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(110, 38);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "Annuler";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += BtnCancel_Click;
        // 
        // FormSeanceDetail
        // 
        AutoScaleDimensions = new SizeF(11F, 28F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(246, 247, 249);
        ClientSize = new Size(620, 590);
        Controls.Add(grpSeance);
        Controls.Add(pnlHeader);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(580, 560);
        Name = "FormSeanceDetail";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Séance - Gestion universitaire";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        grpSeance.ResumeLayout(false);
        formLayout.ResumeLayout(false);
        formLayout.PerformLayout();
        pnlButtons.ResumeLayout(false);
        ResumeLayout(false);
    }
}
