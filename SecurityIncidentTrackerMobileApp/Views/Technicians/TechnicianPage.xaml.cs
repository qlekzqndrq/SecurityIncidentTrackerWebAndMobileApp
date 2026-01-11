using SecurityIncidentTrackerMobileApp.Models;
using CommunityToolkit.Maui.Views;

namespace SecurityIncidentTrackerMobileApp.Views.Technicians;

public partial class TechnicianPage : ContentPage
{
    public TechnicianPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Incarca lista de la server
        listView.ItemsSource = await App.Database.GetTechniciansAsync();
    }

    async void OnAddClicked(object sender, EventArgs e)
    {
        // Deschide editorul gol (null)
        await Navigation.PushModalAsync(new NavigationPage(new TechnicianEditorPage(null)));
    }

    async void OnItemTapped(object sender, TappedEventArgs e)
    {
        var item = (Technician)((Frame)sender).BindingContext;
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
                await Navigation.PushModalAsync(new NavigationPage(new TechnicianEditorPage(item)));
            }
            else if (action == "Delete")
            {
                // Confirmare stergere
                bool confirm = await DisplayAlert("Sterge", "Sigur stergi acest tehnician?", "DA", "NU");
                if (confirm)
                {
                    await App.Database.DeleteTechnicianAsync(item.ID);
                    OnAppearing(); // Refresh lista
                }
            }
        }
    }
}