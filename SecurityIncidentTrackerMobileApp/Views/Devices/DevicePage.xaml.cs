using SecurityIncidentTrackerMobileApp.Models;
using Device = SecurityIncidentTrackerMobileApp.Models.Device;
using CommunityToolkit.Maui.Views;


namespace SecurityIncidentTrackerMobileApp.Views.Devices;

public partial class DevicePage : ContentPage
{
    public DevicePage() { InitializeComponent(); }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        listView.ItemsSource = await App.Database.GetDevicesAsync();
    }

    async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NavigationPage(new DeviceEditorPage(null)));
    }

    async void OnItemTapped(object sender, TappedEventArgs e)
    {
        var item = (Device)((Frame)sender).BindingContext;
        if (item == null) return;

        // Cream Popup-ul
        // Punem "false" ca sa ASCUNDEM butonul de Detalii (avem doar Edit si Sterge)
        var popup = new SecurityIncidentTrackerMobileApp.Views.ActionPopup(false);

        // Îl afisam
        var result = await this.ShowPopupAsync(popup);

        // Verificam ce a ales utilizatorul
        if (result is string action)
        {
            if (action == "Edit")
            {
                // Deschide pagina de editare
                await Navigation.PushModalAsync(new NavigationPage(new DeviceEditorPage(item)));
            }
            else if (action == "Delete")
            {
                // Confirmare stergere
                bool confirm = await DisplayAlert("Confirmare", "Sigur ștergi?", "DA", "NU");
                if (confirm)
                {
                    await App.Database.DeleteDeviceAsync(item.ID);
                    OnAppearing(); // Refresh la lista
                }
            }
        }
    }
}