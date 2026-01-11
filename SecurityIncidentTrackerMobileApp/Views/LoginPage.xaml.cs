using SecurityIncidentTrackerMobileApp;
using System.Text;
using System.Text.Json;

namespace SecurityIncidentTrackerMobileApp.Views;

    public partial class LoginPage : ContentPage
    {
        private const string BaseUrl = "http://10.0.2.2:5214/api/Auth/";

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Eroare", "Te rugăm să introduci email-ul și parola.", "OK");
                return;
            }
            await LoginUser();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        private async Task LoginUser()
        {
            if (LoadingSpinner != null) LoadingSpinner.IsRunning = true;

            try
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                using (var client = new HttpClient(handler))
                {
                    var loginData = new { Email = EmailEntry.Text, Password = PasswordEntry.Text };
                    var json = JsonSerializer.Serialize(loginData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(BaseUrl + "Login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // SALVEAZĂ LOGIN-UL
                        await SecureStorage.SetAsync("user_email", EmailEntry.Text);
                        await SecureStorage.SetAsync("is_logged_in", "true");

                        await DisplayAlert("Succes", "Te-ai logat cu succes!", "OK");

                        if (Application.Current != null)
                        {
                            Application.Current.MainPage = new AppShell();
                        }
                    }
                    else
                    {
                        await DisplayAlert("Eroare", "Email sau parolă incorectă.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Conexiune eșuată", $"Nu s-a putut contacta serverul.\nDetalii: {ex.Message}", "OK");
            }
            finally
            {
                if (LoadingSpinner != null) LoadingSpinner.IsRunning = false;
            }
        }
    }
