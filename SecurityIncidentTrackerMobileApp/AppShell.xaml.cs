namespace SecurityIncidentTrackerMobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Inregistram ruta pentru IncidentPage, pentru navigarea globala in aplicatie (MAUI Shell)    
            Routing.RegisterRoute(nameof(Views.Incidents.IncidentPage), typeof(Views.Incidents.IncidentPage));
        }
    }
}
