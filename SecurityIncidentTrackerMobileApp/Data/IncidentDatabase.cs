using SecurityIncidentTrackerMobileApp.Models;
using Device = SecurityIncidentTrackerMobileApp.Models.Device;

namespace SecurityIncidentTrackerMobileApp.Data
{
    public class IncidentDatabase
    {
        RestService _restService;

        public IncidentDatabase(RestService service)
        {
            _restService = service;
        }

        // CITIRE (GET) 
        public Task<List<Incident>> GetIncidentsAsync() => _restService.GetIncidentsAsync();
        public Task<List<Department>> GetDepartmentsAsync() => _restService.GetDepartmentsAsync();
        public Task<List<Device>> GetDevicesAsync() => _restService.GetDevicesAsync();
        public Task<List<Technician>> GetTechniciansAsync() => _restService.GetTechniciansAsync();
        public Task<List<IncidentType>> GetIncidentTypesAsync() => _restService.GetIncidentTypesAsync();

        // INCIDENT
        public Task SaveIncidentAsync(Incident item, bool isNewItem = false) => _restService.SaveIncidentAsync(item, isNewItem);
        public Task DeleteIncidentAsync(int id) => _restService.DeleteIncidentAsync(id);

        // TECHNICIAN
        public Task SaveTechnicianAsync(Technician item, bool isNewItem = false) => _restService.SaveTechnicianAsync(item, isNewItem);
        public Task DeleteTechnicianAsync(int id) => _restService.DeleteTechnicianAsync(id);

        // DEVICE
        public Task SaveDeviceAsync(Device item, bool isNewItem = false) => _restService.SaveDeviceAsync(item, isNewItem);
        public Task DeleteDeviceAsync(int id) => _restService.DeleteDeviceAsync(id);

        // DEPARTMENT
        public Task SaveDepartmentAsync(Department item, bool isNewItem = false) => _restService.SaveDepartmentAsync(item, isNewItem);
        public Task DeleteDepartmentAsync(int id) => _restService.DeleteDepartmentAsync(id);

        // INCIDENT TYPE
        public Task SaveIncidentTypeAsync(IncidentType item, bool isNewItem = false) => _restService.SaveIncidentTypeAsync(item, isNewItem);
        public Task DeleteIncidentTypeAsync(int id) => _restService.DeleteIncidentTypeAsync(id);

        // UTILIZATORI (LOGIN & REGISTER) 
        // Metoda care trimite emailul si parola la server si primeste user-ul daca e corect
        public Task<User> LoginAsync(string email, string password)
        {
            return _restService.LoginAsync(email, password);
        }

        // Metoda pentru inregistrare
        public Task RegisterAsync(User user)
        {
            return _restService.RegisterAsync(user);
        }

    }
}