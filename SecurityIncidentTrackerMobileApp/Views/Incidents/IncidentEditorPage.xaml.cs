using SecurityIncidentTrackerMobileApp.Models;
using Device = SecurityIncidentTrackerMobileApp.Models.Device;

namespace SecurityIncidentTrackerMobileApp.Views.Incidents;

public partial class IncidentEditorPage : ContentPage
{
    Incident _incident;

    public IncidentEditorPage(Incident incident)
    {
        InitializeComponent();
        _incident = incident;

        // Lista cu cele 4 statusuri din model
        pickerStatus.ItemsSource = new List<string> { "Open", "In Progress", "Resolved", "Closed" };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var techs = await App.Database.GetTechniciansAsync();
        var devices = await App.Database.GetDevicesAsync();
        var types = await App.Database.GetIncidentTypesAsync();
        var departments = await App.Database.GetDepartmentsAsync();

        pickerTech.ItemsSource = techs;
        pickerDevice.ItemsSource = devices;
        pickerType.ItemsSource = types;
        pickerDepartment.ItemsSource = departments;

        if (_incident != null)
        {
            entDescriere.Text = _incident.Description;
            pickerDate.Date = _incident.IncidentDate;
            pickerStatus.SelectedIndex = (int)_incident.Status;

            // Verificam ID-urile nullable (int?)
            if (_incident.TechnicianID.HasValue)
            {
                pickerTech.SelectedItem = techs.FirstOrDefault(x => x.ID == _incident.TechnicianID.Value);
            }

            if (_incident.DeviceID.HasValue)
            {
                pickerDevice.SelectedItem = devices.FirstOrDefault(x => x.ID == _incident.DeviceID.Value);
            }

            if (_incident.IncidentTypeID.HasValue)
            {
                pickerType.SelectedItem = types.FirstOrDefault(x => x.ID == _incident.IncidentTypeID.Value);
            }

            if (_incident.DepartmentID.HasValue)
            {
                pickerDepartment.SelectedItem = departments.FirstOrDefault(x => x.ID == _incident.DepartmentID.Value);
            }
        }
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        // VALIDARI

        // Verificam daca descrierea este goala, prea scurta sau daca data este in viitor    
        // "entDescriere" este numele Entry-ului din XAML
        if (string.IsNullOrWhiteSpace(entDescriere.Text))
        {
            await DisplayAlert("Eroare de validare", "Descrierea incidentului este obligatorie!", "OK");
            return; // Oprim salvarea
        }

        // Verificam daca descrierea are minim 5 caractere
        if (entDescriere.Text.Trim().Length < 5)
        {
            await DisplayAlert("Eroare de validare", "Descrierea este prea scurtă. Te rugăm să dai detalii (minim 5 caractere).", "OK");
            return; // Oprim salvarea
        }

        // Verificam daca data este in viitor
        if (pickerDate.Date > DateTime.Now)
        {
            await DisplayAlert("Eroare de validare", "Data incidentului nu poate fi în viitor.", "OK");
            return;
        }

        // SFARSIT VALIDARI

        // Daca a trecut de if-urile de mai sus, executam codul de salvare
        if (_incident == null) _incident = new Incident();

        _incident.Description = entDescriere.Text;
        _incident.IncidentDate = pickerDate.Date;

        if (pickerStatus.SelectedIndex >= 0)
            _incident.Status = (IncidentStatus)pickerStatus.SelectedIndex;

        var t = pickerTech.SelectedItem as Technician;
        var d = pickerDevice.SelectedItem as Device;
        var tp = pickerType.SelectedItem as IncidentType;
        var dept = pickerDepartment.SelectedItem as Department;

        _incident.TechnicianID = t?.ID;
        _incident.DeviceID = d?.ID;
        _incident.IncidentTypeID = tp?.ID;
        _incident.DepartmentID = dept?.ID;

        bool isNew = (_incident.ID == 0);
        await App.Database.SaveIncidentAsync(_incident, isNew);
        await Navigation.PopModalAsync();
    }

    async void OnCancelClicked(object sender, EventArgs e) => await Navigation.PopModalAsync();
}