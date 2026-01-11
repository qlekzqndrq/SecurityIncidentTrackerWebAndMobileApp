using Microsoft.Maui.Controls;
using SecurityIncidentTrackerMobileApp.Views.Incidents;


namespace SecurityIncidentTrackerMobileApp.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    // 1. PENTRU BUTONUL DE SUS (Button = Clicked = EventArgs)
    // ATENTIE: Aici folosim 'EventArgs', nu 'TappedEventArgs'
    async void OnReportIncidentTapped(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NavigationPage(new IncidentEditorPage(null)));
    }

    // 2. PENTRU FRAME-UL DE JOS (Frame = Tapped = TappedEventArgs)
    // Aici ramane 'TappedEventArgs'
    async void OnIncidentListTapped(object sender, TappedEventArgs e)
    {
        // Cele doua slash-uri "//" inseamna "Mergi la un Tab Principal"
        // Atentie: "Incidents" trebuie sa fie exact ce ai scris la Route in AppShell!
        await Shell.Current.GoToAsync("//Incidents");
    }
}