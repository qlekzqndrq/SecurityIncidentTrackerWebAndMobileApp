using SecurityIncidentTrackerMobileApp.Models;

namespace SecurityIncidentTrackerMobileApp.Views.Departments;

public partial class DepartmentEditorPage : ContentPage
{
    Department _item;

    public DepartmentEditorPage(Department item)
    {
        InitializeComponent();
        _item = item;

        if (_item != null)
            entName.Text = _item.DepartmentName;
    }

    // Încărcăm tehnicienii când se deschide pagina 
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Luăm toți tehnicienii de pe server
        var allTechnicians = await App.Database.GetTechniciansAsync();

        // Dacă edităm un departament existent, bifăm tehnicienii care sunt deja alocați
        if (_item != null && _item.AssignedTechnicians != null)
        {
            foreach (var tech in allTechnicians)
            {
                if (_item.AssignedTechnicians.Any(at => at.ID == tech.ID))
                {
                    tech.IsSelected = true;
                }
            }
        }

        // Punem lista în CollectionView-ul din XAML
        TechniciansSelectionList.ItemsSource = allTechnicians;
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        // Validări
        if (string.IsNullOrWhiteSpace(entName.Text))
        {
            await DisplayAlert("Eroare Validare", "Numele departamentului este obligatoriu!", "OK");
            return;
        }

        if (entName.Text.Trim().Length < 2)
        {
            await DisplayAlert("Eroare Validare", "Numele este prea scurt (minim 2 litere).", "OK");
            return;
        }

        if (_item == null) _item = new Department();

        _item.DepartmentName = entName.Text;

        // Colectăm tehnicienii bifați înainte de salvare 
        if (TechniciansSelectionList.ItemsSource is IEnumerable<Technician> allTechs)
        {
            _item.AssignedTechnicians = allTechs.Where(t => t.IsSelected).ToList();
        }

        await App.Database.SaveDepartmentAsync(_item, _item.ID == 0);
        await Navigation.PopModalAsync();
    }

    async void OnCancelClicked(object sender, EventArgs e) => await Navigation.PopModalAsync();
}