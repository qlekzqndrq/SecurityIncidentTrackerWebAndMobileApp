using SecurityIncidentTrackerMobileApp.Models;
using CommunityToolkit.Maui.Views;

namespace SecurityIncidentTrackerMobileApp.Views.Incidents;

public partial class IncidentPage : ContentPage
{
    public IncidentPage()
    {
        InitializeComponent();

        // Cand cineva da "RefreshList", apelam metoda RefreshData pentru a reincarca lista        
        MessagingCenter.Subscribe<App>(this, "RefreshList", async (sender) =>
        {
            await RefreshData();
        });
    }

    private async Task RefreshData()
    {
        listView.ItemsSource = null;
        listView.ItemsSource = await App.Database.GetIncidentsAsync();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshData();
    }

    async void OnAddClicked(object sender, EventArgs e)
    {
        // Deschide editorul gol
        await Navigation.PushModalAsync(new NavigationPage(new IncidentEditorPage(null)));
    }

    async void OnIncidentTapped(object sender, TappedEventArgs e)
    {
        var frame = (Frame)sender;
        var incident = (Incident)frame.BindingContext;
        if (incident == null) return;

        // Cream Popup-ul
        var popup = new SecurityIncidentTrackerMobileApp.Views.ActionPopup(true);

        // Il afisam
        var result = await this.ShowPopupAsync(popup);

        // Verificam ce a ales utilizatorul
        if (result is string action)
        {
            if (action == "Edit")
            {
                // Deschidem Edit
                await Navigation.PushModalAsync(new NavigationPage(new IncidentEditorPage(incident)));
            }
            else if (action == "Details") // AICI PRINDE CLICK-UL PE DETALII
            {
                // Deschidem Detalii
                await Navigation.PushModalAsync(new NavigationPage(new IncidentDetailsPage(incident)));
            }
            else if (action == "Delete")
            {
                // Stergem
                bool confirm = await DisplayAlert("Șterge", "Sigur vrei să ștergi?", "DA", "NU");
                if (confirm)
                {
                    await App.Database.DeleteIncidentAsync(incident.ID);

                    await RefreshData();
                }
            }
        }
    }
}