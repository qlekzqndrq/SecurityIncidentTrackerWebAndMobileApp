using SecurityIncidentTrackerMobileApp.Models;

namespace SecurityIncidentTrackerMobileApp.Views;

    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            // 1. Validare simplă (DOAR Email și Password)
            if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Eroare", "Completeaza email si parola", "OK");
                return;
            }

            LoadingSpinner.IsRunning = true;
            try
            {
                // 2. Creăm obiectul User (fără FullName)
                var newUser = new User
                {
                    Email = EmailEntry.Text,
                    Password = PasswordEntry.Text
                };

                // 3. Trimitem la server
                await App.Database.RegisterAsync(newUser);

                await DisplayAlert("Succes", "Cont creat! Te redirectionez la Login.", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eroare", ex.Message, "OK");
            }
            finally
            {
                LoadingSpinner.IsRunning = false;
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Dacă userul se răzgândește, îl ducem înapoi la Login
            await Navigation.PopAsync();
        }
    }