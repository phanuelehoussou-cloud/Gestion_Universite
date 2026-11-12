using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormMenuPrincipal : Form
{
    private readonly DashboardDAO _dashboardDao = new();
    private Panel _pnlMainContainer = null!;
    private Panel _pnlDashboardView = null!;
    private Label _lblEtudiantsStat = null!;
    private Label _lblClassesStat = null!;
    private Label _lblRecettesStat = null!;
    private Button? _activeNavButton;

    public FormMenuPrincipal()
    {
        InitializeCustomComponents();
        LoadDashboardStats();
    }

    private void InitializeCustomComponents()
    {
        Text = "IIT - Gestion Universitaire";
        Size = new Size(1280, 780);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UITheme.AppBg;
        FormBorderStyle = FormBorderStyle.None;

        // Custom Crimson Window Title Bar
        var titleBar = UITheme.CreateTitleBar(this, "IIT - Gestion Universitaire", "🎓");
        Controls.Add(titleBar);

        // Sidebar Panel
        var pnlSidebar = new Panel
        {
            Dock = DockStyle.Left,
            Width = 240,
            BackColor = UITheme.SidebarRed,
            Padding = new Padding(0)
        };

        // Top Sidebar Logo Section
        var pnlLogo = new Panel
        {
            Dock = DockStyle.Top,
            Height = 160,
            BackColor = Color.Transparent,
            Padding = new Padding(12)
        };

        var lblCap = new Label
        {
            Text = "🎓",
            Font = new Font("Segoe UI Emoji", 26f),
            ForeColor = Color.White,
            Dock = DockStyle.Top,
            Height = 45,
            TextAlign = ContentAlignment.MiddleCenter
        };

        var lblIIT = new Label
        {
            Text = "IIT",
            Font = new Font("Segoe UI", 26f, FontStyle.Bold),
            ForeColor = Color.White,
            Dock = DockStyle.Top,
            Height = 48,
            TextAlign = ContentAlignment.MiddleCenter
        };

        var lblSlogan = new Label
        {
            Text = "« L'école qui forme par la pratique\nles leaders de demain »",
            Font = new Font("Segoe UI", 8.2f, FontStyle.Italic),
            ForeColor = Color.FromArgb(240, 220, 220),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };

        pnlLogo.Controls.Add(lblSlogan);
        pnlLogo.Controls.Add(lblIIT);
        pnlLogo.Controls.Add(lblCap);
        pnlSidebar.Controls.Add(pnlLogo);

        // Sidebar Navigation Items
        var pnlNav = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            Padding = new Padding(6, 6, 6, 6)
        };

        AddNavButton(pnlNav, "🏠  Accueil", () => ShowDashboard());
        AddNavButton(pnlNav, "📅  Années académiques", () => LoadEmbeddedForm(new FormAnneeSemestre()));
        AddNavButton(pnlNav, "🎓  Filières", () => LoadEmbeddedForm(new FormFiliere()));
        AddNavButton(pnlNav, "📊  Niveaux", () => LoadEmbeddedForm(new FormNiveau()));
        AddNavButton(pnlNav, "👥  Classes", () => LoadEmbeddedForm(new FormClasseList()));
        AddNavButton(pnlNav, "🧑‍🎓  Étudiants", () => LoadEmbeddedForm(new FormEtudiantList()));
        AddNavButton(pnlNav, "📝  Inscriptions", () => LoadEmbeddedForm(new FormInscriptionList()));
        AddNavButton(pnlNav, "👨‍🏫  Enseignants", () => LoadEmbeddedForm(new FormEnseignantList()));
        AddNavButton(pnlNav, "📚  Cours", () => LoadEmbeddedForm(new FormCours()));
        AddNavButton(pnlNav, "🏛️  Salles", () => LoadEmbeddedForm(new FormSalle()));
        AddNavButton(pnlNav, "🗓️  Emplois du temps", () => LoadEmbeddedForm(new FormEmploiDuTemps()));

        // Divider
        var pnlDiv = new Panel { Size = new Size(220, 1), BackColor = Color.FromArgb(170, 40, 40), Margin = new Padding(4, 10, 4, 10) };
        pnlNav.Controls.Add(pnlDiv);

        AddNavButton(pnlNav, "ℹ️  À propos", () => ShowAbout());
        AddNavButton(pnlNav, "🚪  Quitter", () => Application.Exit());

        pnlSidebar.Controls.Add(pnlNav);
        Controls.Add(pnlSidebar);

        // Central Host Container
        _pnlMainContainer = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = UITheme.AppBg
        };
        Controls.Add(_pnlMainContainer);

        // Build Dashboard View
        BuildDashboardView();
        ShowDashboard();
    }

    private void AddNavButton(FlowLayoutPanel container, string text, Action onClick)
    {
        var btn = new Button
        {
            Text = text,
            Size = new Size(224, 38),
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            FlatStyle = FlatStyle.Flat,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(12, 0, 0, 0),
            Cursor = Cursors.Hand,
            Margin = new Padding(2, 2, 2, 2)
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = UITheme.SidebarHover;

        btn.Click += (s, e) =>
        {
            if (_activeNavButton != null)
            {
                _activeNavButton.BackColor = Color.Transparent;
                _activeNavButton.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            }
            btn.BackColor = UITheme.SidebarHover;
            btn.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            _activeNavButton = btn;

            onClick();
        };

        container.Controls.Add(btn);
    }

    private void BuildDashboardView()
    {
        _pnlDashboardView = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = UITheme.AppBg,
            Padding = new Padding(24)
        };

        // Welcome Hero Banner
        var pnlHero = new Panel
        {
            Dock = DockStyle.Top,
            Height = 280,
            BackColor = Color.White,
            Margin = new Padding(0, 0, 0, 20)
        };
        pnlHero.Paint += (s, e) =>
        {
            using var pen = new Pen(UITheme.BorderColor, 1);
            e.Graphics.DrawRectangle(pen, 0, 0, pnlHero.Width - 1, pnlHero.Height - 1);
        };

        var lblHeroTitle = new Label
        {
            Text = "Bienvenue sur IIT – Gestion Universitaire",
            Font = new Font("Segoe UI", 20f, FontStyle.Bold),
            ForeColor = UITheme.PrimaryRed,
            Dock = DockStyle.Top,
            Height = 50,
            TextAlign = ContentAlignment.MiddleCenter
        };

        var lblHeroSubtitle = new Label
        {
            Text = "Gérer aujourd'hui, former demain !",
            Font = new Font("Segoe UI Semibold", 12f, FontStyle.Italic),
            ForeColor = UITheme.TextMuted,
            Dock = DockStyle.Top,
            Height = 35,
            TextAlign = ContentAlignment.MiddleCenter
        };

        // Campus Building Graphic Area
        var pnlCampusGraphic = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(248, 249, 252)
        };
        pnlCampusGraphic.Paint += (s, e) =>
        {
            // Decorative campus skyline / wave banner
            using var brush = new SolidBrush(Color.FromArgb(254, 242, 242));
            e.Graphics.FillRectangle(brush, 0, 0, pnlCampusGraphic.Width, pnlCampusGraphic.Height);

            // IIT Building representation
            using var bBuilding = new SolidBrush(Color.FromArgb(235, 238, 244));
            e.Graphics.FillRectangle(bBuilding, pnlCampusGraphic.Width / 2 - 250, 20, 500, 140);

            using var bRoof = new SolidBrush(UITheme.PrimaryRed);
            e.Graphics.FillRectangle(bRoof, pnlCampusGraphic.Width / 2 - 260, 15, 520, 15);

            // Windows
            using var bWin = new SolidBrush(Color.FromArgb(180, 205, 235));
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    e.Graphics.FillRectangle(bWin, pnlCampusGraphic.Width / 2 - 230 + (c * 60), 45 + (r * 32), 40, 22);
                }
            }

            // Central IIT Sign
            using var bSign = new SolidBrush(UITheme.PrimaryRed);
            e.Graphics.FillRectangle(bSign, pnlCampusGraphic.Width / 2 - 45, 45, 90, 40);
            TextRenderer.DrawText(e.Graphics, "IIT", new Font("Segoe UI", 16f, FontStyle.Bold), new Rectangle(pnlCampusGraphic.Width / 2 - 45, 45, 90, 40), Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        };

        pnlHero.Controls.Add(pnlCampusGraphic);
        pnlHero.Controls.Add(lblHeroSubtitle);
        pnlHero.Controls.Add(lblHeroTitle);

        // KPI Stat Cards Container (3 Cards)
        var pnlKpis = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 150,
            ColumnCount = 3,
            RowCount = 1,
            Margin = new Padding(0, 20, 0, 20)
        };
        pnlKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        pnlKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        pnlKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

        // Card 1: 👥 Étudiants inscrits
        var cardEtudiants = CreateStatCard("👥", "1 248", "Étudiants inscrits", out _lblEtudiantsStat);
        pnlKpis.Controls.Add(cardEtudiants, 0, 0);

        // Card 2: 📅 Classes actives
        var cardClasses = CreateStatCard("📅", "12", "Classes actives", out _lblClassesStat);
        pnlKpis.Controls.Add(cardClasses, 1, 0);

        // Card 3: 🎓 Recettes du jour
        var cardRecettes = CreateStatCard("🎓", "2 850 000 FCFA", "Recettes du jour", out _lblRecettesStat);
        pnlKpis.Controls.Add(cardRecettes, 2, 0);

        // Bottom Banner
        var pnlFooter = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 45,
            BackColor = UITheme.PrimaryRed
        };
        pnlFooter.Paint += (s, e) =>
        {
            using var path = UITheme.GetRoundedRectangle(new Rectangle(0, 0, pnlFooter.Width - 1, pnlFooter.Height - 1), 10);
            pnlFooter.Region = new Region(path);
        };

        var lblFooter = new Label
        {
            Text = "Une université d'excellence au service de la jeunesse",
            Font = new Font("Segoe UI", 11f, FontStyle.Bold),
            ForeColor = Color.White,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };
        pnlFooter.Controls.Add(lblFooter);

        _pnlDashboardView.Controls.Add(pnlFooter);
        _pnlDashboardView.Controls.Add(pnlKpis);
        _pnlDashboardView.Controls.Add(pnlHero);
    }

    private Panel CreateStatCard(string icon, string val, string label, out Label lblVal)
    {
        var card = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(16),
            Margin = new Padding(10)
        };
        card.Paint += (s, e) =>
        {
            using var path = UITheme.GetRoundedRectangle(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 14);
            card.Region = new Region(path);
            using var pen = new Pen(UITheme.BorderColor, 1);
            e.Graphics.DrawPath(pen, path);
        };

        var lblIcon = new Label
        {
            Text = icon,
            Font = new Font("Segoe UI Emoji", 24f),
            ForeColor = UITheme.PrimaryRed,
            Dock = DockStyle.Top,
            Height = 42,
            TextAlign = ContentAlignment.MiddleCenter
        };

        lblVal = new Label
        {
            Text = val,
            Font = new Font("Segoe UI", 18f, FontStyle.Bold),
            ForeColor = UITheme.TextPrimary,
            Dock = DockStyle.Top,
            Height = 40,
            TextAlign = ContentAlignment.MiddleCenter
        };

        var lblText = new Label
        {
            Text = label,
            Font = new Font("Segoe UI Semibold", 10.5f),
            ForeColor = UITheme.TextMuted,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };

        card.Controls.Add(lblText);
        card.Controls.Add(lblVal);
        card.Controls.Add(lblIcon);

        return card;
    }

    private void ShowDashboard()
    {
        _pnlMainContainer.Controls.Clear();
        LoadDashboardStats();
        _pnlMainContainer.Controls.Add(_pnlDashboardView);
    }

    private void LoadDashboardStats()
    {
        var stats = _dashboardDao.GetStatistiques();
        _lblEtudiantsStat.Text = $"{stats.TotalEtudiantsInscrits:N0}".Replace(',', ' ');
        _lblClassesStat.Text = stats.TotalClassesActives.ToString();
        _lblRecettesStat.Text = $"{stats.RecettesDuJour:N0} FCFA".Replace(',', ' ');
    }

    private void LoadEmbeddedForm(Form form)
    {
        _pnlMainContainer.Controls.Clear();
        form.TopLevel = false;
        form.Dock = DockStyle.Fill;
        form.FormBorderStyle = FormBorderStyle.None;
        _pnlMainContainer.Controls.Add(form);
        form.Show();
    }

    private void ShowAbout()
    {
        MessageBox.Show(
            "IIT – Institut d'Ingénierie et de Technologie\n" +
            "Système de Gestion Universitaire Intégré v2.0\n\n" +
            "Conçu et développé avec architecture N-Tiers :\n" +
            " - Module UI : Windows Forms moderne (Design Crimson IIT)\n" +
            " - Module DAO : Modèles & Repositories métier\n" +
            " - Module DataAccess : Persistance & Initialiseur de données\n\n" +
            "Tous droits réservés © IIT.",
            "À propos de l'application",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}
