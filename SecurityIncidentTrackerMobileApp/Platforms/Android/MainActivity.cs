using Android.App;
using Android.Content.PM;
using Android.OS;

namespace SecurityIncidentTrackerMobileApp
{
    [Activity(
    Theme = "@style/Maui.SplashTheme", // Ii spunem Android-ului să înceapă cu tema gri de splash screen definită în Resources/values/styles.xml   
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
    }
}
