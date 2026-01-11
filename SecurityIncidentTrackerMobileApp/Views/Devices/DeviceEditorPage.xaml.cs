using SecurityIncidentTrackerMobileApp.Models;
using Device = SecurityIncidentTrackerMobileApp.Models.Device;

namespace SecurityIncidentTrackerMobileApp.Views.Devices;

public partial class DeviceEditorPage : ContentPage
{
    Device _item;

    public DeviceEditorPage(Device item)
    {
        InitializeComponent();
        _item = item;

        // Populăm câmpurile de text dacă edităm un device existent
        if (_item != null)
        {
            entName.Text = _item.DeviceName;
            entSerial.Text = _item.SerialNumber;
        }
    }

    // Încărcăm lista când apare pagina
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Aducem departamentele din baza de date
        var departments = await App.Database.GetDepartmentsAsync();

        // Le punem în Picker (lista vizuală)
        DepartmentPicker.ItemsSource = departments;

        // Dacă device-ul are deja un departament, îl selectăm automat
        if (_item != null && _item.DepartmentID != null)
        {
            DepartmentPicker.SelectedItem = departments.FirstOrDefault(d => d.ID == _item.DepartmentID);
        }
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        // VALIDAREA
        if (string.IsNullOrWhiteSpace(entName.Text) || string.IsNullOrWhiteSpace(entSerial.Text))
        {
            await DisplayAlert("Eroare Validare", "Numele dispozitivului și Seria sunt obligatorii!", "OK");
            return;
        }

        if (entSerial.Text.Trim().Length < 3)
        {
            await DisplayAlert("Eroare Validare", "Seria este prea scurtă (minim 3 caractere).", "OK");
            return;
        }

        // Creăm obiectul dacă e nou
        if (_item == null) _item = new Device();

        // Punem datele din text în obiect
        _item.DeviceName = entName.Text;
        _item.SerialNumber = entSerial.Text;

        // Salvăm Departamentul ales 
        var selectedDept = DepartmentPicker.SelectedItem as Department;
        if (selectedDept != null)
        {
            _item.DepartmentID = selectedDept.ID;
        }

        // Salvăm în baza de date
        await App.Database.SaveDeviceAsync(_item, _item.ID == 0);
        await Navigation.PopModalAsync();
    }

    async void OnCancelClicked(object sender, EventArgs e) => await Navigation.PopModalAsync();
}