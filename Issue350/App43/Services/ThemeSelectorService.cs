using System;
using System.Threading.Tasks;

using App43.Helpers;

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace App43.Services
{
    public static class ThemeSelectorService
    {
        private const string SettingsKey = "RequestedTheme";

        public static event EventHandler<ElementTheme> OnThemeChanged = delegate { };

        public static bool IsLightThemeEnabled => Theme == ElementTheme.Light;
        public static ElementTheme Theme { get; set; }
        internal static ElementTheme BaseTheme { get; } = (Application.Current.RequestedTheme == ApplicationTheme.Dark) ? ElementTheme.Dark : ElementTheme.Light;
        private static readonly SolidColorBrush baseBrush = Application.Current.Resources["SystemControlForegroundBaseHighBrush"] as SolidColorBrush;
        private static readonly SolidColorBrush altBrush = Application.Current.Resources["SystemControlForegroundAltHighBrush"] as SolidColorBrush;

        public static SolidColorBrush GetSystemControlForegroundForTheme()
        {
            return UseBaseBrush() ? baseBrush : altBrush;
        }

        private static bool UseBaseBrush()
        {
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

        public static async Task SwitchThemeAsync()
        {
            if (Theme == ElementTheme.Dark)
            {
                await SetThemeAsync(ElementTheme.Light);
            }
            else
            {
                await SetThemeAsync(ElementTheme.Dark);
            }
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
            ElementTheme cacheTheme = BaseTheme;
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
