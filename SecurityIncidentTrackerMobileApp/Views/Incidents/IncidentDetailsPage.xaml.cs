using SecurityIncidentTrackerMobileApp.Models;
using Device = SecurityIncidentTrackerMobileApp.Models.Device;

namespace SecurityIncidentTrackerMobileApp.Views.Incidents;

public partial class IncidentDetailsPage : ContentPage
{
    Incident _incident;

    public IncidentDetailsPage(Incident incident)
    {
        InitializeComponent();
        _incident = incident;

        if (_incident != null)
        {
            lblDescriere.Text = _incident.Description;
            lblData.Text = _incident.IncidentDate.ToString("dd/MM/yyyy");
            lblStatus.Text = _incident.Status.ToString();
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_incident == null) return;

        try
        {
            // TEHNICIAN (cautam dupa ID daca exista)
            if (_incident.TechnicianID.HasValue)
            {
                var list = await App.Database.GetTechniciansAsync();
                var item = list.FirstOrDefault(x => x.ID == _incident.TechnicianID.Value);
                // Folosim FullName din model
                lblTehnician.Text = item?.FullName ?? "Necunoscut";
            }
            else lblTehnician.Text = "Nealocat";

            // LOGICA PENTRU DEPARTAMENT 
            if (_incident.DepartmentID.HasValue)
            {
                var list = await App.Database.GetDepartmentsAsync();
                var item = list.FirstOrDefault(x => x.ID == _incident.DepartmentID.Value);

                // Folosim DepartmentName din model
                lblDepartment.Text = item?.DepartmentName ?? "Necunoscut";
            }
            else
            {
                lblDepartment.Text = "Nespecificat";
            }

            // DEVICE
            if (_incident.DeviceID.HasValue)
            {
                var list = await App.Database.GetDevicesAsync();
                var item = list.FirstOrDefault(x => x.ID == _incident.DeviceID.Value);
                // Folosim DeviceName din model
                lblDevice.Text = item?.DeviceName ?? "-";
            }
            else lblDevice.Text = "-";

            // TIP INCIDENT
            if (_incident.IncidentTypeID.HasValue)
            {
                var list = await App.Database.GetIncidentTypesAsync();
                var item = list.FirstOrDefault(x => x.ID == _incident.IncidentTypeID.Value);
                // Dolosim TypeName din model
                lblTip.Text = item?.TypeName ?? "-";
            }
            else lblTip.Text = "-";
        }
        catch { }
    }

    async void OnCloseClicked(object sender, EventArgs e) => await Navigation.PopModalAsync();
}