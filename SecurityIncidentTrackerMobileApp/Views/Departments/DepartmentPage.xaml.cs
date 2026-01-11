using SecurityIncidentTrackerMobileApp.Models;
using CommunityToolkit.Maui.Views;

namespace SecurityIncidentTrackerMobileApp.Views.Departments;

public partial class DepartmentPage : ContentPage
{
    public DepartmentPage() { InitializeComponent(); }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        listView.ItemsSource = await App.Database.GetDepartmentsAsync();
    }

    async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NavigationPage(new DepartmentEditorPage(null)));
    }

    async void OnItemTapped(object sender, TappedEventArgs e)
    {
        var item = (Department)((Frame)sender).BindingContext;
        if (item == null) return;

        // Creeaza Popup-ul
        // Punem "false" (sau nimic) ca sa NU apara butonul de Detalii
        var popup = new SecurityIncidentTrackerMobileApp.Views.ActionPopup(false);

        // Il afiseaza
        var result = await this.ShowPopupAsync(popup);

        // Verifica ce a ales utilizatorul ("Edit" sau "Delete" vin din ActionPopup.xaml.cs)
        if (result is string action)
        {
            if (action == "Edit")
            {
                // Deschide pagina de editare
                await Navigation.PushModalAsync(new NavigationPage(new DepartmentEditorPage(item)));
            }
            else if (action == "Delete")
            {
                // Confirmare stergere
                bool confirm = await DisplayAlert("Confirmare", "Sigur stergi?", "DA", "NU");
                if (confirm)
                {
                    await App.Database.DeleteDepartmentAsync(item.ID);
                    OnAppearing(); // Refresh lista
                }
            }
        }
    }
}