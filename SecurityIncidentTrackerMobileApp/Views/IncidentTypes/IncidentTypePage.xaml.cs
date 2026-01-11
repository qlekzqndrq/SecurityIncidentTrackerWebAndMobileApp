using SecurityIncidentTrackerMobileApp.Models;
using CommunityToolkit.Maui.Views;

namespace SecurityIncidentTrackerMobileApp.Views.IncidentTypes;

public partial class IncidentTypePage : ContentPage
{
    public IncidentTypePage() { InitializeComponent(); }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        listView.ItemsSource = await App.Database.GetIncidentTypesAsync();
    }

    async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NavigationPage(new IncidentTypeEditorPage(null)));
    }

    async void OnItemTapped(object sender, TappedEventArgs e)
    {
        var item = (IncidentType)((Frame)sender).BindingContext;
        if (item == null) return;

        // Cream Popup-ul
        // Punem "false" ca sa ASCUNDEM butonul de Detalii
        var popup = new SecurityIncidentTrackerMobileApp.Views.ActionPopup(false);

        // Il afisam 
        var result = await this.ShowPopupAsync(popup);

        // Verificam ce a ales utilizatorul ("Edit" sau "Delete")
        if (result is string action)
        {
            if (action == "Edit")
            {
                // Deschide pagina de editare
                await Navigation.PushModalAsync(new NavigationPage(new IncidentTypeEditorPage(item)));
            }
            else if (action == "Delete")
            {
                // Confirmare stergere
                bool confirm = await DisplayAlert("Confirmare", "Sigur stergi?", "DA", "NU");
                if (confirm)
                {
                    await App.Database.DeleteIncidentTypeAsync(item.ID);
                    OnAppearing(); // Refresh lista
                }
            }
        }
    }
}