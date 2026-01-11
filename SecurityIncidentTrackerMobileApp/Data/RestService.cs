using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using SecurityIncidentTrackerMobileApp.Models;
using Device = SecurityIncidentTrackerMobileApp.Models.Device;

namespace SecurityIncidentTrackerMobileApp.Data
{
    public class RestService
    {
        HttpClient _client;

        // CONECTARE LA API-UL REST (folosim IP-ul special pentru Android Emulator pentru a accesa localhost-ul PC-ului gazda)             
        public string RestUrl = "http://10.0.2.2:5214/";

        public RestService()
        {
            _client = new HttpClient();
            // Nu mai avem nevoie de handler-ul SSL daca folosim HTTP
        }

        // METODE GENERICE DE CITIRE (GET) 
        public Task<List<Incident>> GetIncidentsAsync() => GetItemsAsync<Incident>("api/Incidents");
        public Task<List<Department>> GetDepartmentsAsync() => GetItemsAsync<Department>("api/Departments");
        public Task<List<Device>> GetDevicesAsync() => GetItemsAsync<Device>("api/Devices");
        public Task<List<Technician>> GetTechniciansAsync() => GetItemsAsync<Technician>("api/Technicians");
        public Task<List<IncidentType>> GetIncidentTypesAsync() => GetItemsAsync<IncidentType>("api/IncidentTypes");

        private async Task<List<T>> GetItemsAsync<T>(string endpoint)
        {
            try
            {
                var response = await _client.GetAsync($"{RestUrl}{endpoint}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    // DEBUG (arată JSON-ul primit)
                    Debug.WriteLine($"JSON primit de la {endpoint}: {content.Substring(0, Math.Min(500, content.Length))}...");

                    // CONFIGURARE (pentru a ignora referințele circulare)
                    var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore,
                        // Ignoră proprietățile care nu se potrivesc
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        Error = (sender, args) =>
                        {
                            Debug.WriteLine($"Eroare deserializare: {args.ErrorContext.Error.Message}");
                            args.ErrorContext.Handled = true; // Continuă chiar dacă e eroare
                        }
                    };

                    var result = JsonConvert.DeserializeObject<List<T>>(content, settings);

                    // DEBUG (arată câte iteme au fost deserializate)
                    Debug.WriteLine($"Iteme deserializate: {result?.Count ?? 0}");

                    return result ?? new List<T>();
                }
                else
                {
                    Debug.WriteLine($"Response failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Eroare la GET {endpoint}: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
            return new List<T>();
        }

        // METODE CRUD (SAVE/DELETE) 

        // INCIDENT
        public Task SaveIncidentAsync(Incident item, bool isNewItem) => SaveItemAsync("api/Incidents", item, item.ID, isNewItem);
        public Task DeleteIncidentAsync(int id) => DeleteItemAsync("api/Incidents", id);

        // CELELALTE ENTITATI 
        public Task SaveTechnicianAsync(Technician item, bool isNewItem) => SaveItemAsync("api/Technicians", item, item.ID, isNewItem);
        public Task DeleteTechnicianAsync(int id) => DeleteItemAsync("api/Technicians", id);

        public Task SaveDeviceAsync(Device item, bool isNewItem) => SaveItemAsync("api/Devices", item, item.ID, isNewItem);
        public Task DeleteDeviceAsync(int id) => DeleteItemAsync("api/Devices", id);

        public Task SaveDepartmentAsync(Department item, bool isNewItem) => SaveItemAsync("api/Departments", item, item.ID, isNewItem);
        public Task DeleteDepartmentAsync(int id) => DeleteItemAsync("api/Departments", id);

        public Task SaveIncidentTypeAsync(IncidentType item, bool isNewItem) => SaveItemAsync("api/IncidentTypes", item, item.ID, isNewItem);
        public Task DeleteIncidentTypeAsync(int id) => DeleteItemAsync("api/IncidentTypes", id);

        // HELPERE 
        private async Task SaveItemAsync<T>(string endpoint, T item, int id, bool isNewItem)
        {
            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                if (isNewItem)
                    await _client.PostAsync($"{RestUrl}{endpoint}", content);
                else
                    await _client.PutAsync($"{RestUrl}{endpoint}/{id}", content);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        private async Task DeleteItemAsync(string endpoint, int id)
        {
            try { await _client.DeleteAsync($"{RestUrl}{endpoint}/{id}"); }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        // LOGIN & REGISTER (folosind api/Auth) 

        public async Task<User> LoginAsync(string email, string password)
        {
            try
            {
                var loginData = new { Email = email, Password = password };
                var json = JsonConvert.SerializeObject(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Folosim api/Auth/Login
                var response = await _client.PostAsync($"{RestUrl}api/Auth/Login", content);

                if (response.IsSuccessStatusCode)
                {
                    return new User { Email = email, Password = password };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Eroare Login: {ex.Message}");
            }
            return null;
        }

        public async Task RegisterAsync(User user)
        {
            try
            {
                // Trimitem doar ce are nevoie serverul (email si password)
                var registerData = new { Email = user.Email, Password = user.Password };

                var json = JsonConvert.SerializeObject(registerData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Folosim api/Auth/Register
                var response = await _client.PostAsync($"{RestUrl}api/Auth/Register", content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Eroare Register: {ex.Message}");
                throw;
            }
        }
    }
}