using SecurityIncidentTrackerMobileApp.Data;
using SecurityIncidentTrackerMobileApp.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Plugin.LocalNotification;

namespace SecurityIncidentTrackerMobileApp;

public partial class App : Application
{
    public static IncidentDatabase Database { get; private set; }
    HubConnection hubConnection;

    public App()
    {
        InitializeComponent();
        Database = new IncidentDatabase(new RestService());

        // Splash screen simplu
        Color darkBackground = Color.FromArgb("#2F2F2F");
        MainPage = new ContentPage
        {
            BackgroundColor = darkBackground,
            Content = new Grid
            {
                BackgroundColor = darkBackground,
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                Children =
                {
                    new Image
                    {
                        Source = "appicon.png",
                        WidthRequest = 200,
                        HeightRequest = 200,
                        Aspect = Aspect.AspectFit,
                        BackgroundColor = Colors.Transparent,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    }
                }
            }
        };
    }

    protected override async void OnStart()
    {
        base.OnStart();

        // Asteapta ca Activity-ul sa fie complet initializat
        await Task.Delay(500);

        await CheckLoginStatus();
        await ConnectToSignalR();
    }

    private async Task CheckLoginStatus()
    {
        try
        {
            // Cerem permisiunea notificarilor (DUPA ce Activity e gata)
            if (!await LocalNotificationCenter.Current.AreNotificationsEnabled())
            {
                await LocalNotificationCenter.Current.RequestNotificationPermission();
            }

            // Pauza pe splash
            await Task.Delay(1500);

            string isLoggedIn = await SecureStorage.Default.GetAsync("is_logged_in");

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (isLoggedIn == "true")
                {
                    MainPage = new AppShell();
                }
                else
                {
                    MainPage = new NavigationPage(new LoginPage())
                    {
                        BarBackgroundColor = Colors.Black,
                        BarTextColor = Colors.White
                    };
                }
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Eroare login check: {ex.Message}");

            MainThread.BeginInvokeOnMainThread(() =>
            {
                MainPage = new NavigationPage(new LoginPage())
                {
                    BarBackgroundColor = Colors.Black,
                    BarTextColor = Colors.White
                };
            });
        }
    }

    private async Task ConnectToSignalR()
    {
        try
        {
            string adresaServer = "http://10.0.2.2:5214/notificationHub";

            hubConnection = new HubConnectionBuilder()
                .WithUrl(adresaServer)
                .WithAutomaticReconnect()
                .Build();

            hubConnection.Closed += async (error) =>
            {
                Console.WriteLine("SignalR s-a închis: " + error?.Message);
            };

            hubConnection.Reconnecting += (error) =>
            {
                Console.WriteLine("SignalR reconectare...");
                return Task.CompletedTask;
            };

            hubConnection.Reconnected += (connectionId) =>
            {
                Console.WriteLine("SignalR reconectat cu ID: " + connectionId);
                return Task.CompletedTask;
            };

            hubConnection.On<string>("ReceiveNotification", async (mesaj) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    Console.WriteLine("Mesaj primit: " + mesaj);

                    MessagingCenter.Send<App>(this, "RefreshList");

                    var request = new NotificationRequest
                    {
                        NotificationId = new Random().Next(100, 1000),
                        Title = "INCIDENT NOU",
                        Description = mesaj,
                        BadgeNumber = 1,
                        Schedule = new NotificationRequestSchedule { NotifyTime = DateTime.Now }
                    };
                    await LocalNotificationCenter.Current.Show(request);
                });
            });

            await hubConnection.StartAsync();
            Console.WriteLine("SignalR conectat ✅");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare SignalR: {ex.Message}");
        }
    }
}