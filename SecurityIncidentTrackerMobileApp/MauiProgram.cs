using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Plugin.LocalNotification;

namespace SecurityIncidentTrackerMobileApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseLocalNotification() // NOTIFICARI
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            #if ANDROID
            void ApplyLargePastilleStyle(Android.Views.View platformView, bool isMultiLine = false)
            {
                if (platformView is Android.Widget.EditText editText)
                {
                    editText.BackgroundTintList =
                        Android.Content.Res.ColorStateList.ValueOf(
                            Android.Graphics.Color.Transparent);
                }

                // FUNDALUL (gri inchis rotunjit)
                var shape = new Android.Graphics.Drawables.GradientDrawable();
                shape.SetShape(Android.Graphics.Drawables.ShapeType.Rectangle);
                shape.SetColor(Android.Graphics.Color.ParseColor("#2b2b2b"));
                shape.SetCornerRadius(15); // Rotunjire mai subtila pentru aspect dreptunghiular
                platformView.Background = shape;

                // FORTAM TEXTUL ALB ȘI MARIMEA
                if (platformView is Android.Widget.TextView textView)
                {
                    textView.SetTextColor(Android.Graphics.Color.White);
                    textView.SetHintTextColor(Android.Graphics.Color.ParseColor("#888888"));
                    textView.TextSize = 16; // Text mai mare, mai usor de citit

                    // SPATIEREA INTERNA (PADDING)
                    // Stanga: 45, Sus: 35, Dreapta: 45, Jos: 35
                    if (isMultiLine)
                        textView.SetPadding(45, 40, 45, 40);
                    else
                        textView.SetPadding(45, 35, 45, 35);
                }

                // INALTIMEA MINIMA 
                // 180 pixeli     
                platformView.SetMinimumHeight(180);
            }

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("BigBox", (handler, view) =>
            {
                ApplyLargePastilleStyle(handler.PlatformView);
            });

            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("BigBox", (handler, view) =>
            {
                ApplyLargePastilleStyle(handler.PlatformView, true);
            });

            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("BigBox", (handler, view) =>
            {
                ApplyLargePastilleStyle(handler.PlatformView);
            });

            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping("BigBox", (handler, view) =>
            {
                ApplyLargePastilleStyle(handler.PlatformView);
            });
            #endif

            #if DEBUG
            builder.Logging.AddDebug();
            #endif
            return builder.Build();
        }
    }
}