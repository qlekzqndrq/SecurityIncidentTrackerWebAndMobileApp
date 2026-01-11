using SecurityIncidentTrackerMobileApp.Models;

namespace SecurityIncidentTrackerMobileApp.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Logout", "Ești sigur că vrei să te deconectezi?", "Da", "Nu");

            if (answer)
            {
                try
                {
                    // ȘTERGE DATELE DE LOGIN
                    SecureStorage.RemoveAll();

                    // Du-te înapoi la LoginPage
                    Application.Current.MainPage = new NavigationPage(new LoginPage())
                    {
                        BarBackgroundColor = Colors.Black,
                        BarTextColor = Colors.White
                    };
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Eroare", $"Nu s-a putut deconecta: {ex.Message}", "OK");
                }
            }
        }
    }
}