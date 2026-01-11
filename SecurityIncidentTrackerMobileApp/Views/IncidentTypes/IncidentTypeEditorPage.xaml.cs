using SecurityIncidentTrackerMobileApp.Models;

namespace SecurityIncidentTrackerMobileApp.Views.IncidentTypes;

public partial class IncidentTypeEditorPage : ContentPage
{
    IncidentType _item;
    public IncidentTypeEditorPage(IncidentType item)
    {
        InitializeComponent();
        _item = item;
        if (_item != null)
        {
            entName.Text = _item.TypeName;
            entDuration.Text = _item.EstimatedDuration.ToString();
        }
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        // VALIDARE

        // Verificam daca descrierea este goala sau are doar spatii albe  
        if (string.IsNullOrWhiteSpace(entName.Text))
        {
            await DisplayAlert("Eroare Validare", "Descrierea tipului este obligatorie!", "OK");
            return; // Oprim salvarea
        }

        // Verificam durata estimata
        // Trebuie sa fie un numar (int.TryParse returneaza false daca e text)
        // Trebuie sa fie mai mare ca 0
        if (!int.TryParse(entDuration.Text, out int duration) || duration <= 0)
        {
            await DisplayAlert("Eroare Validare", "Durata estimată trebuie să fie un număr valid (ex: 60, 120) mai mare ca 0.", "OK");
            return; // Oprim salvarea
        }

        // SFARSIT VALIDARE

        // Codul de salvare
        if (_item == null) _item = new IncidentType();

        _item.TypeName = entName.Text;
        _item.EstimatedDuration = duration; // Folosim variabila 'duration' validata mai sus

        await App.Database.SaveIncidentTypeAsync(_item, _item.ID == 0);
        await Navigation.PopModalAsync();
    }
    async void OnCancelClicked(object sender, EventArgs e) => await Navigation.PopModalAsync();
}