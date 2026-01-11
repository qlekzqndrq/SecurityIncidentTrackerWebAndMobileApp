using SecurityIncidentTrackerMobileApp.Models;

namespace SecurityIncidentTrackerMobileApp.Views.Technicians;

public partial class TechnicianEditorPage : ContentPage
{
    Technician _item;

    public TechnicianEditorPage(Technician item)
    {
        InitializeComponent();
        _item = item;

        // Daca editam, completam campurile
        if (_item != null)
        {
            entFirst.Text = _item.FirstName;
            entLast.Text = _item.LastName;
            entEmail.Text = _item.Email;
            entPhone.Text = _item.Phone;
        }
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        // VALIDARE

        // Verificam numele și prenumele (sa nu fie goale)
        if (string.IsNullOrWhiteSpace(entFirst.Text) || string.IsNullOrWhiteSpace(entLast.Text))
        {
            await DisplayAlert("Eroare Validare", "Numele și Prenumele sunt obligatorii!", "OK");
            return; // Oprim salvarea
        }

        // Verificam emailul (sa nu fie gol si sa contina '@')
        if (string.IsNullOrWhiteSpace(entEmail.Text) || !entEmail.Text.Contains("@"))
        {
            await DisplayAlert("Eroare Validare", "Te rog introdu o adresă de email validă (trebuie să conțină '@').", "OK");
            return; // Oprim salvarea
        }

        // Verificam numarul de telefonul (sa nu fie gol)
        if (string.IsNullOrWhiteSpace(entPhone.Text))
        {
            await DisplayAlert("Eroare Validare", "Numărul de telefon este obligatoriu!", "OK");
            return; // Oprim salvarea
        }

        // SFARSIT VALIDARE

        // Codul de salvare (se executa doar daca datele sunt corecte)
        if (_item == null) _item = new Technician();

        _item.FirstName = entFirst.Text;
        _item.LastName = entLast.Text;
        _item.Email = entEmail.Text;
        _item.Phone = entPhone.Text;

        // Salvam
        bool isNew = (_item.ID == 0);
        await App.Database.SaveTechnicianAsync(_item, isNew);

        await Navigation.PopModalAsync();
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}