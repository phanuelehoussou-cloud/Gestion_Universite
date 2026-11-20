using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormMenuPrincipal : Form
{
    private readonly DashboardDAO _dashboardDao = new();

    private Button? _activeNavButton;

    public FormMenuPrincipal()
    {
        InitializeComponent();
        Load += (_, _) =>
        {
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
                LoadDashboardStats();
        };
    }





    private void SetActiveNavButton(Button button)
    {
        if (_activeNavButton != null)
        {
            _activeNavButton.BackColor = Color.Transparent;
            _activeNavButton.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        }

        button.BackColor = UITheme.SidebarHover;
        button.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        _activeNavButton = button;
    }





    private void ShowDashboard()
    {
        ClearMainContainer();
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
        ClearMainContainer();
        form.TopLevel = false;
        form.Dock = DockStyle.Fill;
        form.FormBorderStyle = FormBorderStyle.None;
        form.AutoScroll = true;
        _pnlMainContainer.Controls.Add(form);
        form.Show();
    }

    private void ClearMainContainer()
    {
        while (_pnlMainContainer.Controls.Count > 0)
        {
            var currentView = _pnlMainContainer.Controls[0];
            _pnlMainContainer.Controls.RemoveAt(0);
            if (!ReferenceEquals(currentView, _pnlDashboardView))
            {
                currentView.Dispose();
            }
        }
    }

    private void ShowAbout()
    {
        MessageBox.Show(
            "Institut Ivoirien de Technologie (IIT)\n" +
            "Système de gestion universitaire\n\n" +
            "Conçu et développé avec architecture N-Tiers :\n" +
            " - Module UI : Windows Forms moderne (Design Crimson IIT)\n" +
            " - Module DAO : Modèles & Repositories métier\n" +
            " - Module DataAccess : Persistance & Initialiseur de données\n\n" +
            "Tous droits réservés  IIT.",
            "À propos de l'application",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}
