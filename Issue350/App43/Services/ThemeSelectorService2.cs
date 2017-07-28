using System;
using System.Threading.Tasks;

using App43.Helpers;

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace App43.Services
{
    public static class ThemeSelectorService2
    {
        private const string SettingsKey = "RequestedTheme2";

        public static event EventHandler<ElementTheme> OnThemeChanged = delegate { };

        public static ElementTheme Theme { get; set; } = ElementTheme.Default;
        internal static ElementTheme BaseTheme { get; } = (Application.Current.RequestedTheme == ApplicationTheme.Dark) ? ElementTheme.Dark : ElementTheme.Light;
        private static readonly SolidColorBrush baseBrush = Application.Current.Resources["SystemControlForegroundBaseHighBrush"] as SolidColorBrush;
        private static readonly SolidColorBrush altBrush = Application.Current.Resources["SystemControlForegroundAltHighBrush"] as SolidColorBrush;

        public static SolidColorBrush GetSystemControlForegroundForTheme()
        {
            return UseBaseBrush() ? baseBrush : altBrush;
        }

        private static bool UseBaseBrush()
        {
            if (Theme == ElementTheme.Default)
                return true;

            if (BaseTheme == ElementTheme.Dark)
            {
                // dark theme
                // Base = white
                // Alt = black
                return (Theme == ElementTheme.Dark);
            }
            else
            {
                // light theme
                // Base = black
                // Alt = white
                return (Theme == ElementTheme.Light);
            }
        }

        public static async Task InitializeAsync()
        {
            Theme = await LoadThemeFromSettingsAsync();
        }
        
        public static async Task SetThemeAsync(ElementTheme theme)
        {
            Theme = theme;

            SetRequestedTheme();
            await SaveThemeInSettingsAsync(Theme);

            OnThemeChanged(null, Theme);
        }

        public static void SetRequestedTheme()
        {
            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = Theme;
            }
        }

        private static async Task<ElementTheme> LoadThemeFromSettingsAsync()
        {
            ElementTheme cacheTheme = ElementTheme.Default;
            string themeName = await ApplicationData.Current.LocalSettings.ReadAsync<string>(SettingsKey);

            if (!string.IsNullOrEmpty(themeName))
            {
                Enum.TryParse(themeName, out cacheTheme);
            }
            
            return cacheTheme;
        }

        private static async Task SaveThemeInSettingsAsync(ElementTheme theme)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKey, theme.ToString());
        }
    }
}
